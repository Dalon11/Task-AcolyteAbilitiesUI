using Feature.CharacterSelection.Core.Enums;
using UnityEngine;

namespace Feature.Loadout.Presentation.Contracts.Models
{
    /// <summary>
    /// Передаёт данные модификации, которую пользователь перетаскивает между списком модификаторов и ячейкой способности.
    /// </summary>
    public readonly struct DraggedAbilityModificationData
    {
        public static readonly DraggedAbilityModificationData Empty =
            new DraggedAbilityModificationData(string.Empty, ModificationType.Unknown, null, Color.clear);

        public DraggedAbilityModificationData(
            string modificationId,
            ModificationType modificationType,
            Sprite icon,
            Color color)
        {
            ModificationId = modificationId;
            ModificationType = modificationType;
            Icon = icon;
            Color = color;
        }

        public string ModificationId { get; }

        public ModificationType ModificationType { get; }

        public Sprite Icon { get; }

        public Color Color { get; }

        public bool IsValid => !string.IsNullOrWhiteSpace(ModificationId)
            && ModificationType != ModificationType.Unknown;
    }
}
