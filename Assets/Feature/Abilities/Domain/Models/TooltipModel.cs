using System;

namespace Feature.Abilities.Domain.Models
{
    /// <summary>
    /// Доменная модель содержимого tooltip с заголовком и описанием.
    /// </summary>
    public sealed class TooltipModel
    {
        public TooltipModel(
            string header,
            string description)
        {
            if (string.IsNullOrWhiteSpace(header))
                throw new ArgumentException("Header не должен быть пустым.", nameof(header));

            if (description == null)
                throw new ArgumentNullException(nameof(description));

            Header = header;
            Description = description;
        }

        public string Header { get; }

        public string Description { get; }
    }
}
