using System;
using UnityEngine;

namespace Feature.Abilities.Presentation.Binding.Contracts
{
    public interface ICharacterPaperViewModel
    {
        event Action onStateChanged;

        string CharacterId { get; }

        string Name { get; }

        int Hp { get; }

        int MaxHp { get; }

        int Armor { get; }

        Sprite CharacterSprite { get; }
    }
}
