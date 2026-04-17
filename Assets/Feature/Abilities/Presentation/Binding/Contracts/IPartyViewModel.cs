using System;
using System.Collections.Generic;

namespace Feature.Abilities.Presentation.Binding.Contracts
{
    public interface IPartyViewModel
    {
        event Action onStateChanged;

        IReadOnlyList<IPartyCharacterItemViewModel> Items { get; }

        bool RequestCharacterSelection(string characterId);
    }
}
