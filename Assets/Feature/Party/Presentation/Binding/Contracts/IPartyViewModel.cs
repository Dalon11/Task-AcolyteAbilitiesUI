using System;
using System.Collections.Generic;

namespace Feature.Party.Presentation.Binding.Contracts
{
    public interface IPartyViewModel
    {
        event Action onStateChanged;

        IReadOnlyList<IPartyCharacterItemViewModel> Items { get; }

        bool RequestCharacterSelection(string characterId);
    }
}



