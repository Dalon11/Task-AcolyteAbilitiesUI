using UnityEngine;

namespace Feature.Abilities.Presentation.Binding.Contracts
{
    public interface IPartyCharacterItemViewModel
    {
        string Id { get; }

        string DisplayName { get; }

        Sprite Icon { get; }

        bool IsSelected { get; }
    }
}
