using UnityEngine;

namespace Feature.Abilities.Presentation.Binding.Contracts
{
    public interface IModificationItemViewModel
    {
        string Id { get; }

        string Name { get; }

        Sprite Icon { get; }

        string TypeDisplayName { get; }

        Sprite TypeIcon { get; }

        Color TypeColor { get; }

        bool IsInteractable { get; }

        bool IsDimmed { get; }
    }
}
