using System;
using System.Collections.Generic;
using Feature.Abilities.Presentation.Binding.Contracts;
using Feature.Common.Presentation.Pooling.Contracts;
using Feature.Common.Presentation.Pooling.Services;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Feature.Abilities.Presentation.Views
{
    /// <summary>
    /// Тонкий bind-слой экрана выбора персонажа.
    /// </summary>
    public sealed class CharacterSelectionScreenView : MonoBehaviour
    {
        private enum HoverTargetType
        {
            None = 0,
            Character = 1,
            Ability = 2,
            Modification = 3
        }

        [SerializeField] private PartyListView _partyListView;
        [SerializeField] private CharacterPaperView _characterPaperView;
        [SerializeField] private AbilitiesListView _abilitiesListView;
        [SerializeField] private ModificationsListView _modificationsListView;
        [SerializeField] private TooltipView _tooltipView;
        [SerializeField] private DraggedModificationSlotView _draggedModificationSlotView;
        [SerializeField] private float _tooltipHoverDelaySeconds = 1f;
        [SerializeField] private float _mouseMovementThreshold = 2f;

        private readonly List<RaycastResult> _raycastResults = new List<RaycastResult>();

        private ICharacterSelectionScreenViewModel _viewModel;
        private IComponentPoolService _componentPoolService;
        private HoverTargetType _pendingHoverTargetType;
        private string _pendingHoverTargetId;
        private bool _isHoverPending;
        private bool _isHoverTooltipShown;
        private float _hoverStationaryStartTime;
        private Vector2 _lastMousePosition;

        public void Bind(ICharacterSelectionScreenViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            Unbind();
            EnsurePoolService();

            _viewModel = viewModel;

            if (_abilitiesListView != null)
                _abilitiesListView.SetPoolService(_componentPoolService);

            if (_modificationsListView != null)
                _modificationsListView.SetPoolService(_componentPoolService);

            if (_partyListView != null)
                _partyListView.Bind(_viewModel.Party, HandlePartyCharacterClick);

            if (_characterPaperView != null)
            {
                _characterPaperView.Bind(_viewModel.CharacterPaper);
                _characterPaperView.SetInputHandlers(HandleCharacterHoverEnter, HandleCharacterHoverExit);
            }

            if (_abilitiesListView != null)
            {
                _abilitiesListView.Bind(
                    _viewModel.Abilities,
                    HandleAbilityHoverEnter,
                    HandleAbilityHoverExit,
                    HandleAbilityPointerDown,
                    HandleAbilityPointerUp);
            }

            if (_modificationsListView != null)
            {
                _modificationsListView.Bind(
                    _viewModel.Modifications,
                    HandleModificationHoverEnter,
                    HandleModificationHoverExit,
                    HandleModificationPointerDown,
                    HandleModificationPointerUp);
            }

            if (_tooltipView != null)
                _tooltipView.Bind(_viewModel.Tooltip);

            if (_draggedModificationSlotView != null)
                _draggedModificationSlotView.Bind(_viewModel.DragSlot);

            Refresh();
        }

        public void Unbind()
        {
            if (_viewModel == null)
                return;

            if (_partyListView != null)
                _partyListView.Unbind();

            if (_characterPaperView != null)
                _characterPaperView.Unbind();

            if (_abilitiesListView != null)
                _abilitiesListView.Unbind();

            if (_modificationsListView != null)
                _modificationsListView.Unbind();

            if (_tooltipView != null)
                _tooltipView.Unbind();

            if (_draggedModificationSlotView != null)
                _draggedModificationSlotView.Unbind();

            CancelPendingTooltipHover();
            _viewModel = null;
        }

        public void HandlePartyCharacterClick(string characterId)
        {
            if (_viewModel == null)
                return;

            CancelPendingTooltipHover();
            _viewModel.OnPartyCharacterClick(characterId);
        }

        public void HandleCharacterHoverEnter()
        {
            if (_viewModel == null)
                return;

            StartPendingTooltipHover(HoverTargetType.Character, string.Empty);
        }

        public void HandleCharacterHoverExit()
        {
            if (_viewModel == null)
                return;

            CancelPendingTooltipHover();
            _viewModel.OnCharacterHoverExit();
        }

        public void HandleAbilityHoverEnter(string abilityId)
        {
            if (_viewModel == null)
                return;

            StartPendingTooltipHover(HoverTargetType.Ability, abilityId);
        }

        public void HandleAbilityHoverExit(string abilityId)
        {
            if (_viewModel == null)
                return;

            CancelPendingTooltipHover();
            _viewModel.OnAbilityHoverExit(abilityId);
        }

        public void HandleAbilityPointerDown(string abilityId, PointerEventData eventData)
        {
            _ = eventData;

            if (_viewModel == null)
                return;

            CancelPendingTooltipHover();
            _viewModel.OnAbilityPointerDown(abilityId);
        }

        public void HandleAbilityPointerUp(string abilityId, PointerEventData eventData)
        {
            _ = abilityId;

            if (_viewModel == null)
                return;

            string targetAbilityId = ResolveAbilityIdUnderPointer(eventData);
            _viewModel.OnAbilityPointerUp(targetAbilityId);
        }

        public void HandleModificationHoverEnter(string modificationId)
        {
            if (_viewModel == null)
                return;

            StartPendingTooltipHover(HoverTargetType.Modification, modificationId);
        }

        public void HandleModificationHoverExit(string modificationId)
        {
            if (_viewModel == null)
                return;

            CancelPendingTooltipHover();
            _viewModel.OnModificationHoverExit(modificationId);
        }

        public void HandleModificationPointerDown(string modificationId, PointerEventData eventData)
        {
            _ = eventData;

            if (_viewModel == null)
                return;

            CancelPendingTooltipHover();
            _viewModel.OnModificationPointerDown(modificationId);
        }

        public void HandleModificationPointerUp(string modificationId, PointerEventData eventData)
        {
            _ = modificationId;

            if (_viewModel == null)
                return;

            string abilityId = ResolveAbilityIdUnderPointer(eventData);
            _viewModel.OnModificationPointerUp(abilityId);
        }

        private void OnDestroy()
        {
            Unbind();

            if (_componentPoolService != null)
            {
                _componentPoolService.Dispose();
                _componentPoolService = null;
            }
        }

        private void Update()
        {
            if (_viewModel == null)
                return;

            ProcessPendingTooltipHover();
        }

        private void Refresh()
        {
            if (_viewModel == null)
                return;

            if (_partyListView != null)
                _partyListView.Refresh();

            if (_characterPaperView != null)
                _characterPaperView.Refresh();

            if (_abilitiesListView != null)
                _abilitiesListView.Refresh();

            if (_modificationsListView != null)
                _modificationsListView.Refresh();

            if (_tooltipView != null)
                _tooltipView.Refresh();

            if (_draggedModificationSlotView != null)
                _draggedModificationSlotView.Refresh();
        }

        private void EnsurePoolService()
        {
            if (_componentPoolService == null)
                _componentPoolService = new ComponentPoolService();
        }

        private string ResolveAbilityIdUnderPointer(PointerEventData eventData)
        {
            if (eventData == null)
                return string.Empty;

            EventSystem eventSystem = EventSystem.current;
            if (eventSystem == null)
                return string.Empty;

            _raycastResults.Clear();
            eventSystem.RaycastAll(eventData, _raycastResults);

            for (int i = 0; i < _raycastResults.Count; i++)
            {
                RaycastResult raycastResult = _raycastResults[i];
                if (raycastResult.gameObject == null)
                    continue;

                AbilityItemView abilityItemView = raycastResult.gameObject.GetComponentInParent<AbilityItemView>();
                if (abilityItemView == null)
                    continue;

                string abilityId = abilityItemView.AbilityId;
                if (string.IsNullOrWhiteSpace(abilityId))
                    continue;

                return abilityId;
            }

            return string.Empty;
        }

        private void StartPendingTooltipHover(HoverTargetType targetType, string targetId)
        {
            _pendingHoverTargetType = targetType;
            _pendingHoverTargetId = targetId ?? string.Empty;
            _isHoverPending = true;
            _isHoverTooltipShown = false;
            _hoverStationaryStartTime = Time.unscaledTime;
            _lastMousePosition = UnityEngine.Input.mousePosition;
        }

        private void CancelPendingTooltipHover()
        {
            _pendingHoverTargetType = HoverTargetType.None;
            _pendingHoverTargetId = string.Empty;
            _isHoverPending = false;
            _isHoverTooltipShown = false;
            _hoverStationaryStartTime = 0f;
            _lastMousePosition = Vector2.zero;
        }

        private void ProcessPendingTooltipHover()
        {
            if (!_isHoverPending || _isHoverTooltipShown)
                return;

            Vector2 currentMousePosition = UnityEngine.Input.mousePosition;
            Vector2 mouseDelta = currentMousePosition - _lastMousePosition;
            float movementThreshold = Mathf.Max(0f, _mouseMovementThreshold);
            float movementThresholdSqr = movementThreshold * movementThreshold;
            if (mouseDelta.sqrMagnitude > movementThresholdSqr)
            {
                _lastMousePosition = currentMousePosition;
                _hoverStationaryStartTime = Time.unscaledTime;
                return;
            }

            float hoverDelaySeconds = Mathf.Max(0f, _tooltipHoverDelaySeconds);
            if (Time.unscaledTime - _hoverStationaryStartTime < hoverDelaySeconds)
                return;

            _isHoverTooltipShown = true;
            ShowPendingTooltip();
        }

        private void ShowPendingTooltip()
        {
            switch (_pendingHoverTargetType)
            {
                case HoverTargetType.Character:
                    _viewModel.OnCharacterHoverEnter();
                    break;
                case HoverTargetType.Ability:
                    _viewModel.OnAbilityHoverEnter(_pendingHoverTargetId);
                    break;
                case HoverTargetType.Modification:
                    _viewModel.OnModificationHoverEnter(_pendingHoverTargetId);
                    break;
                default:
                    break;
            }
        }
    }
}
