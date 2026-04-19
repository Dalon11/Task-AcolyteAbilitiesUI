using System;
using Feature.CharacterSelection.Core.Domain.Models;
using Feature.Tooltip.Presentation.Binding.Contracts;

namespace Feature.Tooltip.Presentation.ViewModels
{

    /// <summary>
    /// Хранит и обновляет состояние представления Tooltip для UI.
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
            if (!IsVisible)
                return;

            IsVisible = false;

            OnStateChanged();
        }

        public void HideAndClear()
        {
            bool isStateChanged = IsVisible || !string.IsNullOrEmpty(_content.Header) || !string.IsNullOrEmpty(_content.Description);
            if (!isStateChanged)
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
