using Feature.Abilities.Configs.ScriptableObjects;
using Feature.Abilities.Domain.Models;

namespace Feature.Abilities.Infrastructure.Factories
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
