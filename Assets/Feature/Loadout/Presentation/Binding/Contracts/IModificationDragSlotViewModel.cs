using System;
using UnityEngine;

namespace Feature.Loadout.Presentation.Binding.Contracts
{
    public interface IModificationDragSlotViewModel
    {
        public event Action onStateChanged;

        public bool IsActive { get; }

        public Sprite Icon { get; }

        public Color Color { get; }
    }
}
