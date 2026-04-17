using System;
using Feature.Abilities.Presentation.Binding.Contracts;

namespace Feature.Abilities.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel содержимого tooltip.
    /// </summary>
    public sealed class TooltipContentViewModel : ITooltipContentViewModel
    {
        public string Header { get; private set; }

        public string Description { get; private set; }

        public void Update(string header, string description)
        {
            if (string.IsNullOrWhiteSpace(header))
                throw new ArgumentException("Заголовок tooltip не должен быть пустым.", nameof(header));

            if (description == null)
                throw new ArgumentNullException(nameof(description));

            Header = header;
            Description = description;
        }

        public void Clear()
        {
            Header = string.Empty;
            Description = string.Empty;
        }
    }
}
