using System;
using Feature.Abilities.Presentation.Views;
using Feature.CharacterPaper.Presentation.Views;
using Feature.CharacterSelection.Presentation.Binding.Contracts;
using Feature.Common.Presentation.Pooling.Contracts;
using Feature.Loadout.Presentation.Views;
using Feature.Modifications.Presentation.Views;
using Feature.Party.Presentation.Views;
using Feature.Tooltip.Presentation.Views;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Feature.CharacterSelection.Presentation.Views
{
    /// <summary>
    /// ������ bind-���� ������ ������ ���������.
    /// </summary>
    public sealed class CharacterSelectionScreenView : MonoBehaviour
    {
        [SerializeField] private PartyListView _partyListView;
        [SerializeField] private CharacterPaperView _characterPaperView;
        [SerializeField] private AbilitiesListView _abilitiesListView;
        [SerializeField] private ModificationsListView _modificationsListView;
        [SerializeField] private TooltipView _tooltipView;
        [SerializeField] private DraggedModificationSlotView _draggedModificationSlotView;
        [SerializeField] private float _tooltipHoverDelaySeconds = 1f;
        [SerializeField] private float _mouseMovementThreshold = 2f;

        private ICharacterSelectionScreenViewModel _viewModel;
        private IComponentPoolService _componentPoolService;
        private TooltipHoverDelayCoordinator _tooltipHoverDelayCoordinator;
        private AbilityDropTargetResolver _abilityDropTargetResolver;

        private void Awake()
        {
            _tooltipHoverDelayCoordinator =
                new TooltipHoverDelayCoordinator(_tooltipHoverDelaySeconds, _mouseMovementThreshold);
            _abilityDropTargetResolver = new AbilityDropTargetResolver();
        }

        public void SetPoolService(IComponentPoolService componentPoolService)
        {
            if (componentPoolService == null)
                throw new ArgumentNullException(nameof(componentPoolService));

            _componentPoolService = componentPoolService;
        }

        public void Bind(ICharacterSelectionScreenViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            if (_componentPoolService == null)
                throw new InvalidOperationException("�� �������� ����� ��� ����������� ��� CharacterSelectionScreenView.");

            Unbind();

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

            StartPendingTooltipHover(TooltipHoverDelayCoordinator.HoverTargetType.Character, string.Empty);
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

            StartPendingTooltipHover(TooltipHoverDelayCoordinator.HoverTargetType.Ability, abilityId);
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

            StartPendingTooltipHover(TooltipHoverDelayCoordinator.HoverTargetType.Modification, modificationId);
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

        private string ResolveAbilityIdUnderPointer(PointerEventData eventData)
        {
            if (_abilityDropTargetResolver == null)
                return string.Empty;

            return _abilityDropTargetResolver.ResolveAbilityIdUnderPointer(eventData);
        }

        private void StartPendingTooltipHover(TooltipHoverDelayCoordinator.HoverTargetType targetType, string targetId)
        {
            if (_tooltipHoverDelayCoordinator == null)
                return;

            _tooltipHoverDelayCoordinator.StartPending(
                targetType,
                targetId,
                UnityEngine.Input.mousePosition,
                Time.unscaledTime);
        }

        private void CancelPendingTooltipHover()
        {
            if (_tooltipHoverDelayCoordinator == null)
                return;

            _tooltipHoverDelayCoordinator.Cancel();
        }

        private void ProcessPendingTooltipHover()
        {
            if (_tooltipHoverDelayCoordinator == null)
                return;

            _tooltipHoverDelayCoordinator.Process(
                UnityEngine.Input.mousePosition,
                Time.unscaledTime,
                ShowPendingTooltip);
        }

        private void ShowPendingTooltip(TooltipHoverDelayCoordinator.HoverTargetType targetType, string targetId)
        {
            switch (targetType)
            {
                case TooltipHoverDelayCoordinator.HoverTargetType.Character:
                    _viewModel.OnCharacterHoverEnter();
                    break;
                case TooltipHoverDelayCoordinator.HoverTargetType.Ability:
                    _viewModel.OnAbilityHoverEnter(targetId);
                    break;
                case TooltipHoverDelayCoordinator.HoverTargetType.Modification:
                    _viewModel.OnModificationHoverEnter(targetId);
                    break;
                default:
                    break;
            }
        }
    }
}



