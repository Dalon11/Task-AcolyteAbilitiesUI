using System;
using System.Collections.Generic;
using Feature.CharacterSelection.Core.Domain.Models;
using Feature.CharacterSelection.Core.Enums;
using Feature.Abilities.Presentation.Binding.Contracts;
using Feature.Loadout.Presentation.ViewModels;
using UnityEngine;

namespace Feature.Abilities.Presentation.ViewModels
{

    /// <summary>
    /// Хранит и обновляет состояние представления Abilities List для UI.
    /// </summary>
    public sealed class AbilitiesListViewModel : IAbilitiesListViewModel
    {
        private readonly List<AbilityItemViewModel> _items;

        public AbilitiesListViewModel()
        {
            _items = new List<AbilityItemViewModel>();
        }

        public event Action onStateChanged;

        public IReadOnlyList<IAbilityItemViewModel> Items => _items;

        public void Rebuild(IReadOnlyList<AbilityModel> abilities)
        {
            _items.Clear();

            if (abilities != null)
            {
                for (int i = 0; i < abilities.Count; i++)
                {
                    AbilityModel ability = abilities[i];
                    if (ability == null)
                        continue;

                    _items.Add(new AbilityItemViewModel(ability));
                }
            }

            OnStateChanged();
        }

        public IReadOnlyList<AbilityModificationPlacementState> GetCurrentPlacements()
        {
            List<AbilityModificationPlacementState> placements = new List<AbilityModificationPlacementState>();

            for (int i = 0; i < _items.Count; i++)
            {
                AbilityItemViewModel item = _items[i];
                if (!item.HasAppliedModification)
                    continue;

                if (string.IsNullOrWhiteSpace(item.AppliedModificationId))
                    continue;

                placements.Add(new AbilityModificationPlacementState(item.Id, item.AppliedModificationId));
            }

            return placements.AsReadOnly();
        }

        public void ClearDragPreview()
        {
            for (int i = 0; i < _items.Count; i++)
                _items[i].SetDropTargetState(false, Color.clear);

            OnStateChanged();
        }

        public void SetDragPreview(ModificationType modificationType, Color color)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                AbilityItemViewModel item = _items[i];
                bool isCompatible = item.SupportsModificationType(modificationType)
                    && !item.HasAppliedModification;
                item.SetDropTargetState(isCompatible, color);
            }

            OnStateChanged();
        }

        public bool TryApplyModificationToAbility(
            string abilityId,
            string modificationId,
            ModificationType modificationType,
            Sprite modificationIcon,
            Color modificationColor)
        {
            AbilityItemViewModel item;
            if (!TryGetItemById(abilityId, out item))
                return false;

            if (!item.IsCompatibleDropTarget)
                return false;

            if (item.HasAppliedModification)
                return false;

            item.ApplyModification(modificationId, modificationType, modificationIcon, modificationColor);
            OnStateChanged();
            return true;
        }

        public bool TryApplyModificationToAbilityFromState(
            string abilityId,
            string modificationId,
            ModificationType modificationType,
            Sprite modificationIcon,
            Color modificationColor)
        {
            AbilityItemViewModel item;
            if (!TryGetItemById(abilityId, out item))
                return false;

            if (item.HasAppliedModification)
                return false;

            if (!item.SupportsModificationType(modificationType))
                return false;

            item.ApplyModification(modificationId, modificationType, modificationIcon, modificationColor);
            OnStateChanged();
            return true;
        }

        public bool TryTakeAppliedModificationFromAbility(string abilityId, out DraggedAbilityModificationData draggedData)
        {
            AbilityItemViewModel item;
            if (!TryGetItemById(abilityId, out item))
            {
                draggedData = DraggedAbilityModificationData.Empty;
                return false;
            }

            string modificationId;
            ModificationType modificationType;
            Sprite icon;
            Color color;
            bool isTaken = item.TryTakeAppliedModification(out modificationId, out modificationType, out icon, out color);
            if (!isTaken)
            {
                draggedData = DraggedAbilityModificationData.Empty;
                return false;
            }

            draggedData = new DraggedAbilityModificationData(
                modificationId,
                modificationType,
                icon,
                color);
            OnStateChanged();
            return true;
        }

        public bool TryGetItemById(string abilityId, out AbilityItemViewModel item)
        {
            if (string.IsNullOrWhiteSpace(abilityId))
            {
                item = null;
                return false;
            }

            for (int i = 0; i < _items.Count; i++)
            {
                AbilityItemViewModel currentItem = _items[i];
                if (!string.Equals(currentItem.Id, abilityId, StringComparison.Ordinal))
                    continue;

                item = currentItem;
                return true;
            }

            item = null;
            return false;
        }

        public bool TryGetTooltipContent(string abilityId, out string header, out string description)
        {
            AbilityItemViewModel item;
            if (!TryGetItemById(abilityId, out item))
            {
                header = string.Empty;
                description = string.Empty;
                return false;
            }

            return item.TryGetTooltipContent(out header, out description);
        }

        private void OnStateChanged()
        {
            Action handler = onStateChanged;
            if (handler != null)
                handler.Invoke();
        }
    }
}
