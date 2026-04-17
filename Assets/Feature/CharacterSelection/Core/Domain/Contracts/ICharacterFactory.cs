using Feature.CharacterSelection.Core.Configs.ScriptableObjects;
using Feature.CharacterSelection.Core.Domain.Models;

namespace Feature.CharacterSelection.Core.Domain.Contracts
{

    public interface ICharacterFactory
    {

        CharacterModel CreateCharacter(CharacterConfig characterConfig);
    }
}
