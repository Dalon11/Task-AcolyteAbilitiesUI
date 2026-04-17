using System;
using Feature.Abilities.Presentation.Binding.Contracts;
using UnityEngine;

namespace Feature.Abilities.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel элемента персонажа в блоке Party.
    /// </summary>
    public sealed class PartyCharacterItemViewModel : IPartyCharacterItemViewModel
    {
        public PartyCharacterItemViewModel(string id, string displayName, Sprite icon)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Id персонажа не должен быть пустым.", nameof(id));

            if (string.IsNullOrWhiteSpace(displayName))
                throw new ArgumentException("DisplayName персонажа не должен быть пустым.", nameof(displayName));

            Id = id;
            DisplayName = displayName;
            Icon = icon;
        }

        public string Id { get; }

        public string DisplayName { get; }

        public Sprite Icon { get; }

        public bool IsSelected { get; private set; }

        public void SetSelected(bool isSelected)
        {
            IsSelected = isSelected;
        }
    }
}
