using System.Collections.Generic;
using Feature.CharacterSelection.Core.Enums;
using UnityEngine;

namespace Feature.Abilities.Presentation.Binding.Contracts
{
    public interface IAbilityItemViewModel
    {
        string Id { get; }

        string Name { get; }

        Sprite Icon { get; }

        AbilityType AbilityType { get; }

        IReadOnlyList<ModificationType> SupportedModificationTypes { get; }

        bool IsCompatibleDropTarget { get; }

        Color DropTargetColor { get; }

        bool HasAppliedModification { get; }

        Sprite AppliedModificationIcon { get; }

        Color AppliedModificationColor { get; }
    }
}
