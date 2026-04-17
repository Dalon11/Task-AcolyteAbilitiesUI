using Feature.CharacterSelection.Core.Configs.ScriptableObjects;
using Feature.CharacterSelection.Core.Domain.Models;

namespace Feature.CharacterSelection.Core.Infrastructure.Factories
{
    /// <summary>
    /// Контракт фабрики доменной модели способности.
    /// </summary>
    public interface IAbilityModelFactory
    {
        /// <summary>
        /// Создает доменную модель способности из конфига.
        /// </summary>
        AbilityModel Create(AbilityConfig abilityConfig, int index);
    }
}



