using System;
using Feature.CharacterSelection.Presentation.Binding.Contracts;
using Feature.Common.Presentation.Pooling.Contracts;
using Feature.Loadout.Presentation.Views;
using Feature.Modifications.Presentation.Views;
using Feature.Party.Presentation.Views;
using Feature.Abilities.Presentation.Views;
using Feature.CharacterPaper.Presentation.Views;
using Feature.Tooltip.Presentation.Views;
using UnityEngine.EventSystems;

namespace Feature.CharacterSelection.Presentation.Views
{

    /// <summary>
    /// Координирует взаимодействие компонентов в сценарии Character Selection Screen Bindings.
    /// </summary>
    public sealed class CharacterSelectionScreenBindingsCoordinator
    {
        private readonly PartyListView _partyListView;
        private readonly CharacterPaperView _characterPaperView;
        private readonly AbilitiesListView _abilitiesListView;
        private readonly ModificationsListView _modificationsListView;
        private readonly TooltipView _tooltipView;
        private readonly DraggedModificationSlotView _draggedModificationSlotView;

        public CharacterSelectionScreenBindingsCoordinator(
            PartyListView partyListView,
            CharacterPaperView characterPaperView,
            AbilitiesListView abilitiesListView,
            ModificationsListView modificationsListView,
            TooltipView tooltipView,
            DraggedModificationSlotView draggedModificationSlotView)
        {
            _partyListView = partyListView;
            _characterPaperView = characterPaperView;
            _abilitiesListView = abilitiesListView;
            _modificationsListView = modificationsListView;
            _tooltipView = tooltipView;
            _draggedModificationSlotView = draggedModificationSlotView;
        }

        public void Bind(
            ICharacterSelectionScreenViewModel viewModel,
            IComponentPoolService componentPoolService,
            Action<string> onPartyCharacterClick,
            Action onCharacterHoverEnter,
            Action onCharacterHoverExit,
            Action<string> onAbilityHoverEnter,
            Action<string> onAbilityHoverExit,
            Action<string, PointerEventData> onAbilityPointerDown,
            Action<string, PointerEventData> onAbilityPointerUp,
            Action<string> onModificationHoverEnter,
            Action<string> onModificationHoverExit,
            Action<string, PointerEventData> onModificationPointerDown,
            Action<string, PointerEventData> onModificationPointerUp)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            if (componentPoolService == null)
                throw new ArgumentNullException(nameof(componentPoolService));

            if (_abilitiesListView != null)
                _abilitiesListView.SetPoolService(componentPoolService);

            if (_modificationsListView != null)
                _modificationsListView.SetPoolService(componentPoolService);

            if (_partyListView != null)
                _partyListView.Bind(viewModel.Party, onPartyCharacterClick);

            if (_characterPaperView != null)
            {
                _characterPaperView.Bind(viewModel.CharacterPaper);
                _characterPaperView.SetInputHandlers(onCharacterHoverEnter, onCharacterHoverExit);
            }

            if (_abilitiesListView != null)
            {
                _abilitiesListView.Bind(
                    viewModel.Abilities,
                    onAbilityHoverEnter,
                    onAbilityHoverExit,
                    onAbilityPointerDown,
                    onAbilityPointerUp);
            }

            if (_modificationsListView != null)
            {
                _modificationsListView.Bind(
                    viewModel.Modifications,
                    onModificationHoverEnter,
                    onModificationHoverExit,
                    onModificationPointerDown,
                    onModificationPointerUp);
            }

            if (_tooltipView != null)
                _tooltipView.Bind(viewModel.Tooltip);

            if (_draggedModificationSlotView != null)
                _draggedModificationSlotView.Bind(viewModel.DragSlot);
        }

        public void Unbind()
        {
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
        }

        public void Refresh()
        {
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
    }
}
