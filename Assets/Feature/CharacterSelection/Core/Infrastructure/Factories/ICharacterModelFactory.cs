using System.Collections.Generic;
using Feature.CharacterSelection.Core.Configs.ScriptableObjects;
using Feature.CharacterSelection.Core.Domain.Models;

namespace Feature.CharacterSelection.Core.Infrastructure.Factories
{

    public interface ICharacterModelFactory
    {

        public CharacterModel Create(
            CharacterConfig characterConfig,
            IReadOnlyList<AbilityModel> abilities,
            IReadOnlyList<ModificationModel> modifications);
    }
}
