using System;
using System.Collections.Generic;
using Feature.Abilities.Configs.ScriptableObjects;
using Feature.Abilities.Enums;
using Feature.Abilities.Domain.Models;
using UnityEngine;

namespace Feature.Abilities.Infrastructure.Factories
{
    /// <summary>
    /// Фабрика построения доменной модели способности.
    /// </summary>
    public sealed class AbilityModelFactory : IAbilityModelFactory
    {
        private const string AbilityIdFallbackPrefix = "ability_";
        private static readonly ModificationTypeFlags[] CachedTypeFlags = BuildCachedTypeFlags();

        public AbilityModel Create(AbilityConfig abilityConfig, int index)
        {
            ValidateAbilityConfig(abilityConfig);

            string abilityId = BuildAbilityId(abilityConfig, index);
            string abilityName = BuildAbilityName(abilityConfig, abilityId);
            Sprite icon = BuildIcon(abilityConfig);
            string description = BuildDescription(abilityConfig);
            AbilityType abilityType = BuildAbilityType(abilityConfig);
            List<ModificationType> supportedTypes = BuildSupportedTypes(abilityConfig);

            return new AbilityModel(
                abilityId,
                abilityName,
                icon,
                description,
                abilityType,
                supportedTypes);
        }

        private void ValidateAbilityConfig(AbilityConfig abilityConfig)
        {
            if (abilityConfig == null)
                throw new ArgumentNullException(nameof(abilityConfig));
        }

        private string BuildAbilityId(AbilityConfig abilityConfig, int index) =>
            GetRequiredId(abilityConfig.name, AbilityIdFallbackPrefix + index);

        private string BuildAbilityName(AbilityConfig abilityConfig, string abilityId)
        {
            if (!string.IsNullOrWhiteSpace(abilityConfig.AbilityName))
                return abilityConfig.AbilityName;

            return abilityId;
        }

        private Sprite BuildIcon(AbilityConfig abilityConfig) => abilityConfig.IconSprite;

        private string BuildDescription(AbilityConfig abilityConfig) => abilityConfig.Description ?? string.Empty;

        private AbilityType BuildAbilityType(AbilityConfig abilityConfig) => abilityConfig.AbilityType;

        private List<ModificationType> BuildSupportedTypes(AbilityConfig abilityConfig) =>
            MapSupportedTypes(abilityConfig.SupportedModificationTypes);

        private List<ModificationType> MapSupportedTypes(ModificationTypeFlags flags)
        {
            List<ModificationType> supportedTypes = new List<ModificationType>();
            for (int i = 0; i < CachedTypeFlags.Length; i++)
            {
                ModificationTypeFlags currentFlag = CachedTypeFlags[i];
                if ((flags & currentFlag) != currentFlag)
                    continue;

                ModificationType mappedType = MapFlagToType(currentFlag);
                supportedTypes.Add(mappedType);
            }

            return supportedTypes;
        }

        private static ModificationTypeFlags[] BuildCachedTypeFlags()
        {
            Array enumValues = Enum.GetValues(typeof(ModificationTypeFlags));
            List<ModificationTypeFlags> flags = new List<ModificationTypeFlags>();
            for (int i = 0; i < enumValues.Length; i++)
            {
                ModificationTypeFlags currentFlag = (ModificationTypeFlags)enumValues.GetValue(i);
                if (currentFlag == ModificationTypeFlags.None || currentFlag == ModificationTypeFlags.All)
                    continue;

                flags.Add(currentFlag);
            }

            return flags.ToArray();
        }

        private ModificationType MapFlagToType(ModificationTypeFlags flag)
        {
            switch (flag)
            {
                case ModificationTypeFlags.Psyker:
                    return ModificationType.Psyker;
                case ModificationTypeFlags.Dot:
                    return ModificationType.Dot;
                case ModificationTypeFlags.Attack:
                    return ModificationType.Attack;
                case ModificationTypeFlags.Buff:
                    return ModificationType.Buff;
                case ModificationTypeFlags.Debuff:
                    return ModificationType.Debuff;
                default:
                    throw new InvalidOperationException(
                        $"Неизвестный флаг {nameof(ModificationTypeFlags)}: {flag}. " +
                        $"Обновите {nameof(MapFlagToType)} в {nameof(AbilityModelFactory)}.");
            }
        }

        private string GetRequiredId(string rawId, string fallback)
        {
            if (!string.IsNullOrWhiteSpace(rawId))
                return rawId;

            return fallback;
        }
    }
}
