using System;
using System.Collections.Generic;
using Feature.CharacterSelection.Core.Configs.ScriptableObjects;
using Feature.CharacterSelection.Core.Domain.Models;
using UnityEngine;

namespace Feature.CharacterSelection.Core.Infrastructure.Factories
{

    /// <summary>
    /// Создает экземпляры Character Model по входным данным.
    /// </summary>
    public sealed class CharacterModelFactory : ICharacterModelFactory
    {
        private const string CharacterIdFallback = "character";

        public CharacterModel Create(
            CharacterConfig characterConfig,
            IReadOnlyList<AbilityModel> abilities,
            IReadOnlyList<ModificationModel> modifications)
        {
            ValidateInput(characterConfig, abilities, modifications);

            string characterId = BuildCharacterId(characterConfig);
            string characterName = BuildCharacterName(characterConfig, characterId);
            Sprite portrait = BuildPortrait(characterConfig);
            Sprite partyIcon = BuildPartyIcon(characterConfig);
            string description = BuildDescription(characterConfig);
            CharacterStatsModel statsModel = BuildStats(characterConfig);

            return new CharacterModel(
                characterId,
                characterName,
                portrait,
                partyIcon,
                description,
                statsModel,
                abilities,
                modifications);
        }

        private void ValidateInput(
            CharacterConfig characterConfig,
            IReadOnlyList<AbilityModel> abilities,
            IReadOnlyList<ModificationModel> modifications)
        {
            if (characterConfig == null)
                throw new ArgumentNullException(nameof(characterConfig));

            if (abilities == null)
                throw new ArgumentNullException(nameof(abilities));

            if (modifications == null)
                throw new ArgumentNullException(nameof(modifications));
        }

        private string BuildCharacterId(CharacterConfig characterConfig) => GetRequiredId(characterConfig.name, CharacterIdFallback);

        private string BuildCharacterName(CharacterConfig characterConfig, string characterId)
        {
            if (!string.IsNullOrWhiteSpace(characterConfig.CharacterName))
                return characterConfig.CharacterName;

            return characterId;
        }

        private Sprite BuildPortrait(CharacterConfig characterConfig) => characterConfig.CharacterSprite;

        private Sprite BuildPartyIcon(CharacterConfig characterConfig) => characterConfig.IconSprite;

        private string BuildDescription(CharacterConfig characterConfig) => characterConfig.Description ?? string.Empty;

        private CharacterStatsModel BuildStats(CharacterConfig characterConfig) =>
            new CharacterStatsModel(characterConfig.Hp, characterConfig.Armor);

        private string GetRequiredId(string rawId, string fallback)
        {
            if (!string.IsNullOrWhiteSpace(rawId))
                return rawId;

            return fallback;
        }
    }
}
