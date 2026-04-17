using System;
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
    }
}
