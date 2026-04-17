using System.Collections.Generic;
using Feature.Loadout.Presentation.ViewModels;

namespace Feature.Loadout.Presentation.Contracts
{
    public interface ICharacterLoadoutStateService
    {
        void Save(string characterId, IReadOnlyList<AbilityModificationPlacementState> placements);

        bool TryGet(string characterId, out IReadOnlyList<AbilityModificationPlacementState> placements);
    }
}
