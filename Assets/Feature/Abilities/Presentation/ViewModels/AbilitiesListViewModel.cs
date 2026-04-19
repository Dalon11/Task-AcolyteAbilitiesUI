using System;
using System.Collections.Generic;
using Feature.CharacterSelection.Core.Domain.Models;
using Feature.CharacterSelection.Core.Enums;
using Feature.Abilities.Presentation.Binding.Contracts;
using Feature.Common.Presentation.State;
using Feature.Loadout.Presentation.Contracts.Models;
using UnityEngine;

namespace Feature.Abilities.Presentation.ViewModels
{

    /// <summary>
    /// Хранит и обновляет состояние представления Abilities List для UI.
    /// </summary>
    public sealed class AbilitiesListViewModel : IAbilitiesListViewModel
    {
        private static readonly IReadOnlyList<AbilityModificationPlacementState> EmptyPlacements =
            Array.Empty<AbilityModificationPlacementState>();

        private readonly List<AbilityItemViewModel> _items;
        private readonly Dictionary<string, AbilityItemViewModel> _itemsById;
        private readonly DeferredStateChangeNotifier _stateChangeNotifier;

        public AbilitiesListViewModel()
        {
            _items = new List<AbilityItemViewModel>();
            _itemsById = new Dictionary<string, AbilityItemViewModel>();
            _stateChangeNotifier = new DeferredStateChangeNotifier();
        }

        public event Action onStateChanged;

        public IReadOnlyList<IAbilityItemViewModel> Items => _items;

        public void BeginStateChangeBatch()
        {
            _stateChangeNotifier.BeginBatch();
        }

        public void EndStateChangeBatch()
        {
            _stateChangeNotifier.EndBatch(onStateChanged);
        }

        public void Rebuild(IReadOnlyList<AbilityModel> abilities)
        {
            _items.Clear();
            _itemsById.Clear();

            if (abilities != null)
            {
                for (int i = 0; i < abilities.Count; i++)
                {
                    AbilityModel ability = abilities[i];
                    if (ability == null)
                        continue;

                    AbilityItemViewModel item = new AbilityItemViewModel(ability);
                    _items.Add(item);

                    if (!_itemsById.ContainsKey(item.Id))
                        _itemsById.Add(item.Id, item);
                }
            }

            NotifyStateChanged();
        }

        public IReadOnlyList<AbilityModificationPlacementState> GetCurrentPlacements()
        {
            int placementsCount = 0;
            for (int i = 0; i < _items.Count; i++)
            {
                AbilityItemViewModel probeItem = _items[i];
                if (!probeItem.HasAppliedModification)
                    continue;

                if (string.IsNullOrWhiteSpace(probeItem.AppliedModificationId))
                    continue;

                placementsCount++;
            }

            if (placementsCount == 0)
                return EmptyPlacements;

            List<AbilityModificationPlacementState> placements =
                new List<AbilityModificationPlacementState>(placementsCount);

            for (int i = 0; i < _items.Count; i++)
            {
                AbilityItemViewModel item = _items[i];
                if (!item.HasAppliedModification)
                    continue;

                if (string.IsNullOrWhiteSpace(item.AppliedModificationId))
                    continue;

                placements.Add(new AbilityModificationPlacementState(item.Id, item.AppliedModificationId));
            }

            return placements;
        }

        public void ClearDragPreview()
        {
            for (int i = 0; i < _items.Count; i++)
                _items[i].SetDropTargetState(false, Color.clear);

            NotifyStateChanged();
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

            NotifyStateChanged();
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
            NotifyStateChanged();
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
            NotifyStateChanged();
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
            NotifyStateChanged();
            return true;
        }

        public bool TryGetItemById(string abilityId, out AbilityItemViewModel item)
        {
            if (string.IsNullOrWhiteSpace(abilityId))
            {
                item = null;
                return false;
            }

            return _itemsById.TryGetValue(abilityId, out item);
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

        private void NotifyStateChanged()
        {
            _stateChangeNotifier.Notify(onStateChanged);
        }
    }
}
