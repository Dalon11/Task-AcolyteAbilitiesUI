using System;
using Feature.Abilities.Configs.ScriptableObjects;
using Feature.Abilities.Enums;
using Feature.Abilities.Domain.Contracts;
using Feature.Abilities.Domain.Models;
using UnityEngine;

namespace Feature.Abilities.Infrastructure.Factories
{
    /// <summary>
    /// Фабрика построения доменной модели модификатора.
    /// </summary>
    public sealed class ModificationModelFactory : IModificationModelFactory
    {
        private const string ModificationIdFallbackPrefix = "modification_";

        private readonly IModificationTypePresentationRepository _modificationTypePresentationRepository;

        public ModificationModelFactory(IModificationTypePresentationRepository modificationTypePresentationRepository)
        {
            if (modificationTypePresentationRepository == null)
                throw new ArgumentNullException(nameof(modificationTypePresentationRepository));

            _modificationTypePresentationRepository = modificationTypePresentationRepository;
        }

        public ModificationModel Create(ModificationConfig modificationConfig, int index)
        {
            ValidateModificationConfig(modificationConfig);

            string modificationId = BuildModificationId(modificationConfig, index);
            string modificationName = BuildModificationName(modificationConfig, modificationId);
            Sprite icon = BuildIcon(modificationConfig);
            string description = BuildDescription(modificationConfig);
            ModificationType modificationType = BuildModificationType(modificationConfig);
            ModificationTypePresentationModel typePresentation = ResolveTypePresentation(modificationType);

            return new ModificationModel(
                modificationId,
                modificationName,
                icon,
                description,
                modificationType,
                typePresentation);
        }

        private void ValidateModificationConfig(ModificationConfig modificationConfig)
        {
            if (modificationConfig == null)
                throw new ArgumentNullException(nameof(modificationConfig));
        }

        private string BuildModificationId(ModificationConfig modificationConfig, int index) => 
            GetRequiredId(modificationConfig.name, ModificationIdFallbackPrefix + index);

        private string BuildModificationName(ModificationConfig modificationConfig, string modificationId)
        {
            if (!string.IsNullOrWhiteSpace(modificationConfig.ModificationName))
                return modificationConfig.ModificationName;

            return modificationId;
        }

        private Sprite BuildIcon(ModificationConfig modificationConfig) => modificationConfig.IconSprite;

        private string BuildDescription(ModificationConfig modificationConfig) => modificationConfig.Description ?? string.Empty;

        private ModificationType BuildModificationType(ModificationConfig modificationConfig) => modificationConfig.ModificationType;

        private ModificationTypePresentationModel ResolveTypePresentation(ModificationType modificationType)
        {
            ModificationTypePresentationModel typePresentation;
            if (_modificationTypePresentationRepository.TryGetByType(modificationType, out typePresentation))
                return typePresentation;

            return new ModificationTypePresentationModel(
                modificationType,
                modificationType.ToString(),
                null,
                Color.white);
        }

        private string GetRequiredId(string rawId, string fallback)
        {
            if (!string.IsNullOrWhiteSpace(rawId))
                return rawId;

            return fallback;
        }
    }
}

