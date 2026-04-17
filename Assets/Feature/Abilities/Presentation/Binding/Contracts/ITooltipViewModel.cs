using System;

namespace Feature.Abilities.Presentation.Binding.Contracts
{
    public interface ITooltipViewModel
    {
        event Action onStateChanged;

        bool IsVisible { get; }

        ITooltipContentViewModel Content { get; }

        void Hide();
    }
}
