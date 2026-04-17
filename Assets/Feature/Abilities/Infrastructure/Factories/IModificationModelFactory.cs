using Feature.Abilities.Configs.ScriptableObjects;
using Feature.Abilities.Domain.Models;

namespace Feature.Abilities.Infrastructure.Factories
{
    /// <summary>
    /// Контракт фабрики доменной модели модификатора.
    /// </summary>
    public interface IModificationModelFactory
    {
        /// <summary>
        /// Создает доменную модель модификатора из конфига.
        /// </summary>
        ModificationModel Create(ModificationConfig modificationConfig, int index);
    }
}
