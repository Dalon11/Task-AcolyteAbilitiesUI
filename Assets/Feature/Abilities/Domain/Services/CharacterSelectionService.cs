using System;
using Feature.Abilities.Domain.Contracts;
using Feature.Abilities.Domain.Models;

namespace Feature.Abilities.Domain.Services
{
    /// <summary>
    /// Сервис выбора и хранения текущего активного персонажа.
    /// </summary>
    public sealed class CharacterSelectionService : ICharacterSelectionService
    {
        private readonly ICharacterRepository _characterRepository;
        private CharacterModel _currentCharacter;

        public CharacterSelectionService(ICharacterRepository characterRepository)
        {
            if (characterRepository == null)
                throw new ArgumentNullException(nameof(characterRepository));

            _characterRepository = characterRepository;
        }

        public event Action<CharacterModel> onCharacterChanged;

        public bool TryGetCurrentCharacter(out CharacterModel character)
        {
            if (_currentCharacter == null)
            {
                character = null;
                return false;
            }

            character = _currentCharacter;
            return true;
        }

        public bool SelectCharacter(string characterId)
        {
            CharacterModel nextCharacter;
            if (!_characterRepository.TryGetCharacterById(characterId, out nextCharacter))
                return false;

            if (_currentCharacter != null && string.Equals(_currentCharacter.Id, nextCharacter.Id, StringComparison.Ordinal))
                return true;

            _currentCharacter = nextCharacter;
            Action<CharacterModel> handler = onCharacterChanged;
            if (handler != null)
                handler.Invoke(_currentCharacter);

            return true;
        }
    }
}
