using System;

namespace Feature.CharacterSelection.Core.Domain.Models
{

    /// <summary>
    /// Описывает доменную модель Character Stats.
    /// </summary>
    public sealed class CharacterStatsModel
    {
        public CharacterStatsModel(int hp, int armor)
        {
            if (hp < 0)
                throw new ArgumentOutOfRangeException(nameof(hp), "HP �� ����� ���� �������������.");

            if (armor < 0)
                throw new ArgumentOutOfRangeException(nameof(armor), "����� �� ����� ���� �������������.");

            Hp = hp;
            Armor = armor;
        }

        public int Hp { get; }

        public int Armor { get; }
    }
}
