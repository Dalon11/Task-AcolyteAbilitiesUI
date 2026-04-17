using System.Collections.Generic;
using Feature.Abilities.Enums;
using Feature.Abilities.Domain.Models;

namespace Feature.Abilities.Domain.Contracts
{
    /// <summary>
    /// Контракт источника данных отображения типов модификаторов.
    /// </summary>
    public interface IModificationTypePresentationRepository
    {
        /// <summary>
        /// Возвращает все данные отображения типов модификаторов.
        /// </summary>
        IReadOnlyList<ModificationTypePresentationModel> GetAll();

        /// <summary>
        /// Пытается получить данные отображения по типу модификатора.
        /// </summary>
        bool TryGetByType(ModificationType type, out ModificationTypePresentationModel presentation);
    }
}
