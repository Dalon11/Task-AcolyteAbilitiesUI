using System;

namespace Feature.Tooltip.Presentation.Binding.Contracts
{
    public interface ITooltipViewModel
    {
        public event Action onStateChanged;

        public bool IsVisible { get; }

        public ITooltipContentViewModel Content { get; }

        public void Hide();
    }
}
