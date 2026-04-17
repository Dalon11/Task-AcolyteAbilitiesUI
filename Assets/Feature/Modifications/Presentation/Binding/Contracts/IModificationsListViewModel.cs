using System;
using System.Collections.Generic;

namespace Feature.Modifications.Presentation.Binding.Contracts
{
    public interface IModificationsListViewModel
    {
        public event Action onStateChanged;

        public IReadOnlyList<IModificationItemViewModel> Items { get; }
    }
}
