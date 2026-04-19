using System;
using System.Collections.Generic;
using Feature.CharacterSelection.Core.Domain.Models;

namespace Feature.Party.Presentation.Binding.Contracts
{
    public interface IPartyViewModel
    {
        public event Action onStateChanged;

        public IReadOnlyList<IPartyCharacterItemViewModel> Items { get; }

        public void Build(IReadOnlyList<CharacterModel> characters, string selectedCharacterId);

        public void SetSelectedCharacter(string selectedCharacterId);

        public bool RequestCharacterSelection(string characterId);
    }
}
