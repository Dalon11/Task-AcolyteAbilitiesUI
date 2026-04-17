using System;
using Feature.Abilities.Enums;
using UnityEngine;

namespace Feature.Abilities.Domain.Models
{
    /// <summary>
    /// Доменная модель модификатора, назначаемого на способность.
    /// </summary>
    public sealed class ModificationModel
    {
        public ModificationModel(
            string id,
            string name,
            Sprite icon,
            string description,
            ModificationType type,
            ModificationTypePresentationModel typePresentation)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Id модификатора не должен быть пустым.", nameof(id));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя модификатора не должно быть пустым.", nameof(name));

            if (description == null)
                throw new ArgumentNullException(nameof(description));

            if (typePresentation == null)
                throw new ArgumentNullException(nameof(typePresentation));

            Id = id;
            Name = name;
            Icon = icon;
            Description = description;
            ModificationType = type;
            TypePresentation = typePresentation;
        }

        public string Id { get; }

        public string Name { get; }

        public Sprite Icon { get; }

        public string Description { get; }

        public ModificationType ModificationType { get; }

        public ModificationTypePresentationModel TypePresentation { get; }
    }
}
