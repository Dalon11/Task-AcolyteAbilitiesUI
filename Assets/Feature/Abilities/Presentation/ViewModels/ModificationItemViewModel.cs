using System;
using Feature.Abilities.Domain.Models;
using Feature.Abilities.Enums;
using Feature.Abilities.Presentation.Binding.Contracts;
using UnityEngine;

namespace Feature.Abilities.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel элемента списка модификаторов.
    /// </summary>
    public sealed class ModificationItemViewModel : IModificationItemViewModel
    {
        public ModificationItemViewModel(ModificationModel modification)
        {
            if (modification == null)
                throw new ArgumentNullException(nameof(modification));

            Id = modification.Id;
            Name = modification.Name;
            Description = modification.Description;
            Icon = modification.Icon;
            ModificationType = modification.ModificationType;
            TypeDisplayName = modification.TypePresentation.DisplayName;
            TypeIcon = modification.TypePresentation.Icon;
            TypeColor = modification.TypePresentation.Color;
        }

        public string Id { get; }

        public string Name { get; }

        public string Description { get; }

        public Sprite Icon { get; }

        public ModificationType ModificationType { get; }

        public string TypeDisplayName { get; }

        public Sprite TypeIcon { get; }

        public Color TypeColor { get; }

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
