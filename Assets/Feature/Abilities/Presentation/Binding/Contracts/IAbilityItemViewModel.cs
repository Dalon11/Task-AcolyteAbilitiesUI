using System.Collections.Generic;
using Feature.CharacterSelection.Core.Enums;
using UnityEngine;

namespace Feature.Abilities.Presentation.Binding.Contracts
{
    public interface IAbilityItemViewModel
    {
        public string Id { get; }

        public string Name { get; }

        public Sprite Icon { get; }

        public AbilityType AbilityType { get; }

        public IReadOnlyList<ModificationType> SupportedModificationTypes { get; }

        public bool IsCompatibleDropTarget { get; }

        public Color DropTargetColor { get; }

        public bool HasAppliedModification { get; }

        public Sprite AppliedModificationIcon { get; }

        public Color AppliedModificationColor { get; }
    }
}
