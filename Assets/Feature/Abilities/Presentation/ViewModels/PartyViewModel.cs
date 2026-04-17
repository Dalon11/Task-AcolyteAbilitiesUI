using System;
using System.Collections.Generic;
using Feature.Abilities.Domain.Models;
using Feature.Abilities.Presentation.Binding.Contracts;

namespace Feature.Abilities.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel списка Party, которая строит элементы, хранит выбор и запрашивает смену персонажа.
    /// </summary>
    public sealed class PartyViewModel : IPartyViewModel
    {
        private readonly List<PartyCharacterItemViewModel> _items;

        public PartyViewModel()
        {
            _items = new List<PartyCharacterItemViewModel>();
        }

        public event Action onStateChanged;
        public event Action<string> onCharacterSelectionRequested;

        public IReadOnlyList<IPartyCharacterItemViewModel> Items => _items;

        public void Build(IReadOnlyList<CharacterModel> characters, string selectedCharacterId)
        {
            _items.Clear();

            if (characters != null)
            {
                for (int i = 0; i < characters.Count; i++)
                {
                    CharacterModel character = characters[i];
                    if (character == null)
                        continue;

                    PartyCharacterItemViewModel item = new PartyCharacterItemViewModel(character.Id, character.Name, character.PartyIcon);
                    item.SetSelected(string.Equals(character.Id, selectedCharacterId, StringComparison.Ordinal));
                    _items.Add(item);
                }
            }

            OnStateChanged();
        }

        public void SetSelectedCharacter(string selectedCharacterId)
        {
            bool hasChanges = false;

            for (int i = 0; i < _items.Count; i++)
            {
                PartyCharacterItemViewModel item = _items[i];
                bool shouldBeSelected = string.Equals(item.Id, selectedCharacterId, StringComparison.Ordinal);
                if (item.IsSelected == shouldBeSelected)
                    continue;

                item.SetSelected(shouldBeSelected);
                hasChanges = true;
            }

            if (hasChanges)
                OnStateChanged();
        }

        public bool RequestCharacterSelection(string characterId)
        {
            if (string.IsNullOrWhiteSpace(characterId))
                return false;

            for (int i = 0; i < _items.Count; i++)
            {
                PartyCharacterItemViewModel item = _items[i];
                if (!string.Equals(item.Id, characterId, StringComparison.Ordinal))
                    continue;

                Action<string> handler = onCharacterSelectionRequested;
                if (handler != null)
                    handler.Invoke(characterId);

                return true;
            }

            return false;
        }

        private void OnStateChanged()
        {
            Action handler = onStateChanged;
            if (handler != null)
                handler.Invoke();
        }
    }
}
