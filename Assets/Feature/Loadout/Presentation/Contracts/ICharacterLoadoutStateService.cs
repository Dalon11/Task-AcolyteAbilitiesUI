using System.Collections.Generic;
using Feature.Loadout.Presentation.ViewModels;

namespace Feature.Loadout.Presentation.Contracts
{
    public interface ICharacterLoadoutStateService
    {
        public void Save(string characterId, IReadOnlyList<AbilityModificationPlacementState> placements);

        public bool TryGet(string characterId, out IReadOnlyList<AbilityModificationPlacementState> placements);
    }
}
