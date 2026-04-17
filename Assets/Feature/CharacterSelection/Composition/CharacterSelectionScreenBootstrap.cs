using System;
using Feature.CharacterSelection.Core.Configs.ScriptableObjects;
using Feature.CharacterSelection.Presentation.Views;
using UnityEngine;

namespace Feature.CharacterSelection.Composition
{

    [DisallowMultipleComponent]
    /// <summary>
    /// Инициализирует сценарий Character Selection Screen в контексте сцены.
    /// </summary>
    public sealed class CharacterSelectionScreenBootstrap : MonoBehaviour
    {
        [SerializeField] private CharacterSelectionScreenView _screenView;
        [SerializeField] private CharacterCatalog _characterCatalog;
        [SerializeField] private ModificationTypeCatalog _modificationTypeCatalog;

        private CharacterSelectionScreenComposition _composition;

        private void Awake()
        {
            CharacterSelectionScreenView screenView = ResolveScreenView();
            CharacterSelectionScreenComposer composer = new CharacterSelectionScreenComposer(
                _characterCatalog,
                _modificationTypeCatalog);

            _composition = composer.Compose();
            screenView.SetPoolService(_composition.ComponentPoolService);
            screenView.Bind(_composition.ScreenViewModel);
            _composition.ScreenViewModel.Initialize();
        }

        private void OnDestroy()
        {
            if (_screenView != null)
                _screenView.Unbind();

            if (_composition != null)
            {
                _composition.Dispose();
                _composition = null;
            }
        }

        private CharacterSelectionScreenView ResolveScreenView()
        {
            if (_screenView == null)
                _screenView = GetComponent<CharacterSelectionScreenView>();

            if (_screenView == null)
                throw new InvalidOperationException("Не найден CharacterSelectionScreenView рядом с CharacterSelectionScreenBootstrap.");

            return _screenView;
        }
    }
}
