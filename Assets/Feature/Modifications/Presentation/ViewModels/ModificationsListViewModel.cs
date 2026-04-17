using System;
using System.Collections.Generic;
using Feature.CharacterSelection.Core.Domain.Models;
using Feature.CharacterSelection.Core.Enums;
using Feature.Modifications.Presentation.Binding.Contracts;

namespace Feature.Modifications.Presentation.ViewModels
{

    /// <summary>
    /// Хранит и обновляет состояние представления Modifications List для UI.
    /// </summary>
    public sealed class ModificationsListViewModel : IModificationsListViewModel
    {
        private readonly List<ModificationItemViewModel> _items;

        public ModificationsListViewModel()
        {
            _items = new List<ModificationItemViewModel>();
        }

        public event Action onStateChanged;

        public IReadOnlyList<IModificationItemViewModel> Items => _items;

        public void Rebuild(IReadOnlyList<ModificationModel> modifications)
        {
            _items.Clear();

            if (modifications != null)
            {
                for (int i = 0; i < modifications.Count; i++)
                {
                    ModificationModel modification = modifications[i];
                    if (modification == null)
                        continue;

                    _items.Add(new ModificationItemViewModel(modification));
                }
            }

            _items.Sort(CompareByTypeThenName);

            OnStateChanged();
        }

        public void ApplyAvailabilityByAbilities(IReadOnlyList<AbilityModel> abilities)
        {
            HashSet<ModificationType> supportedTypes = new HashSet<ModificationType>();

            if (abilities != null)
            {
                for (int i = 0; i < abilities.Count; i++)
                {
                    AbilityModel ability = abilities[i];
                    if (ability == null || ability.SupportedModificationTypes == null)
                        continue;

                    for (int typeIndex = 0; typeIndex < ability.SupportedModificationTypes.Count; typeIndex++)
                        supportedTypes.Add(ability.SupportedModificationTypes[typeIndex]);
                }
            }

            for (int i = 0; i < _items.Count; i++)
            {
                ModificationItemViewModel item = _items[i];
                bool isAvailable = supportedTypes.Contains(item.ModificationType);
                item.SetInteractableState(isAvailable, !isAvailable);
            }

            OnStateChanged();
        }

        public bool TryLockById(string modificationId, out ModificationItemViewModel item)
        {
            if (!TryGetItemById(modificationId, out item))
                return false;

            if (!item.IsInteractable)
                return false;

            item.SetInteractableState(false, true);
            OnStateChanged();
            return true;
        }

        public void UnlockById(string modificationId)
        {
            ModificationItemViewModel item;
            if (!TryGetItemById(modificationId, out item))
                return;

            item.SetInteractableState(true, false);
            OnStateChanged();
        }

        public bool TryGetItemById(string modificationId, out ModificationItemViewModel item)
        {
            if (string.IsNullOrWhiteSpace(modificationId))
            {
                item = null;
                return false;
            }

            for (int i = 0; i < _items.Count; i++)
            {
                ModificationItemViewModel currentItem = _items[i];
                if (!string.Equals(currentItem.Id, modificationId, StringComparison.Ordinal))
                    continue;

                item = currentItem;
                return true;
            }

            item = null;
            return false;
        }

        public bool TryGetTooltipContent(string modificationId, out string header, out string description)
        {
            ModificationItemViewModel item;
            if (!TryGetItemById(modificationId, out item))
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

        private int CompareByTypeThenName(ModificationItemViewModel left, ModificationItemViewModel right)
        {
            if (ReferenceEquals(left, right))
                return 0;

            if (left == null)
                return 1;

            if (right == null)
                return -1;

            int typeComparison = left.ModificationType.CompareTo(right.ModificationType);
            if (typeComparison != 0)
                return typeComparison;

            int nameComparison = string.Compare(left.Name, right.Name, StringComparison.OrdinalIgnoreCase);
            if (nameComparison != 0)
                return nameComparison;

            return string.Compare(left.Id, right.Id, StringComparison.Ordinal);
        }
    }
}
