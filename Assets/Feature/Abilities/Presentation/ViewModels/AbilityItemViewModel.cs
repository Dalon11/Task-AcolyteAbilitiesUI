using System;
using System.Collections.Generic;
using Feature.Abilities.Domain.Models;
using Feature.Abilities.Enums;
using Feature.Abilities.Presentation.Binding.Contracts;
using UnityEngine;

namespace Feature.Abilities.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel элемента списка способностей.
    /// </summary>
    public sealed class AbilityItemViewModel : IAbilityItemViewModel
    {
        private readonly IReadOnlyList<ModificationType> _supportedModificationTypes;

        public AbilityItemViewModel(AbilityModel ability)
        {
            if (ability == null)
                throw new ArgumentNullException(nameof(ability));

            Id = ability.Id;
            Name = ability.Name;
            Icon = ability.Icon;
            Description = ability.Description;
            AbilityType = ability.AbilityType;
            _supportedModificationTypes = new List<ModificationType>(ability.SupportedModificationTypes).AsReadOnly();
        }

        public string Id { get; }

        public string Name { get; }

        public Sprite Icon { get; }

        public string Description { get; }

        public AbilityType AbilityType { get; }

        public IReadOnlyList<ModificationType> SupportedModificationTypes => _supportedModificationTypes;

        public bool TryGetTooltipContent(out string header, out string description)
        {
            if (string.IsNullOrWhiteSpace(Name) || Description == null)
            {
                header = string.Empty;
                description = string.Empty;
                return false;
            }

            header = Name;
            description = Description;
            return true;
        }
    }
}
