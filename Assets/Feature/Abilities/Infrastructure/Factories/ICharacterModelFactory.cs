using System.Collections.Generic;
using Feature.Abilities.Configs.ScriptableObjects;
using Feature.Abilities.Domain.Models;

namespace Feature.Abilities.Infrastructure.Factories
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
