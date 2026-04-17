using Feature.CharacterSelection.Core.Configs.ScriptableObjects;
using Feature.CharacterSelection.Core.Domain.Models;

namespace Feature.CharacterSelection.Core.Infrastructure.Factories
{

    public interface IModificationModelFactory
    {

        public ModificationModel Create(ModificationConfig modificationConfig, int index);
    }
}
