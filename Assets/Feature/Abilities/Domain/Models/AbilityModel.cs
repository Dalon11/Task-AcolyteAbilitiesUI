using System;
using System.Collections.Generic;
using Feature.Abilities.Enums;
using UnityEngine;

namespace Feature.Abilities.Domain.Models
{
    /// <summary>
    /// Доменная модель способности персонажа.
    /// </summary>
    public sealed class AbilityModel
    {
        private readonly IReadOnlyList<ModificationType> _supportedModificationTypes;

        public AbilityModel(
            string id,
            string name,
            Sprite icon,
            string description,
            AbilityType abilityType,
            IReadOnlyList<ModificationType> supportedModificationTypes)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Id способности не должен быть пустым.", nameof(id));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя способности не должно быть пустым.", nameof(name));

            if (description == null)
                throw new ArgumentNullException(nameof(description));

            if (supportedModificationTypes == null)
                throw new ArgumentNullException(nameof(supportedModificationTypes));

            Id = id;
            Name = name;
            Icon = icon;
            Description = description;
            AbilityType = abilityType;
            _supportedModificationTypes = new List<ModificationType>(supportedModificationTypes).AsReadOnly();
        }

        public string Id { get; }

        public string Name { get; }

        public Sprite Icon { get; }

        public string Description { get; }

        public AbilityType AbilityType { get; }

        public IReadOnlyList<ModificationType> SupportedModificationTypes => _supportedModificationTypes;
    }
}
