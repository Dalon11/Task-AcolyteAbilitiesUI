using System;
using System.Collections.Generic;

namespace Feature.Abilities.Presentation.Binding.Contracts
{
    public interface IAbilitiesListViewModel
    {
        event Action onStateChanged;

        IReadOnlyList<IAbilityItemViewModel> Items { get; }
    }
}


