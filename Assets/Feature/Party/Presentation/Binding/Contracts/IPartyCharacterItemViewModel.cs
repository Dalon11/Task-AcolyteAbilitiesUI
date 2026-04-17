using UnityEngine;

namespace Feature.Party.Presentation.Binding.Contracts
{
    public interface IPartyCharacterItemViewModel
    {
        string Id { get; }

        string DisplayName { get; }

        Sprite Icon { get; }

        bool IsSelected { get; }
    }
}



