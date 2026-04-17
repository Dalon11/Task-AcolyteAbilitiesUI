using System;
using Feature.CharacterSelection.Presentation.ViewModels;
using Feature.Common.Presentation.Pooling.Contracts;

namespace Feature.CharacterSelection.Composition
{

    /// <summary>
    /// Содержит собранные зависимости и управляет их жизненным циклом для сценария Character Selection Screen.
    /// </summary>
    public sealed class CharacterSelectionScreenComposition : IDisposable
    {
        public CharacterSelectionScreenComposition(
            CharacterSelectionScreenViewModel screenViewModel,
            IComponentPoolService componentPoolService)
        {
            if (screenViewModel == null)
                throw new ArgumentNullException(nameof(screenViewModel));

            if (componentPoolService == null)
                throw new ArgumentNullException(nameof(componentPoolService));

            ScreenViewModel = screenViewModel;
            ComponentPoolService = componentPoolService;
        }

        public CharacterSelectionScreenViewModel ScreenViewModel { get; }

        public IComponentPoolService ComponentPoolService { get; }

        public void Dispose()
        {
            ScreenViewModel.Dispose();
            ComponentPoolService.Dispose();
        }
    }
}
