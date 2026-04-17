using System;
using Feature.CharacterSelection.Core.Domain.Models;
using Feature.CharacterSelection.Core.Enums;
using Feature.Modifications.Presentation.Binding.Contracts;
using UnityEngine;

namespace Feature.Modifications.Presentation.ViewModels
{

    /// <summary>
    /// Хранит и обновляет состояние представления Modification Item для UI.
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
            IsInteractable = true;
            IsDimmed = false;
        }

        public string Id { get; }

        public string Name { get; }

        public string Description { get; }

        public Sprite Icon { get; }

        public ModificationType ModificationType { get; }

        public string TypeDisplayName { get; }

        public Sprite TypeIcon { get; }

        public Color TypeColor { get; }

        public bool IsInteractable { get; private set; }

        public bool IsDimmed { get; private set; }

        public void SetInteractableState(bool isInteractable, bool isDimmed)
        {
            IsInteractable = isInteractable;
            IsDimmed = isDimmed;
        }

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
