using System;
using System.Collections.Generic;
using Feature.Abilities.Domain.Models;

namespace Feature.Abilities.Presentation.ViewModels
{
    /// <summary>
    /// Координирует обновление блоков экрана при смене активного персонажа.
    /// </summary>
    public sealed class CharacterSelectionScreenStateCoordinator
    {
        private readonly PartyViewModel _party;
        private readonly CharacterPaperViewModel _characterPaper;
        private readonly AbilitiesListViewModel _abilities;
        private readonly ModificationsListViewModel _modifications;

        public CharacterSelectionScreenStateCoordinator(
            PartyViewModel party,
            CharacterPaperViewModel characterPaper,
            AbilitiesListViewModel abilities,
            ModificationsListViewModel modifications)
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

        public void ApplyCharacter(CharacterModel character)
        {
            if (character == null)
            {
                ClearScreen();
                return;
            }

            _party.SetSelectedCharacter(character.Id);
            _characterPaper.Update(character);
            _abilities.Rebuild(character.Abilities);
            _modifications.Rebuild(character.Modifications);
        }

        public void ClearScreen()
        {
            _party.SetSelectedCharacter(string.Empty);
            _characterPaper.Clear();
            _abilities.Rebuild(null);
            _modifications.Rebuild(null);
        }
    }
}
