using System.Collections.Generic;
using Feature.Abilities.Domain.Models;

namespace Feature.Abilities.Domain.Contracts
{
    /// <summary>
    /// Контракт источника данных персонажей.
    /// </summary>
    public interface ICharacterRepository
    {
        /// <summary>
        /// Возвращает неизменяемый список доступных персонажей.
        /// </summary>
        IReadOnlyList<CharacterModel> GetAllCharacters();

        /// <summary>
        /// Пытается получить персонажа по идентификатору.
        /// </summary>
        bool TryGetCharacterById(string characterId, out CharacterModel character);
    }
}
