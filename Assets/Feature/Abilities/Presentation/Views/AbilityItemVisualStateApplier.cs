using Feature.Abilities.Presentation.Binding.Contracts;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.Abilities.Presentation.Views
{

    /// <summary>
    /// Применяет визуальное состояние элемента способности по данным его ViewModel.
    /// </summary>
    public sealed class AbilityItemVisualStateApplier
    {
        private readonly Image _frameColorTarget;
        private readonly Image[] _dropFieldColorTargets;
        private readonly Color _dropFieldEnabledColor;
        private readonly Image _slotIconImage;
        private readonly Image _slotColorTarget;

        private readonly Color _initialFrameColor;
        private readonly Color[] _initialDropFieldColors;
        private readonly bool[] _hasInitialDropFieldColors;
        private readonly Color _initialSlotColor;

        private readonly bool _hasInitialFrameColor;
        private readonly bool _hasInitialSlotColor;

        public AbilityItemVisualStateApplier(
            Image frameColorTarget,
            Image[] dropFieldColorTargets,
            Color dropFieldEnabledColor,
            Image slotIconImage,
            Image slotColorTarget)
        {
            _frameColorTarget = frameColorTarget;
            _dropFieldColorTargets = dropFieldColorTargets;
            _dropFieldEnabledColor = dropFieldEnabledColor;
            _slotIconImage = slotIconImage;
            _slotColorTarget = slotColorTarget;

            if (_frameColorTarget != null)
            {
                _initialFrameColor = _frameColorTarget.color;
                _hasInitialFrameColor = true;
            }

            if (_dropFieldColorTargets == null)
            {
                _initialDropFieldColors = new Color[0];
                _hasInitialDropFieldColors = new bool[0];
            }
            else
            {
                _initialDropFieldColors = new Color[_dropFieldColorTargets.Length];
                _hasInitialDropFieldColors = new bool[_dropFieldColorTargets.Length];

                for (int i = 0; i < _dropFieldColorTargets.Length; i++)
                {
                    Image target = _dropFieldColorTargets[i];
                    if (target == null)
                        continue;

                    _initialDropFieldColors[i] = target.color;
                    _hasInitialDropFieldColors[i] = true;
                }
            }

            if (_slotColorTarget != null)
            {
                _initialSlotColor = _slotColorTarget.color;
                _hasInitialSlotColor = true;
            }
        }

        public void Apply(IAbilityItemViewModel viewModel)
        {
            if (viewModel == null)
                return;

            ApplyDropPreview(viewModel);
            ApplyAppliedSlot(viewModel);
        }

        private void ApplyDropPreview(IAbilityItemViewModel viewModel)
        {
            if (_frameColorTarget != null)
            {
                if (viewModel.IsCompatibleDropTarget)
                    _frameColorTarget.color = ComposeColor(viewModel.DropTargetColor, _initialFrameColor, _hasInitialFrameColor);
                else if (_hasInitialFrameColor)
                    _frameColorTarget.color = _initialFrameColor;
            }

            if (viewModel.HasAppliedModification)
            {
                ApplyColorToDropFields(_dropFieldEnabledColor);
                return;
            }

            if (viewModel.IsCompatibleDropTarget)
            {
                ApplyColorToDropFields(viewModel.DropTargetColor);
                return;
            }

            RestoreInitialDropFieldColors();
        }

        private void ApplyAppliedSlot(IAbilityItemViewModel viewModel)
        {
            if (!viewModel.HasAppliedModification)
            {
                if (_slotIconImage != null)
                {
                    _slotIconImage.sprite = null;
                    _slotIconImage.gameObject.SetActive(false);
                }

                if (_slotColorTarget != null && _hasInitialSlotColor)
                    _slotColorTarget.color = _initialSlotColor;

                return;
            }

            if (_slotIconImage != null)
            {
                Sprite appliedIcon = viewModel.AppliedModificationIcon;
                bool hasAppliedIcon = appliedIcon != null;
                _slotIconImage.sprite = appliedIcon;
                _slotIconImage.gameObject.SetActive(hasAppliedIcon);
            }

            if (_slotColorTarget != null)
                _slotColorTarget.color = ComposeColor(viewModel.AppliedModificationColor, _initialSlotColor, _hasInitialSlotColor);
        }

        private void ApplyColorToDropFields(Color sourceColor)
        {
            if (_dropFieldColorTargets == null)
                return;

            for (int i = 0; i < _dropFieldColorTargets.Length; i++)
            {
                Image target = _dropFieldColorTargets[i];
                if (target == null)
                    continue;

                bool hasInitial = i < _hasInitialDropFieldColors.Length && _hasInitialDropFieldColors[i];
                Color initialColor = i < _initialDropFieldColors.Length ? _initialDropFieldColors[i] : Color.clear;
                target.color = ComposeColor(sourceColor, initialColor, hasInitial);
            }
        }

        private void RestoreInitialDropFieldColors()
        {
            if (_dropFieldColorTargets == null)
                return;

            for (int i = 0; i < _dropFieldColorTargets.Length; i++)
            {
                Image target = _dropFieldColorTargets[i];
                if (target == null)
                    continue;

                if (i >= _hasInitialDropFieldColors.Length || !_hasInitialDropFieldColors[i])
                    continue;

                target.color = _initialDropFieldColors[i];
            }
        }

        private Color ComposeColor(Color source, Color baseColor, bool hasBaseColor)
        {
            Color result = source;
            if (hasBaseColor)
                result.a = baseColor.a;
            else if (result.a <= 0f)
                result.a = 1f;

            return result;
        }
    }
}
