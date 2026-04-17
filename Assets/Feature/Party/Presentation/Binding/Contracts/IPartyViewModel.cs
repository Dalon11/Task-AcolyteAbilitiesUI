using System;
using System.Collections.Generic;

namespace Feature.Party.Presentation.Binding.Contracts
{
    public interface IPartyViewModel
    {
        public event Action onStateChanged;

        public IReadOnlyList<IPartyCharacterItemViewModel> Items { get; }

        public bool RequestCharacterSelection(string characterId);
    }
}
