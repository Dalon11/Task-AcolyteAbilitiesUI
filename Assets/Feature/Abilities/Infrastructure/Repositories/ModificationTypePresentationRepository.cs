using System;
using System.Collections.Generic;
using Feature.Abilities.Configs.ScriptableObjects;
using Feature.Abilities.Enums;
using Feature.Abilities.Domain.Contracts;
using Feature.Abilities.Domain.Models;
using UnityEngine;

namespace Feature.Abilities.Infrastructure.Repositories
{
    /// <summary>
    /// Репозиторий данных отображения типа модификатора из ModificationTypeCatalog.
    /// </summary>
    public sealed class ModificationTypePresentationRepository : IModificationTypePresentationRepository
    {
        private readonly IReadOnlyList<ModificationTypePresentationModel> _presentations;
        private readonly Dictionary<ModificationType, ModificationTypePresentationModel> _presentationsByType;

        public ModificationTypePresentationRepository(ModificationTypeCatalog catalog)
        {
            if (catalog == null)
                throw new ArgumentNullException(nameof(catalog));

            List<ModificationTypePresentationModel> presentations = new List<ModificationTypePresentationModel>();
            Dictionary<ModificationType, ModificationTypePresentationModel> presentationsByType =
                new Dictionary<ModificationType, ModificationTypePresentationModel>();

            IReadOnlyList<ModificationTypeCatalog.ModificationTypeEntry> entries = catalog.Entries;
            for (int i = 0; i < entries.Count; i++)
            {
                ModificationTypeCatalog.ModificationTypeEntry entry = entries[i];
                if (entry == null)
                    continue;

                ModificationType domainType = entry.Type;
                string displayName = string.IsNullOrWhiteSpace(entry.DisplayName)
                    ? domainType.ToString()
                    : entry.DisplayName;
                Sprite icon = entry.IconSprite;
                Color slotColor = entry.SlotColor;

                ModificationTypePresentationModel presentation =
                    new ModificationTypePresentationModel(domainType, displayName, icon, slotColor);

                if (!presentationsByType.ContainsKey(domainType))
                {
                    presentationsByType.Add(domainType, presentation);
                    presentations.Add(presentation);
                }
            }

            _presentations = presentations.AsReadOnly();
            _presentationsByType = presentationsByType;
        }

        public IReadOnlyList<ModificationTypePresentationModel> GetAll()
        {
            return _presentations;
        }

        public bool TryGetByType(ModificationType type, out ModificationTypePresentationModel presentation)
        {
            return _presentationsByType.TryGetValue(type, out presentation);
        }
    }
}
