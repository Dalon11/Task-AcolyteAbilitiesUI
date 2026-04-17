using Feature.CharacterSelection.Core.Configs.ScriptableObjects;
using Feature.CharacterSelection.Core.Domain.Models;

namespace Feature.CharacterSelection.Core.Domain.Contracts
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



