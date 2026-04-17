using System.Collections.Generic;
using Feature.CharacterSelection.Core.Enums;
using Feature.CharacterSelection.Core.Domain.Models;

namespace Feature.CharacterSelection.Core.Domain.Contracts
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



