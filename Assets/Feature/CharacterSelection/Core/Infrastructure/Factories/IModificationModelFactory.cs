using Feature.CharacterSelection.Core.Configs.ScriptableObjects;
using Feature.CharacterSelection.Core.Domain.Models;

namespace Feature.CharacterSelection.Core.Infrastructure.Factories
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



