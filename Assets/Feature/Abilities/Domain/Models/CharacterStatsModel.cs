using System;

namespace Feature.Abilities.Domain.Models
{
    /// <summary>
    /// Базовые боевые характеристики персонажа.
    /// </summary>
    public sealed class CharacterStatsModel
    {
        public CharacterStatsModel(int hp, int armor)
        {
            if (hp < 0)
                throw new ArgumentOutOfRangeException(nameof(hp), "HP не может быть отрицательным.");

            if (armor < 0)
                throw new ArgumentOutOfRangeException(nameof(armor), "Броня не может быть отрицательной.");

            Hp = hp;
            Armor = armor;
        }

        public int Hp { get; }

        public int Armor { get; }
    }
}
