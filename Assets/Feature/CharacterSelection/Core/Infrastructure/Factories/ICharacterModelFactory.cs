using System.Collections.Generic;
using Feature.CharacterSelection.Core.Configs.ScriptableObjects;
using Feature.CharacterSelection.Core.Domain.Models;

namespace Feature.CharacterSelection.Core.Infrastructure.Factories
{
    /// <summary>
    /// Контракт фабрики доменной модели персонажа.
    /// </summary>
    public interface ICharacterModelFactory
    {
        /// <summary>
        /// Создает доменную модель персонажа из конфига и подготовленных коллекций.
        /// </summary>
        CharacterModel Create(
            CharacterConfig characterConfig,
            IReadOnlyList<AbilityModel> abilities,
            IReadOnlyList<ModificationModel> modifications);
    }
}



