using System;
using Feature.Abilities.Presentation.Views;
using Feature.CharacterPaper.Presentation.Views;
using Feature.CharacterSelection.Presentation.Binding.Contracts;
using Feature.Common.Presentation.Pooling.Contracts;
using Feature.Loadout.Presentation.Views;
using Feature.Modifications.Presentation.Views;
using Feature.Party.Presentation.Views;
using Feature.Tooltip.Presentation.Configs;
using Feature.Tooltip.Presentation.Views;
using UnityEngine;

namespace Feature.CharacterSelection.Presentation.Views
{

    /// <summary>
    /// Отвечает за отображение и обработку UI-блока Character Selection Screen.
    /// </summary>
    public sealed class CharacterSelectionScreenView : MonoBehaviour
    {
        [SerializeField] private PartyListView _partyListView;
        [SerializeField] private CharacterPaperView _characterPaperView;
        [SerializeField] private AbilitiesListView _abilitiesListView;
        [SerializeField] private ModificationsListView _modificationsListView;
        [SerializeField] private DraggedModificationSlotView _draggedModificationSlotView;
        [Space]
        [SerializeField] private TooltipView _tooltipView;

        private ICharacterSelectionScreenViewModel _viewModel;
        private IComponentPoolService _componentPoolService;
        private TooltipHoverDelayConfig _tooltipHoverDelayConfig;
        private CharacterSelectionScreenInputRouter _inputRouter;
        private CharacterSelectionScreenBindingsCoordinator _bindingsCoordinator;

        private void Awake()
        {
            _bindingsCoordinator = new CharacterSelectionScreenBindingsCoordinator(
                _partyListView,
                _characterPaperView,
                _abilitiesListView,
                _modificationsListView,
                _tooltipView,
                _draggedModificationSlotView);
        }

        public void SetTooltipHoverDelayConfig(TooltipHoverDelayConfig tooltipHoverDelayConfig)
        {
            if (tooltipHoverDelayConfig == null)
                throw new ArgumentNullException(nameof(tooltipHoverDelayConfig));

            _tooltipHoverDelayConfig = tooltipHoverDelayConfig;
            _inputRouter = new CharacterSelectionScreenInputRouter(_tooltipHoverDelayConfig);
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
                throw new InvalidOperationException("Не задан IComponentPoolService для CharacterSelectionScreenView.");

            if (_inputRouter == null)
                throw new InvalidOperationException("Не задан TooltipHoverDelayConfig для CharacterSelectionScreenView.");

            Unbind();

            _viewModel = viewModel;
            _inputRouter.SetViewModel(viewModel);
            _bindingsCoordinator.Bind(
                viewModel,
                _componentPoolService,
                _inputRouter.HandlePartyCharacterClick,
                _inputRouter.HandleCharacterHoverEnter,
                _inputRouter.HandleCharacterHoverExit,
                _inputRouter.HandleAbilityHoverEnter,
                _inputRouter.HandleAbilityHoverExit,
                _inputRouter.HandleAbilityPointerDown,
                _inputRouter.HandleAbilityPointerUp,
                _inputRouter.HandleModificationHoverEnter,
                _inputRouter.HandleModificationHoverExit,
                _inputRouter.HandleModificationPointerDown,
                _inputRouter.HandleModificationPointerUp);
            _bindingsCoordinator.Refresh();
        }

        public void Unbind()
        {
            if (_viewModel == null)
                return;

            _bindingsCoordinator.Unbind();
            _inputRouter.ClearViewModel();
            _viewModel = null;
        }

        private void OnDestroy()
        {
            Unbind();
        }

        private void Update()
        {
            if (_viewModel == null)
                return;

            if (!_inputRouter.HasPendingHover)
                return;

            _inputRouter.ProcessPendingHover();
        }
    }
}
