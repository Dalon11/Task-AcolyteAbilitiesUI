using System;
using Feature.Abilities.Presentation.ViewModels;
using Feature.CharacterPaper.Presentation.ViewModels;
using Feature.CharacterSelection.Core.Configs.ScriptableObjects;
using Feature.CharacterSelection.Core.Domain.Contracts;
using Feature.CharacterSelection.Core.Domain.Services;
using Feature.CharacterSelection.Core.Infrastructure.Factories;
using Feature.CharacterSelection.Core.Infrastructure.Repositories;
using Feature.CharacterSelection.Presentation.ViewModels;
using Feature.Common.Presentation.Pooling.Contracts;
using Feature.Common.Presentation.Pooling.Services;
using Feature.Loadout.Presentation.ViewModels;
using Feature.Modifications.Presentation.ViewModels;
using Feature.Party.Presentation.ViewModels;
using Feature.Tooltip.Presentation.ViewModels;

namespace Feature.CharacterSelection.Composition
{

    /// <summary>
    /// Собирает зависимости для сценария Character Selection Screen.
    /// </summary>
    public sealed class CharacterSelectionScreenComposer
    {
        private readonly CharacterCatalog _characterCatalog;
        private readonly ModificationTypeCatalog _modificationTypeCatalog;

        public CharacterSelectionScreenComposer(
            CharacterCatalog characterCatalog,
            ModificationTypeCatalog modificationTypeCatalog)
        {
            if (characterCatalog == null)
                throw new ArgumentNullException(nameof(characterCatalog));

            if (modificationTypeCatalog == null)
                throw new ArgumentNullException(nameof(modificationTypeCatalog));

            _characterCatalog = characterCatalog;
            _modificationTypeCatalog = modificationTypeCatalog;
        }

        public CharacterSelectionScreenComposition Compose()
        {
            ICharacterRepository characterRepository = BuildCharacterRepository();
            ICharacterSelectionService characterSelectionService = new CharacterSelectionService(characterRepository);
            CharacterSelectionScreenDependencies screenDependencies = BuildScreenDependencies();
            CharacterSelectionScreenViewModel screenViewModel = new CharacterSelectionScreenViewModel(
                characterRepository,
                characterSelectionService,
                screenDependencies);
            IComponentPoolService componentPoolService = new ComponentPoolService();

            return new CharacterSelectionScreenComposition(screenViewModel, componentPoolService);
        }

        private ICharacterRepository BuildCharacterRepository()
        {
            IModificationTypePresentationRepository modificationTypePresentationRepository =
                new ModificationTypePresentationRepository(_modificationTypeCatalog);

            ICharacterFactory characterFactory = new CharacterFactory(modificationTypePresentationRepository);
            ICharacterRepository characterRepository = new CharacterRepository(_characterCatalog, characterFactory);

            return characterRepository;
        }

        private CharacterSelectionScreenDependencies BuildScreenDependencies()
        {
            PartyViewModel partyViewModel = new PartyViewModel();
            CharacterPaperViewModel characterPaperViewModel = new CharacterPaperViewModel();
            AbilitiesListViewModel abilitiesListViewModel = new AbilitiesListViewModel();
            ModificationsListViewModel modificationsListViewModel = new ModificationsListViewModel();
            TooltipViewModel tooltipViewModel = new TooltipViewModel();
            ModificationDragSlotViewModel dragSlotViewModel = new ModificationDragSlotViewModel();
            CharacterLoadoutStateService characterLoadoutStateService = new CharacterLoadoutStateService();

            CharacterSelectionScreenStateCoordinator screenStateCoordinator = new CharacterSelectionScreenStateCoordinator(
                partyViewModel,
                characterPaperViewModel,
                abilitiesListViewModel,
                modificationsListViewModel);
            CharacterSelectionTooltipCoordinator tooltipCoordinator = new CharacterSelectionTooltipCoordinator(
                characterPaperViewModel,
                abilitiesListViewModel,
                modificationsListViewModel,
                tooltipViewModel);
            ModificationDragAndDropCoordinator modificationDragAndDropCoordinator = new ModificationDragAndDropCoordinator(
                abilitiesListViewModel,
                modificationsListViewModel,
                dragSlotViewModel);

            return new CharacterSelectionScreenDependencies(
                partyViewModel,
                characterPaperViewModel,
                abilitiesListViewModel,
                modificationsListViewModel,
                tooltipViewModel,
                dragSlotViewModel,
                screenStateCoordinator,
                tooltipCoordinator,
                modificationDragAndDropCoordinator,
                characterLoadoutStateService);
        }
    }
}
