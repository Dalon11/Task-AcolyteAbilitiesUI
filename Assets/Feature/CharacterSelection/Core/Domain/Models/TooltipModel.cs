using System;

namespace Feature.CharacterSelection.Core.Domain.Models
{
    /// <summary>
    /// �������� ������ ����������� tooltip � ���������� � ���������.
    /// </summary>
    public sealed class TooltipModel
    {
        public TooltipModel(
            string header,
            string description)
        {
            if (string.IsNullOrWhiteSpace(header))
                throw new ArgumentException("Header �� ������ ���� ������.", nameof(header));

            if (description == null)
                throw new ArgumentNullException(nameof(description));

            Header = header;
            Description = description;
        }

        public string Header { get; }

        public string Description { get; }
    }
}



