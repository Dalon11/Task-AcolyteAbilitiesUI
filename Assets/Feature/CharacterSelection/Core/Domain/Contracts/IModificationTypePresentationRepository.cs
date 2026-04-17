using System.Collections.Generic;
using Feature.CharacterSelection.Core.Enums;
using Feature.CharacterSelection.Core.Domain.Models;

namespace Feature.CharacterSelection.Core.Domain.Contracts
{

    public interface IModificationTypePresentationRepository
    {

        IReadOnlyList<ModificationTypePresentationModel> GetAll();

        bool TryGetByType(ModificationType type, out ModificationTypePresentationModel presentation);
    }
}
