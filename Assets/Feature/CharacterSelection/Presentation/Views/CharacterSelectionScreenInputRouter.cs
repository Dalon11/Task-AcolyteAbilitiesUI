using System;
using Feature.CharacterSelection.Presentation.Binding.Contracts;
using Feature.Tooltip.Presentation.Configs;
using Feature.Tooltip.Presentation.Views;
using UnityEngine.EventSystems;

namespace Feature.CharacterSelection.Presentation.Views
{

    /// <summary>
    /// Маршрутизирует пользовательский ввод экрана выбора персонажа в методы root ViewModel.
    /// </summary>
    public sealed class CharacterSelectionScreenInputRouter
    {
        private readonly CharacterSelectionTooltipHoverInputAdapter _tooltipHoverInputAdapter;
        private readonly CharacterSelectionDropTargetInputAdapter _dropTargetInputAdapter;

        private ICharacterSelectionScreenViewModel _viewModel;

        public CharacterSelectionScreenInputRouter(TooltipHoverDelayConfig tooltipHoverDelayConfig)
        {
            if (tooltipHoverDelayConfig == null)
                throw new ArgumentNullException(nameof(tooltipHoverDelayConfig));

            _tooltipHoverInputAdapter = new CharacterSelectionTooltipHoverInputAdapter(
                tooltipHoverDelayConfig.HoverDelaySeconds,
                tooltipHoverDelayConfig.MouseMovementThreshold);
            _dropTargetInputAdapter = new CharacterSelectionDropTargetInputAdapter();
        }

        public void SetViewModel(ICharacterSelectionScreenViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool HasPendingHover => _viewModel != null && _tooltipHoverInputAdapter.HasPendingHover;

        public void ClearViewModel()
        {
            CancelPendingTooltipHover();
            _viewModel = null;
        }

        public void ProcessPendingHover()
        {
            if (_viewModel == null)
                return;

            _tooltipHoverInputAdapter.Process(ShowPendingTooltip);
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

            _tooltipHoverInputAdapter.StartPending(
                TooltipHoverDelayCoordinator.HoverTargetType.Character,
                string.Empty);
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

            _tooltipHoverInputAdapter.StartPending(
                TooltipHoverDelayCoordinator.HoverTargetType.Ability,
                abilityId);
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

            string targetAbilityId = _dropTargetInputAdapter.ResolveAbilityIdUnderPointer(eventData);
            _viewModel.OnAbilityPointerUp(targetAbilityId);
        }

        public void HandleModificationHoverEnter(string modificationId)
        {
            if (_viewModel == null)
                return;

            _tooltipHoverInputAdapter.StartPending(
                TooltipHoverDelayCoordinator.HoverTargetType.Modification,
                modificationId);
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

            string abilityId = _dropTargetInputAdapter.ResolveAbilityIdUnderPointer(eventData);
            _viewModel.OnModificationPointerUp(abilityId);
        }

        private void CancelPendingTooltipHover()
        {
            _tooltipHoverInputAdapter.Cancel();
        }

        private void ShowPendingTooltip(TooltipHoverDelayCoordinator.HoverTargetType targetType, string targetId)
        {
            if (_viewModel == null)
                return;

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
