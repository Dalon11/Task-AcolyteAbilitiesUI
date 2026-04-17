using System;
using Feature.Loadout.Presentation.Binding.Contracts;
using UnityEngine;

namespace Feature.Loadout.Presentation.ViewModels
{

    /// <summary>
    /// Хранит и обновляет состояние представления Modification Drag Slot для UI.
    /// </summary>
    public sealed class ModificationDragSlotViewModel : IModificationDragSlotViewModel
    {
        private Color _color;

        public event Action onStateChanged;

        public bool IsActive { get; private set; }

        public Sprite Icon { get; private set; }

        public Color Color => _color;

        public void Show(Sprite icon, Color color)
        {
            IsActive = true;
            Icon = icon;
            _color = color;
            OnStateChanged();
        }

        public void Hide()
        {
            IsActive = false;
            Icon = null;
            _color = UnityEngine.Color.clear;
            OnStateChanged();
        }

        private void OnStateChanged()
        {
            Action handler = onStateChanged;
            if (handler != null)
                handler.Invoke();
        }
    }
}
