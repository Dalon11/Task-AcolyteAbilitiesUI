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
        private Color _dropTargetColor;
        private Color _appliedModificationColor;

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
            _dropTargetColor = Color.clear;
            _appliedModificationColor = Color.clear;
            AppliedModificationId = string.Empty;
            AppliedModificationType = ModificationType.Unknown;
        }

        public string Id { get; }

        public string Name { get; }

        public Sprite Icon { get; }

        public string Description { get; }

        public AbilityType AbilityType { get; }

        public IReadOnlyList<ModificationType> SupportedModificationTypes => _supportedModificationTypes;

        public bool IsCompatibleDropTarget { get; private set; }

        public Color DropTargetColor => _dropTargetColor;

        public bool HasAppliedModification { get; private set; }

        public string AppliedModificationId { get; private set; }

        public ModificationType AppliedModificationType { get; private set; }

        public Sprite AppliedModificationIcon { get; private set; }

        public Color AppliedModificationColor => _appliedModificationColor;

        public bool SupportsModificationType(ModificationType modificationType)
        {
            for (int i = 0; i < _supportedModificationTypes.Count; i++)
            {
                if (_supportedModificationTypes[i] == modificationType)
                    return true;
            }

            return false;
        }

        public void SetDropTargetState(bool isCompatibleDropTarget, Color dropTargetColor)
        {
            IsCompatibleDropTarget = isCompatibleDropTarget;
            _dropTargetColor = isCompatibleDropTarget ? dropTargetColor : Color.clear;
        }

        public void ApplyModification(string modificationId, ModificationType modificationType, Sprite icon, Color color)
        {
            HasAppliedModification = true;
            AppliedModificationId = modificationId;
            AppliedModificationType = modificationType;
            AppliedModificationIcon = icon;
            _appliedModificationColor = color;
        }

        public bool TryTakeAppliedModification(
            out string modificationId,
            out ModificationType modificationType,
            out Sprite icon,
            out Color color)
        {
            if (!HasAppliedModification)
            {
                modificationId = string.Empty;
                modificationType = ModificationType.Unknown;
                icon = null;
                color = Color.clear;
                return false;
            }

            modificationId = AppliedModificationId;
            modificationType = AppliedModificationType;
            icon = AppliedModificationIcon;
            color = _appliedModificationColor;

            ClearAppliedModification();
            return true;
        }

        public void ClearAppliedModification()
        {
            HasAppliedModification = false;
            AppliedModificationId = string.Empty;
            AppliedModificationType = ModificationType.Unknown;
            AppliedModificationIcon = null;
            _appliedModificationColor = Color.clear;
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
