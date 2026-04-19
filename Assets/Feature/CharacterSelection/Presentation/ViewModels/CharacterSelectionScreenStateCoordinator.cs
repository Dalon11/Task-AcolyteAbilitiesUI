using System;
using System.Collections.Generic;
using Feature.Abilities.Presentation.Binding.Contracts;
using Feature.CharacterPaper.Presentation.Binding.Contracts;
using Feature.CharacterSelection.Core.Domain.Models;
using Feature.CharacterSelection.Presentation.Contracts;
using Feature.Loadout.Presentation.Contracts.Models;
using Feature.Modifications.Presentation.Binding.Contracts;
using Feature.Party.Presentation.Binding.Contracts;
using UnityEngine;

namespace Feature.CharacterSelection.Presentation.ViewModels
{

    /// <summary>
    /// Координирует взаимодействие компонентов в сценарии Character Selection Screen State.
    /// </summary>
    public sealed class CharacterSelectionScreenStateCoordinator : ICharacterSelectionScreenStateCoordinator
    {
        private readonly IPartyViewModel _party;
        private readonly ICharacterPaperViewModel _characterPaper;
        private readonly IAbilitiesListViewModel _abilities;
        private readonly IModificationsListViewModel _modifications;

        public CharacterSelectionScreenStateCoordinator(
            IPartyViewModel party,
            ICharacterPaperViewModel characterPaper,
            IAbilitiesListViewModel abilities,
            IModificationsListViewModel modifications)
        {
            if (party == null)
                throw new ArgumentNullException(nameof(party));

            if (characterPaper == null)
                throw new ArgumentNullException(nameof(characterPaper));

            if (abilities == null)
                throw new ArgumentNullException(nameof(abilities));

            if (modifications == null)
                throw new ArgumentNullException(nameof(modifications));

            _party = party;
            _characterPaper = characterPaper;
            _abilities = abilities;
            _modifications = modifications;
        }

        public void BuildParty(IReadOnlyList<CharacterModel> characters, string selectedCharacterId)
        {
            _party.Build(characters, selectedCharacterId);
        }

        public void ApplyCharacter(CharacterModel character, IReadOnlyList<AbilityModificationPlacementState> placements)
        {
            if (character == null)
            {
                ClearScreen();
                return;
            }

            _party.SetSelectedCharacter(character.Id);
            _characterPaper.Update(character);
            _abilities.BeginStateChangeBatch();
            _modifications.BeginStateChangeBatch();

            try
            {
                _abilities.Rebuild(character.Abilities);
                _modifications.Rebuild(character.Modifications);
                _modifications.ApplyAvailabilityByAbilities(character.Abilities);
                ApplySavedPlacements(placements);
            }
            finally
            {
                _abilities.EndStateChangeBatch();
                _modifications.EndStateChangeBatch();
            }
        }

        public void ClearScreen()
        {
            _party.SetSelectedCharacter(string.Empty);
            _characterPaper.Clear();
            _abilities.Rebuild(null);
            _modifications.Rebuild(null);
        }

        private void ApplySavedPlacements(IReadOnlyList<AbilityModificationPlacementState> placements)
        {
            if (placements == null)
                return;

            for (int i = 0; i < placements.Count; i++)
            {
                AbilityModificationPlacementState placement = placements[i];
                if (placement == null)
                    continue;

                if (string.IsNullOrWhiteSpace(placement.AbilityId)
                    || string.IsNullOrWhiteSpace(placement.ModificationId))
                    continue;

                IModificationItemViewModel modificationItem;
                if (!_modifications.TryGetItemById(placement.ModificationId, out modificationItem))
                    continue;

                if (!modificationItem.IsInteractable)
                    continue;

                bool isApplied = _abilities.TryApplyModificationToAbilityFromState(
                    placement.AbilityId,
                    modificationItem.Id,
                    modificationItem.ModificationType,
                    ResolveIcon(modificationItem),
                    modificationItem.TypeColor);
                if (!isApplied)
                    continue;

                IModificationItemViewModel lockedItem;
                _modifications.TryLockById(modificationItem.Id, out lockedItem);
            }
        }

        private Sprite ResolveIcon(IModificationItemViewModel modificationItem)
        {
            if (modificationItem == null)
                return null;

            if (modificationItem.Icon != null)
                return modificationItem.Icon;

            return modificationItem.TypeIcon;
        }
    }
}
