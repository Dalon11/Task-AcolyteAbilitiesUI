using System;
using System.Collections.Generic;
using Feature.CharacterSelection.Core.Domain.Models;
using Feature.CharacterSelection.Core.Enums;
using Feature.Common.Presentation.State;
using Feature.Modifications.Presentation.Binding.Contracts;

namespace Feature.Modifications.Presentation.ViewModels
{

    /// <summary>
    /// Хранит и обновляет состояние представления Modifications List для UI.
    /// </summary>
    public sealed class ModificationsListViewModel : IModificationsListViewModel
    {
        private readonly List<ModificationItemViewModel> _items;
        private readonly Dictionary<string, ModificationItemViewModel> _itemsById;
        private readonly HashSet<ModificationType> _supportedTypesBuffer;
        private readonly DeferredStateChangeNotifier _stateChangeNotifier;

        public ModificationsListViewModel()
        {
            _items = new List<ModificationItemViewModel>();
            _itemsById = new Dictionary<string, ModificationItemViewModel>();
            _supportedTypesBuffer = new HashSet<ModificationType>();
            _stateChangeNotifier = new DeferredStateChangeNotifier();
        }

        public event Action onStateChanged;

        public IReadOnlyList<IModificationItemViewModel> Items => _items;

        public void BeginStateChangeBatch()
        {
            _stateChangeNotifier.BeginBatch();
        }

        public void EndStateChangeBatch()
        {
            _stateChangeNotifier.EndBatch(onStateChanged);
        }

        public void Rebuild(IReadOnlyList<ModificationModel> modifications)
        {
            _items.Clear();
            _itemsById.Clear();

            if (modifications != null)
            {
                for (int i = 0; i < modifications.Count; i++)
                {
                    ModificationModel modification = modifications[i];
                    if (modification == null)
                        continue;

                    ModificationItemViewModel item = new ModificationItemViewModel(modification);
                    _items.Add(item);

                    if (!_itemsById.ContainsKey(item.Id))
                        _itemsById.Add(item.Id, item);
                }
            }

            _items.Sort(CompareByTypeThenName);

            NotifyStateChanged();
        }

        public void ApplyAvailabilityByAbilities(IReadOnlyList<AbilityModel> abilities)
        {
            _supportedTypesBuffer.Clear();

            if (abilities != null)
            {
                for (int i = 0; i < abilities.Count; i++)
                {
                    AbilityModel ability = abilities[i];
                    if (ability == null || ability.SupportedModificationTypes == null)
                        continue;

                    for (int typeIndex = 0; typeIndex < ability.SupportedModificationTypes.Count; typeIndex++)
                        _supportedTypesBuffer.Add(ability.SupportedModificationTypes[typeIndex]);
                }
            }

            for (int i = 0; i < _items.Count; i++)
            {
                ModificationItemViewModel item = _items[i];
                bool isAvailable = _supportedTypesBuffer.Contains(item.ModificationType);
                item.SetInteractableState(isAvailable, !isAvailable);
            }

            NotifyStateChanged();
        }

        public bool TryLockById(string modificationId, out IModificationItemViewModel item)
        {
            ModificationItemViewModel concreteItem;
            if (!TryGetConcreteItemById(modificationId, out concreteItem))
            {
                item = null;
                return false;
            }

            if (!concreteItem.IsInteractable)
            {
                item = concreteItem;
                return false;
            }

            concreteItem.SetInteractableState(false, true);
            item = concreteItem;
            NotifyStateChanged();
            return true;
        }

        public void UnlockById(string modificationId)
        {
            ModificationItemViewModel concreteItem;
            if (!TryGetConcreteItemById(modificationId, out concreteItem))
                return;

            concreteItem.SetInteractableState(true, false);
            NotifyStateChanged();
        }

        public bool TryGetItemById(string modificationId, out IModificationItemViewModel item)
        {
            ModificationItemViewModel concreteItem;
            bool hasItem = TryGetConcreteItemById(modificationId, out concreteItem);
            item = concreteItem;
            return hasItem;
        }

        public bool TryGetTooltipContent(string modificationId, out string header, out string description)
        {
            ModificationItemViewModel concreteItem;
            if (!TryGetConcreteItemById(modificationId, out concreteItem))
            {
                header = string.Empty;
                description = string.Empty;
                return false;
            }

            return concreteItem.TryGetTooltipContent(out header, out description);
        }

        private bool TryGetConcreteItemById(string modificationId, out ModificationItemViewModel item)
        {
            if (string.IsNullOrWhiteSpace(modificationId))
            {
                item = null;
                return false;
            }

            return _itemsById.TryGetValue(modificationId, out item);
        }

        private void NotifyStateChanged()
        {
            _stateChangeNotifier.Notify(onStateChanged);
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
