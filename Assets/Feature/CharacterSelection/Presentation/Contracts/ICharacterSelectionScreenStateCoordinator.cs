using System.Collections.Generic;
using Feature.CharacterSelection.Core.Domain.Models;
using Feature.Loadout.Presentation.ViewModels;

namespace Feature.CharacterSelection.Presentation.Contracts
{
    public interface ICharacterSelectionScreenStateCoordinator
    {
        public void BuildParty(IReadOnlyList<CharacterModel> characters, string selectedCharacterId);

        public void ApplyCharacter(CharacterModel character, IReadOnlyList<AbilityModificationPlacementState> placements);

        public void ClearScreen();
    }
}
