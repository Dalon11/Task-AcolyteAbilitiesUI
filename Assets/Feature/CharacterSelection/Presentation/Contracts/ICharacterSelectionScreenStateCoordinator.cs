using System.Collections.Generic;
using Feature.CharacterSelection.Core.Domain.Models;
using Feature.Loadout.Presentation.ViewModels;

namespace Feature.CharacterSelection.Presentation.Contracts
{
    public interface ICharacterSelectionScreenStateCoordinator
    {
        void BuildParty(IReadOnlyList<CharacterModel> characters, string selectedCharacterId);

        void ApplyCharacter(CharacterModel character, IReadOnlyList<AbilityModificationPlacementState> placements);

        void ClearScreen();
    }
}
