using System;
using Feature.CharacterSelection.Core.Domain.Models;

namespace Feature.CharacterSelection.Core.Domain.Contracts
{

    public interface ICharacterSelectionService
    {

        event Action<CharacterModel> onCharacterChanged;

        bool TryGetCurrentCharacter(out CharacterModel character);

        bool SelectCharacter(string characterId);
    }
}
