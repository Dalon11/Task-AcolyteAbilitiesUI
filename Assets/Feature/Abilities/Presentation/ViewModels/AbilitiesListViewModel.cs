using System;
using System.Collections.Generic;
using Feature.Abilities.Domain.Models;
using Feature.Abilities.Presentation.Binding.Contracts;

namespace Feature.Abilities.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel списка способностей.
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
