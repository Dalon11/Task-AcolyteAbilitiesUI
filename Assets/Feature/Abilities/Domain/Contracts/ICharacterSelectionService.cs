using System;
using Feature.Abilities.Domain.Models;

namespace Feature.Abilities.Domain.Contracts
{
    /// <summary>
    /// Контракт выбора и хранения текущего активного персонажа.
    /// </summary>
    public interface ICharacterSelectionService
    {
        /// <summary>
        /// Событие изменения выбранного персонажа.
        /// </summary>
        event Action<CharacterModel> onCharacterChanged;

        /// <summary>
        /// Пытается получить текущего выбранного персонажа.
        /// </summary>
        bool TryGetCurrentCharacter(out CharacterModel character);

        /// <summary>
        /// Выбирает персонажа по идентификатору.
        /// </summary>
        bool SelectCharacter(string characterId);
    }
}
