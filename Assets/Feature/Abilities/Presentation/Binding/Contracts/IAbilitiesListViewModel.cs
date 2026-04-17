using System;
using System.Collections.Generic;
using Feature.Loadout.Presentation.ViewModels;

namespace Feature.Abilities.Presentation.Binding.Contracts
{
    public interface IAbilitiesListViewModel
    {
        event Action onStateChanged;

        IReadOnlyList<IAbilityItemViewModel> Items { get; }

        IReadOnlyList<AbilityModificationPlacementState> GetCurrentPlacements();
    }
}
