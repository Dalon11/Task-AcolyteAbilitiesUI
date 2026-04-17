using System;
using System.Collections.Generic;
using Feature.Abilities.Domain.Models;
using Feature.Abilities.Presentation.Binding.Contracts;

namespace Feature.Abilities.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel списка модификаторов.
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
    }
}
