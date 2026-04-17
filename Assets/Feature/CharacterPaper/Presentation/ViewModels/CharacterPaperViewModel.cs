using System;
using Feature.CharacterSelection.Core.Domain.Models;
using Feature.CharacterPaper.Presentation.Binding.Contracts;
using UnityEngine;

namespace Feature.CharacterPaper.Presentation.ViewModels
{

    /// <summary>
    /// Хранит и обновляет состояние представления Character Paper для UI.
    /// </summary>
    public sealed class CharacterPaperViewModel : ICharacterPaperViewModel
    {
        public event Action onStateChanged;

        public bool HasCharacter { get; private set; }

        public string CharacterId { get; private set; }

        public string Name { get; private set; }

        public int Hp { get; private set; }

        public int MaxHp { get; private set; }

        public int Armor { get; private set; }

        public Sprite CharacterSprite { get; private set; }

        public string Description { get; private set; }

        public void Update(CharacterModel character)
        {
            if (character == null)
                throw new ArgumentNullException(nameof(character));

            HasCharacter = true;
            CharacterId = character.Id;
            Name = character.Name;
            Hp = character.Stats.Hp;
            MaxHp = character.Stats.Hp;
            Armor = character.Stats.Armor;
            CharacterSprite = character.Portrait;
            Description = character.Description;

            OnStateChanged();
        }

        public void Clear()
        {
            HasCharacter = false;
            CharacterId = string.Empty;
            Name = string.Empty;
            Hp = 0;
            MaxHp = 0;
            Armor = 0;
            CharacterSprite = null;
            Description = string.Empty;

            OnStateChanged();
        }

        public bool TryGetTooltipContent(out string header, out string description)
        {
            if (!HasCharacter || string.IsNullOrWhiteSpace(Name) || Description == null)
            {
                header = string.Empty;
                description = string.Empty;
                return false;
            }

            header = Name;
            description = Description;
            return true;
        }

        private void OnStateChanged()
        {
            Action handler = onStateChanged;
            if (handler != null)
                handler.Invoke();
        }
    }
}
