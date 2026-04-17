using Feature.Abilities.Configs.ScriptableObjects;
using Feature.Abilities.Domain.Models;

namespace Feature.Abilities.Domain.Contracts
{
    /// <summary>
    /// Контракт преобразования конфигов персонажа в доменные модели.
    /// </summary>
    public interface ICharacterFactory
    {
        /// <summary>
        /// Создает доменную модель персонажа из ScriptableObject-конфига.
        /// </summary>
        CharacterModel CreateCharacter(CharacterConfig characterConfig);
    }
}
