using System.Collections.Generic;
using Feature.CharacterSelection.Core.Domain.Models;

namespace Feature.CharacterSelection.Core.Domain.Contracts
{

    public interface ICharacterRepository
    {

        IReadOnlyList<CharacterModel> GetAllCharacters();

        bool TryGetCharacterById(string characterId, out CharacterModel character);
    }
}
