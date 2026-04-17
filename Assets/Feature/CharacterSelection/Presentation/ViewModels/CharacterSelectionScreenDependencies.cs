using System;
using Feature.Abilities.Presentation.Binding.Contracts;
using Feature.CharacterPaper.Presentation.Binding.Contracts;
using Feature.CharacterSelection.Presentation.Contracts;
using Feature.Loadout.Presentation.Binding.Contracts;
using Feature.Loadout.Presentation.Contracts;
using Feature.Modifications.Presentation.Binding.Contracts;
using Feature.Party.Presentation.Binding.Contracts;
using Feature.Tooltip.Presentation.Binding.Contracts;

namespace Feature.CharacterSelection.Presentation.ViewModels
{

    /// <summary>
    /// Хранит набор зависимостей, необходимых root ViewModel экрана выбора персонажа.
    /// </summary>
    public sealed class CharacterSelectionScreenDependencies
    {
        public CharacterSelectionScreenDependencies(
            IPartyViewModel partyViewModel,
            ICharacterPaperViewModel characterPaperViewModel,
            IAbilitiesListViewModel abilitiesListViewModel,
            IModificationsListViewModel modificationsListViewModel,
            ITooltipViewModel tooltipViewModel,
            IModificationDragSlotViewModel dragSlotViewModel,
            ICharacterSelectionScreenStateCoordinator screenStateCoordinator,
            ICharacterSelectionTooltipCoordinator tooltipCoordinator,
            IModificationDragAndDropCoordinator modificationDragAndDropCoordinator,
            ICharacterLoadoutStateService characterLoadoutStateService)
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

        public IPartyViewModel PartyViewModel { get; }

        public ICharacterPaperViewModel CharacterPaperViewModel { get; }

        public IAbilitiesListViewModel AbilitiesListViewModel { get; }

        public IModificationsListViewModel ModificationsListViewModel { get; }

        public ITooltipViewModel TooltipViewModel { get; }

        public IModificationDragSlotViewModel DragSlotViewModel { get; }

        public ICharacterSelectionScreenStateCoordinator ScreenStateCoordinator { get; }

        public ICharacterSelectionTooltipCoordinator TooltipCoordinator { get; }

        public IModificationDragAndDropCoordinator ModificationDragAndDropCoordinator { get; }

        public ICharacterLoadoutStateService CharacterLoadoutStateService { get; }
    }
}
