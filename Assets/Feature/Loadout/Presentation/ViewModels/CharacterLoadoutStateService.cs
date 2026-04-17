using System;
using System.Collections.Generic;
using Feature.Loadout.Presentation.Contracts;

namespace Feature.Loadout.Presentation.ViewModels
{
    /// <summary>
    /// Предоставляет сервисную логику Character Loadout State для сценариев экрана.
    /// </summary>
    public sealed class CharacterLoadoutStateService : ICharacterLoadoutStateService
    {
        private readonly Dictionary<string, IReadOnlyList<AbilityModificationPlacementState>> _placementsByCharacterId;

        public CharacterLoadoutStateService()
        {
            _placementsByCharacterId =
                new Dictionary<string, IReadOnlyList<AbilityModificationPlacementState>>(StringComparer.Ordinal);
        }

        public void Save(string characterId, IReadOnlyList<AbilityModificationPlacementState> placements)
        {
            if (string.IsNullOrWhiteSpace(characterId))
                return;

            if (placements == null || placements.Count == 0)
            {
                _placementsByCharacterId.Remove(characterId);
                return;
            }

            _placementsByCharacterId[characterId] = new List<AbilityModificationPlacementState>(placements).AsReadOnly();
        }

        public bool TryGet(string characterId, out IReadOnlyList<AbilityModificationPlacementState> placements)
        {
            if (string.IsNullOrWhiteSpace(characterId))
            {
                placements = null;
                return false;
            }

            return _placementsByCharacterId.TryGetValue(characterId, out placements);
        }
    }
}
