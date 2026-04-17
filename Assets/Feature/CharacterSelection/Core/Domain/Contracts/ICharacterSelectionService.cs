using System;
using Feature.CharacterSelection.Core.Domain.Models;

namespace Feature.CharacterSelection.Core.Domain.Contracts
{

    public interface ICharacterSelectionService
    {

        public event Action<CharacterModel> onCharacterChanged;

        public bool TryGetCurrentCharacter(out CharacterModel character);

        public bool SelectCharacter(string characterId);
    }
}
