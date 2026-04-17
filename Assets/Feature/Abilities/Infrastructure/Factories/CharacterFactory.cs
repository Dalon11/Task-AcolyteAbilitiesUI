using System;
using System.Collections.Generic;
using Feature.Abilities.Configs.ScriptableObjects;
using Feature.Abilities.Domain.Contracts;
using Feature.Abilities.Domain.Models;

namespace Feature.Abilities.Infrastructure.Factories
{
    /// <summary>
    /// Тонкая оркестрация построения CharacterModel из специализированных фабрик.
    /// </summary>
    public sealed class CharacterFactory : ICharacterFactory
    {
        private readonly IAbilityModelFactory _abilityModelFactory;
        private readonly IModificationModelFactory _modificationModelFactory;
        private readonly ICharacterModelFactory _characterModelFactory;

        public CharacterFactory(IModificationTypePresentationRepository modificationTypePresentationRepository)
            : this(
                new AbilityModelFactory(),
                new ModificationModelFactory(modificationTypePresentationRepository),
                new CharacterModelFactory())
        {
        }

        public CharacterFactory(
            IAbilityModelFactory abilityModelFactory,
            IModificationModelFactory modificationModelFactory,
            ICharacterModelFactory characterModelFactory)
        {
            if (abilityModelFactory == null)
                throw new ArgumentNullException(nameof(abilityModelFactory));

            if (modificationModelFactory == null)
                throw new ArgumentNullException(nameof(modificationModelFactory));

            if (characterModelFactory == null)
                throw new ArgumentNullException(nameof(characterModelFactory));

            _abilityModelFactory = abilityModelFactory;
            _modificationModelFactory = modificationModelFactory;
            _characterModelFactory = characterModelFactory;
        }

        public CharacterModel CreateCharacter(CharacterConfig characterConfig)
        {
            ValidateCharacterConfig(characterConfig);

            List<AbilityModel> abilities = BuildAbilities(characterConfig.Abilities);
            List<ModificationModel> modifications = BuildModifications(characterConfig.Modifications);

            return _characterModelFactory.Create(characterConfig, abilities, modifications);
        }

        private void ValidateCharacterConfig(CharacterConfig characterConfig)
        {
            if (characterConfig == null)
                throw new ArgumentNullException(nameof(characterConfig));
        }

        private List<AbilityModel> BuildAbilities(AbilityConfig[] abilityConfigs)
        {
            List<AbilityModel> abilities = new List<AbilityModel>();
            if (abilityConfigs == null)
                return abilities;

            for (int i = 0; i < abilityConfigs.Length; i++)
            {
                AbilityConfig abilityConfig = abilityConfigs[i];
                if (abilityConfig == null)
                    continue;

                AbilityModel abilityModel = CreateAbilityModel(abilityConfig, i);
                abilities.Add(abilityModel);
            }

            return abilities;
        }

        private List<ModificationModel> BuildModifications(ModificationConfig[] modificationConfigs)
        {
            List<ModificationModel> modifications = new List<ModificationModel>();
            if (modificationConfigs == null)
                return modifications;

            for (int i = 0; i < modificationConfigs.Length; i++)
            {
                ModificationConfig modificationConfig = modificationConfigs[i];
                if (modificationConfig == null)
                    continue;

                ModificationModel modificationModel = CreateModificationModel(modificationConfig, i);
                modifications.Add(modificationModel);
            }

            modifications.Sort(CompareModifications);

            return modifications;
        }

        private AbilityModel CreateAbilityModel(AbilityConfig abilityConfig, int index) =>
            _abilityModelFactory.Create(abilityConfig, index);

        private ModificationModel CreateModificationModel(ModificationConfig modificationConfig, int index) =>
            _modificationModelFactory.Create(modificationConfig, index);

        private int CompareModifications(ModificationModel left, ModificationModel right)
        {
            if (ReferenceEquals(left, right))
                return 0;

            if (left == null)
                return 1;

            if (right == null)
                return -1;

            int typeComparison = left.ModificationType.CompareTo(right.ModificationType);
            if (typeComparison != 0)
                return typeComparison;

            int nameComparison = string.Compare(left.Name, right.Name, StringComparison.Ordinal);
            if (nameComparison != 0)
                return nameComparison;

            return string.Compare(left.Id, right.Id, StringComparison.Ordinal);
        }
    }
}
