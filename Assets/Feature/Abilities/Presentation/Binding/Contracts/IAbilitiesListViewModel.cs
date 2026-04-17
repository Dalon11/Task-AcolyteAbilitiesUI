using System;
using System.Collections.Generic;
using Feature.Loadout.Presentation.ViewModels;

namespace Feature.Abilities.Presentation.Binding.Contracts
{
    public interface IAbilitiesListViewModel
    {
        public event Action onStateChanged;

        public IReadOnlyList<IAbilityItemViewModel> Items { get; }

        public IReadOnlyList<AbilityModificationPlacementState> GetCurrentPlacements();
    }
}
