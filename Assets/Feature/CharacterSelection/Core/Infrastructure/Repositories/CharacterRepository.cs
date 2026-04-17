using System;
using System.Collections.Generic;
using Feature.CharacterSelection.Core.Configs.ScriptableObjects;
using Feature.CharacterSelection.Core.Domain.Contracts;
using Feature.CharacterSelection.Core.Domain.Models;

namespace Feature.CharacterSelection.Core.Infrastructure.Repositories
{

    /// <summary>
    /// Предоставляет доступ к данным Character.
    /// </summary>
    public sealed class CharacterRepository : ICharacterRepository
    {
        private readonly IReadOnlyList<CharacterModel> _characters;
        private readonly Dictionary<string, CharacterModel> _charactersById;

        public CharacterRepository(CharacterCatalog characterCatalog, ICharacterFactory characterFactory)
        {
            if (characterCatalog == null)
                throw new ArgumentNullException(nameof(characterCatalog));

            if (characterFactory == null)
                throw new ArgumentNullException(nameof(characterFactory));

            List<CharacterModel> characters = new List<CharacterModel>();
            Dictionary<string, CharacterModel> charactersById = new Dictionary<string, CharacterModel>(StringComparer.Ordinal);

            IReadOnlyList<CharacterConfig> configs = characterCatalog.Characters;
            for (int i = 0; i < configs.Count; i++)
            {
                CharacterConfig config = configs[i];
                if (config == null)
                    continue;

                CharacterModel model = characterFactory.CreateCharacter(config);
                if (charactersById.ContainsKey(model.Id))
                    throw new InvalidOperationException("Обнаружены дубликаты идентификаторов персонажей в CharacterCatalog.");

                characters.Add(model);
                charactersById.Add(model.Id, model);
            }

            _characters = characters.AsReadOnly();
            _charactersById = charactersById;
        }

        public IReadOnlyList<CharacterModel> GetAllCharacters()
        {
            return _characters;
        }

        public bool TryGetCharacterById(string characterId, out CharacterModel character)
        {
            if (string.IsNullOrWhiteSpace(characterId))
            {
                character = null;
                return false;
            }

            return _charactersById.TryGetValue(characterId, out character);
        }
    }
}
