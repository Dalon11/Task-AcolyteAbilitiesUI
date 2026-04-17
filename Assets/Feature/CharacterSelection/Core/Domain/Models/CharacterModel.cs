using System;
using System.Collections.Generic;
using UnityEngine;

namespace Feature.CharacterSelection.Core.Domain.Models
{

    /// <summary>
    /// Описывает доменную модель Character.
    /// </summary>
    public sealed class CharacterModel
    {
        private readonly IReadOnlyList<AbilityModel> _abilities;
        private readonly IReadOnlyList<ModificationModel> _modifications;

        public CharacterModel(
            string id,
            string name,
            Sprite portrait,
            Sprite partyIcon,
            string description,
            CharacterStatsModel stats,
            IReadOnlyList<AbilityModel> abilities,
            IReadOnlyList<ModificationModel> modifications)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Id персонажа не может быть пустым.", nameof(id));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя персонажа не может быть пустым.", nameof(name));

            if (description == null)
                throw new ArgumentNullException(nameof(description));

            if (stats == null)
                throw new ArgumentNullException(nameof(stats));

            if (abilities == null)
                throw new ArgumentNullException(nameof(abilities));

            if (modifications == null)
                throw new ArgumentNullException(nameof(modifications));

            Id = id;
            Name = name;
            Portrait = portrait;
            PartyIcon = partyIcon;
            Description = description;
            Stats = stats;
            _abilities = new List<AbilityModel>(abilities).AsReadOnly();
            _modifications = new List<ModificationModel>(modifications).AsReadOnly();
        }

        public string Id { get; }

        public string Name { get; }

        public Sprite Portrait { get; }

        public Sprite PartyIcon { get; }

        public string Description { get; }

        public CharacterStatsModel Stats { get; }

        public IReadOnlyList<AbilityModel> Abilities => _abilities;

        public IReadOnlyList<ModificationModel> Modifications => _modifications;
    }
}
