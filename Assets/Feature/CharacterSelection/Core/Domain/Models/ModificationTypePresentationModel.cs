using System;
using Feature.CharacterSelection.Core.Enums;
using UnityEngine;

namespace Feature.CharacterSelection.Core.Domain.Models
{

    /// <summary>
    /// Описывает доменную модель Modification Type Presentation.
    /// </summary>
    public sealed class ModificationTypePresentationModel
    {
        public ModificationTypePresentationModel(
            ModificationType type,
            string displayName,
            Sprite icon,
            Color color)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                throw new ArgumentException("DisplayName �� ������ ���� ������.", nameof(displayName));

            Type = type;
            DisplayName = displayName;
            Icon = icon;
            Color = color;
        }

        public ModificationType Type { get; }

        public string DisplayName { get; }

        public Sprite Icon { get; }

        public Color Color { get; }
    }
}
