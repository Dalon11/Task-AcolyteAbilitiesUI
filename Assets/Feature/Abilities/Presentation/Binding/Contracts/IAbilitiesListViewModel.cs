using System;
using System.Collections.Generic;
using Feature.CharacterSelection.Core.Domain.Models;
using Feature.CharacterSelection.Core.Enums;
using Feature.Loadout.Presentation.Contracts.Models;
using UnityEngine;

namespace Feature.Abilities.Presentation.Binding.Contracts
{
    public interface IAbilitiesListViewModel
    {
        public event Action onStateChanged;

        public IReadOnlyList<IAbilityItemViewModel> Items { get; }

        public void BeginStateChangeBatch();

        public void EndStateChangeBatch();

        public void Rebuild(IReadOnlyList<AbilityModel> abilities);

        public IReadOnlyList<AbilityModificationPlacementState> GetCurrentPlacements();

        public void ClearDragPreview();

        public void SetDragPreview(ModificationType modificationType, Color color);

        public bool TryApplyModificationToAbility(
            string abilityId,
            string modificationId,
            ModificationType modificationType,
            Sprite modificationIcon,
            Color modificationColor);

        public bool TryApplyModificationToAbilityFromState(
            string abilityId,
            string modificationId,
            ModificationType modificationType,
            Sprite modificationIcon,
            Color modificationColor);

        public bool TryTakeAppliedModificationFromAbility(string abilityId, out DraggedAbilityModificationData draggedData);

        public bool TryGetTooltipContent(string abilityId, out string header, out string description);
    }
}
