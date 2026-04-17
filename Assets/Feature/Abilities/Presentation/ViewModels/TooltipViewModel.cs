using System;
using Feature.Abilities.Domain.Models;
using Feature.Abilities.Presentation.Binding.Contracts;

namespace Feature.Abilities.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel единого tooltip экрана.
    /// </summary>
    public sealed class TooltipViewModel : ITooltipViewModel
    {
        private readonly TooltipContentViewModel _content;

        public TooltipViewModel()
        {
            _content = new TooltipContentViewModel();
            Content = _content;
        }

        public event Action onStateChanged;

        public bool IsVisible { get; private set; }

        public ITooltipContentViewModel Content { get; }

        public void Show(TooltipModel tooltip)
        {
            if (tooltip == null)
                throw new ArgumentNullException(nameof(tooltip));

            _content.Update(tooltip.Header, tooltip.Description);
            IsVisible = true;

            OnStateChanged();
        }

        public void Hide()
        {
            if (!IsVisible && string.IsNullOrEmpty(_content.Header))
                return;

            IsVisible = false;
            _content.Clear();

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
