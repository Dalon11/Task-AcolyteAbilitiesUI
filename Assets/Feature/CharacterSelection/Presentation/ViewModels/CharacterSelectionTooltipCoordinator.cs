using System;
using Feature.Abilities.Presentation.ViewModels;
using Feature.CharacterPaper.Presentation.ViewModels;
using Feature.CharacterSelection.Core.Domain.Models;
using Feature.Modifications.Presentation.ViewModels;
using Feature.Tooltip.Presentation.ViewModels;

namespace Feature.CharacterSelection.Presentation.ViewModels
{
    /// <summary>
    /// Координирует hover-flow и видимость единого tooltip экрана.
    /// </summary>
    public sealed class CharacterSelectionTooltipCoordinator
    {
        private readonly CharacterPaperViewModel _characterPaper;
        private readonly AbilitiesListViewModel _abilities;
        private readonly ModificationsListViewModel _modifications;
        private readonly TooltipViewModel _tooltip;

        public CharacterSelectionTooltipCoordinator(
            CharacterPaperViewModel characterPaper,
            AbilitiesListViewModel abilities,
            ModificationsListViewModel modifications,
            TooltipViewModel tooltip)
        {
            if (characterPaper == null)
                throw new ArgumentNullException(nameof(characterPaper));

            if (abilities == null)
                throw new ArgumentNullException(nameof(abilities));

            if (modifications == null)
                throw new ArgumentNullException(nameof(modifications));

            if (tooltip == null)
                throw new ArgumentNullException(nameof(tooltip));

            _characterPaper = characterPaper;
            _abilities = abilities;
            _modifications = modifications;
            _tooltip = tooltip;
        }

        public void OnCharacterHoverEnter()
        {
            string header;
            string description;
            if (!_characterPaper.TryGetTooltipContent(out header, out description))
            {
                HideTooltip();
                return;
            }

            ShowTooltip(header, description);
        }

        public void OnAbilityHoverEnter(string abilityId)
        {
            string header;
            string description;
            if (!_abilities.TryGetTooltipContent(abilityId, out header, out description))
            {
                HideTooltip();
                return;
            }

            ShowTooltip(header, description);
        }

        public void OnModificationHoverEnter(string modificationId)
        {
            string header;
            string description;
            if (!_modifications.TryGetTooltipContent(modificationId, out header, out description))
            {
                HideTooltip();
                return;
            }

            ShowTooltip(header, description);
        }

        public void OnHoverExit()
        {
            HideTooltip();
        }

        public void HideTooltip()
        {
            _tooltip.Hide();
        }

        private void ShowTooltip(string header, string description)
        {
            if (string.IsNullOrWhiteSpace(header) || description == null)
            {
                HideTooltip();
                return;
            }

            TooltipModel tooltip = new TooltipModel(header, description);
            _tooltip.Show(tooltip);
        }
    }
}



