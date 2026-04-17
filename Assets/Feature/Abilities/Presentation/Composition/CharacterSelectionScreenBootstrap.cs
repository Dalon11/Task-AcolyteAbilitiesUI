using System;
using Feature.Abilities.Configs.ScriptableObjects;
using Feature.Abilities.Domain.Contracts;
using Feature.Abilities.Domain.Services;
using Feature.Abilities.Infrastructure.Factories;
using Feature.Abilities.Infrastructure.Repositories;
using Feature.Abilities.Presentation.ViewModels;
using Feature.Abilities.Presentation.Views;
using UnityEngine;

namespace Feature.Abilities.Presentation.Composition
{
    /// <summary>
    /// Композиция экрана выбора персонажа: собирает зависимости и связывает View с ViewModel.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class CharacterSelectionScreenBootstrap : MonoBehaviour
    {
        [SerializeField] private CharacterSelectionScreenView _screenView;
        [SerializeField] private CharacterCatalog _characterCatalog;
        [SerializeField] private ModificationTypeCatalog _modificationTypeCatalog;

        private CharacterSelectionScreenViewModel _screenViewModel;

        private void Awake()
        {
            CharacterSelectionScreenView screenView = ResolveScreenView();

            ICharacterRepository characterRepository = BuildCharacterRepository();
            ICharacterSelectionService characterSelectionService = new CharacterSelectionService(characterRepository);

            _screenViewModel = new CharacterSelectionScreenViewModel(
                characterRepository,
                characterSelectionService);

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
        }

        private CharacterSelectionScreenView ResolveScreenView()
        {
            if (_screenView == null)
                _screenView = GetComponent<CharacterSelectionScreenView>();

            if (_screenView == null)
                throw new InvalidOperationException("Не найден CharacterSelectionScreenView для биндинга экрана выбора персонажа.");

            return _screenView;
        }

        private ICharacterRepository BuildCharacterRepository()
        {
            if (_characterCatalog == null)
                throw new InvalidOperationException("Не задан CharacterCatalog в CharacterSelectionScreenBootstrap.");

            if (_modificationTypeCatalog == null)
                throw new InvalidOperationException("Не задан ModificationTypeCatalog в CharacterSelectionScreenBootstrap.");

            IModificationTypePresentationRepository modificationTypePresentationRepository =
                new ModificationTypePresentationRepository(_modificationTypeCatalog);

            ICharacterFactory characterFactory = new CharacterFactory(modificationTypePresentationRepository);
            ICharacterRepository characterRepository = new CharacterRepository(_characterCatalog, characterFactory);

            return characterRepository;
        }
    }
}
