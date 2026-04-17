using System.Collections.Generic;
using Feature.CharacterSelection.Core.Domain.Models;

namespace Feature.CharacterSelection.Core.Domain.Contracts
{

    public interface ICharacterRepository
    {

        public IReadOnlyList<CharacterModel> GetAllCharacters();

        public bool TryGetCharacterById(string characterId, out CharacterModel character);
    }
}
