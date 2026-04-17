using Feature.Abilities.Enums;
using UnityEngine;

namespace Feature.Abilities.Presentation.ViewModels
{
    public readonly struct DraggedAbilityModificationData
    {
        public static readonly DraggedAbilityModificationData Empty =
            new DraggedAbilityModificationData(string.Empty, string.Empty, ModificationType.Unknown, null, Color.clear);

        public DraggedAbilityModificationData(
            string sourceAbilityId,
            string modificationId,
            ModificationType modificationType,
            Sprite icon,
            Color color)
        {
            SourceAbilityId = sourceAbilityId;
            ModificationId = modificationId;
            ModificationType = modificationType;
            Icon = icon;
            Color = color;
        }

        public string SourceAbilityId { get; }

        public string ModificationId { get; }

        public ModificationType ModificationType { get; }

        public Sprite Icon { get; }

        public Color Color { get; }

        public bool IsValid => !string.IsNullOrWhiteSpace(SourceAbilityId)
            && !string.IsNullOrWhiteSpace(ModificationId)
            && ModificationType != ModificationType.Unknown;
    }
}
