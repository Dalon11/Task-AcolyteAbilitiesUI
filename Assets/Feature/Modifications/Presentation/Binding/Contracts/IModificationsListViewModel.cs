using System;
using System.Collections.Generic;

namespace Feature.Modifications.Presentation.Binding.Contracts
{
    public interface IModificationsListViewModel
    {
        event Action onStateChanged;

        IReadOnlyList<IModificationItemViewModel> Items { get; }
    }
}



