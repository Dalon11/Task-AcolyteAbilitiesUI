using System;
using Feature.CharacterSelection.Core.Domain.Models;
using UnityEngine;

namespace Feature.CharacterPaper.Presentation.Binding.Contracts
{
    public interface ICharacterPaperViewModel
    {
        public event Action onStateChanged;

        public string CharacterId { get; }

        public string Name { get; }

        public int Hp { get; }

        public int MaxHp { get; }

        public int Armor { get; }

        public Sprite CharacterSprite { get; }

        public void Update(CharacterModel character);

        public void Clear();

        public bool TryGetTooltipContent(out string header, out string description);
    }
}
