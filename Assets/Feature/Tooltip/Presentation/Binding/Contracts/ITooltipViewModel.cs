using System;
using Feature.CharacterSelection.Core.Domain.Models;

namespace Feature.Tooltip.Presentation.Binding.Contracts
{
    public interface ITooltipViewModel
    {
        public event Action onStateChanged;

        public bool IsVisible { get; }

        public ITooltipContentViewModel Content { get; }

        public void Show(TooltipModel tooltip);

        public void Hide();

        public void HideAndClear();
    }
}
