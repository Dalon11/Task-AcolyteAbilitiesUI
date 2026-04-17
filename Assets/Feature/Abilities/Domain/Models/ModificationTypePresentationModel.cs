using System;
using Feature.Abilities.Enums;
using UnityEngine;

namespace Feature.Abilities.Domain.Models
{
    /// <summary>
    /// Метаданные отображения типа модификатора.
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
                throw new ArgumentException("DisplayName не должен быть пустым.", nameof(displayName));

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

