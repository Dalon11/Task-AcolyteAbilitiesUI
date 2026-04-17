using System;
using Feature.Abilities.Presentation.ViewModels;
using Feature.CharacterPaper.Presentation.ViewModels;
using Feature.Loadout.Presentation.ViewModels;
using Feature.Modifications.Presentation.ViewModels;
using Feature.Party.Presentation.ViewModels;
using Feature.Tooltip.Presentation.ViewModels;

namespace Feature.CharacterSelection.Presentation.ViewModels
{
    /// <summary>
    /// Набор зависимостей корневой ViewModel экрана выбора персонажа.
    /// </summary>
    public sealed class CharacterSelectionScreenDependencies
    {
        public CharacterSelectionScreenDependencies(
            PartyViewModel partyViewModel,
            CharacterPaperViewModel characterPaperViewModel,
            AbilitiesListViewModel abilitiesListViewModel,
            ModificationsListViewModel modificationsListViewModel,
            TooltipViewModel tooltipViewModel,
            ModificationDragSlotViewModel dragSlotViewModel,
            CharacterSelectionScreenStateCoordinator screenStateCoordinator,
            CharacterSelectionTooltipCoordinator tooltipCoordinator,
            ModificationDragAndDropCoordinator modificationDragAndDropCoordinator,
            CharacterLoadoutStateService characterLoadoutStateService)
        {
            if (partyViewModel == null)
                throw new ArgumentNullException(nameof(partyViewModel));

            if (characterPaperViewModel == null)
                throw new ArgumentNullException(nameof(characterPaperViewModel));

            if (abilitiesListViewModel == null)
                throw new ArgumentNullException(nameof(abilitiesListViewModel));

            if (modificationsListViewModel == null)
                throw new ArgumentNullException(nameof(modificationsListViewModel));

            if (tooltipViewModel == null)
                throw new ArgumentNullException(nameof(tooltipViewModel));

            if (dragSlotViewModel == null)
                throw new ArgumentNullException(nameof(dragSlotViewModel));

            if (screenStateCoordinator == null)
                throw new ArgumentNullException(nameof(screenStateCoordinator));

            if (tooltipCoordinator == null)
                throw new ArgumentNullException(nameof(tooltipCoordinator));

            if (modificationDragAndDropCoordinator == null)
                throw new ArgumentNullException(nameof(modificationDragAndDropCoordinator));

            if (characterLoadoutStateService == null)
                throw new ArgumentNullException(nameof(characterLoadoutStateService));

            PartyViewModel = partyViewModel;
            CharacterPaperViewModel = characterPaperViewModel;
            AbilitiesListViewModel = abilitiesListViewModel;
            ModificationsListViewModel = modificationsListViewModel;
            TooltipViewModel = tooltipViewModel;
            DragSlotViewModel = dragSlotViewModel;
            ScreenStateCoordinator = screenStateCoordinator;
            TooltipCoordinator = tooltipCoordinator;
            ModificationDragAndDropCoordinator = modificationDragAndDropCoordinator;
            CharacterLoadoutStateService = characterLoadoutStateService;
        }

        public PartyViewModel PartyViewModel { get; }

        public CharacterPaperViewModel CharacterPaperViewModel { get; }

        public AbilitiesListViewModel AbilitiesListViewModel { get; }

        public ModificationsListViewModel ModificationsListViewModel { get; }

        public TooltipViewModel TooltipViewModel { get; }

        public ModificationDragSlotViewModel DragSlotViewModel { get; }

        public CharacterSelectionScreenStateCoordinator ScreenStateCoordinator { get; }

        public CharacterSelectionTooltipCoordinator TooltipCoordinator { get; }

        public ModificationDragAndDropCoordinator ModificationDragAndDropCoordinator { get; }

        public CharacterLoadoutStateService CharacterLoadoutStateService { get; }
    }
}



