using System;
using Feature.Abilities.Presentation.ViewModels;
using Feature.CharacterPaper.Presentation.ViewModels;
using Feature.CharacterSelection.Core.Configs.ScriptableObjects;
using Feature.CharacterSelection.Core.Domain.Contracts;
using Feature.CharacterSelection.Core.Domain.Services;
using Feature.CharacterSelection.Core.Infrastructure.Factories;
using Feature.CharacterSelection.Core.Infrastructure.Repositories;
using Feature.CharacterSelection.Presentation.ViewModels;
using Feature.CharacterSelection.Presentation.Views;
using Feature.Common.Presentation.Pooling.Contracts;
using Feature.Common.Presentation.Pooling.Services;
using Feature.Loadout.Presentation.ViewModels;
using Feature.Modifications.Presentation.ViewModels;
using Feature.Party.Presentation.ViewModels;
using Feature.Tooltip.Presentation.ViewModels;
using UnityEngine;

namespace Feature.CharacterSelection.Composition
{
    /// <summary>
    /// ���������� ������ ������ ���������: �������� ����������� � ��������� View � ViewModel.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class CharacterSelectionScreenBootstrap : MonoBehaviour
    {
        [SerializeField] private CharacterSelectionScreenView _screenView;
        [SerializeField] private CharacterCatalog _characterCatalog;
        [SerializeField] private ModificationTypeCatalog _modificationTypeCatalog;

        private CharacterSelectionScreenViewModel _screenViewModel;
        private IComponentPoolService _componentPoolService;

        private void Awake()
        {
            CharacterSelectionScreenView screenView = ResolveScreenView();

            ICharacterRepository characterRepository = BuildCharacterRepository();
            ICharacterSelectionService characterSelectionService = new CharacterSelectionService(characterRepository);
            CharacterSelectionScreenDependencies screenDependencies = BuildScreenDependencies();

            _screenViewModel = new CharacterSelectionScreenViewModel(
                characterRepository,
                characterSelectionService,
                screenDependencies);

            _componentPoolService = new ComponentPoolService();
            screenView.SetPoolService(_componentPoolService);
            screenView.Bind(_screenViewModel);
            _screenViewModel.Initialize();
        }

        private void OnDestroy()
        {
            if (_screenView != null)
                _screenView.Unbind();

            if (_screenViewModel != null)
            {
                _screenViewModel.Dispose();
                _screenViewModel = null;
            }

            if (_componentPoolService != null)
            {
                _componentPoolService.Dispose();
                _componentPoolService = null;
            }
        }

        private CharacterSelectionScreenView ResolveScreenView()
        {
            if (_screenView == null)
                _screenView = GetComponent<CharacterSelectionScreenView>();

            if (_screenView == null)
                throw new InvalidOperationException("�� ������ CharacterSelectionScreenView ��� �������� ������ ������ ���������.");

            return _screenView;
        }

        private ICharacterRepository BuildCharacterRepository()
        {
            if (_characterCatalog == null)
                throw new InvalidOperationException("�� ����� CharacterCatalog � CharacterSelectionScreenBootstrap.");

            if (_modificationTypeCatalog == null)
                throw new InvalidOperationException("�� ����� ModificationTypeCatalog � CharacterSelectionScreenBootstrap.");

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



