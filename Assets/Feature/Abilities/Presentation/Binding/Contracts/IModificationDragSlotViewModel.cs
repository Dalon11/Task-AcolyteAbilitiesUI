using System;
using UnityEngine;

namespace Feature.Abilities.Presentation.Binding.Contracts
{
    public interface IModificationDragSlotViewModel
    {
        event Action onStateChanged;

        bool IsActive { get; }

        Sprite Icon { get; }

        Color Color { get; }
    }
}
