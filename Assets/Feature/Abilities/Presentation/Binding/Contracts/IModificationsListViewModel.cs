using System;
using System.Collections.Generic;

namespace Feature.Abilities.Presentation.Binding.Contracts
{
    public interface IModificationsListViewModel
    {
        event Action onStateChanged;

        IReadOnlyList<IModificationItemViewModel> Items { get; }
    }
}
