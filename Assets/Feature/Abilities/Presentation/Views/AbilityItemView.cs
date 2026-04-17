using System;
using Feature.Abilities.Presentation.Binding.Contracts;
using Feature.Abilities.Presentation.Views.Input;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Feature.Abilities.Presentation.Views
{
    /// <summary>
    /// Тонкий bind-слой элемента Ability, который связывает данные и пробрасывает hover-события.
    /// </summary>
    public sealed class AbilityItemView : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private Image _frameColorTarget;
        [SerializeField] private Image[] _dropFieldColorTargets = new Image[0];
        [SerializeField] private Color _dropFieldEnabledColor = Color.white;
        [SerializeField] private Image _slotIconImage;
        [SerializeField] private Image _slotColorTarget;
        [SerializeField] private UiPointerInputView _hoverPointerInputView;
        [SerializeField] private UiPointerInputView _slotPointerInputView;

        private IAbilityItemViewModel _viewModel;
        private Action<string> _onHoverEnter;
        private Action<string> _onHoverExit;
        private Action<string, PointerEventData> _onPointerDown;
        private Action<string, PointerEventData> _onPointerUp;

        private Color _initialFrameColor;
        private Color[] _initialDropFieldColors = new Color[0];
        private bool[] _hasInitialDropFieldColors = new bool[0];
        private Color _initialSlotColor;
        private bool _hasInitialFrameColor;
        private bool _hasInitialSlotColor;

        public string AbilityId
        {
            get
            {
                if (_viewModel == null)
                    return string.Empty;

                return _viewModel.Id;
            }
        }

        private void Awake()
        {
            if (_hoverPointerInputView == null)
                _hoverPointerInputView = GetComponent<UiPointerInputView>();

            if (_slotPointerInputView == null)
                _slotPointerInputView = _hoverPointerInputView;

            if (_hoverPointerInputView != null)
            {
                _hoverPointerInputView.onPointerEnter += OnPointerEnter;
                _hoverPointerInputView.onPointerExit += OnPointerExit;
            }

            if (_slotPointerInputView != null)
            {
                _slotPointerInputView.onPointerDown += OnPointerDown;
                _slotPointerInputView.onPointerUp += OnPointerUp;
            }

            if (_frameColorTarget != null)
            {
                _initialFrameColor = _frameColorTarget.color;
                _hasInitialFrameColor = true;
            }

            CacheInitialDropFieldColors();

            if (_slotColorTarget != null)
            {
                _initialSlotColor = _slotColorTarget.color;
                _hasInitialSlotColor = true;
            }
        }

        private void OnDestroy()
        {
            if (_hoverPointerInputView != null)
            {
                _hoverPointerInputView.onPointerEnter -= OnPointerEnter;
                _hoverPointerInputView.onPointerExit -= OnPointerExit;
            }

            if (_slotPointerInputView != null)
            {
                _slotPointerInputView.onPointerDown -= OnPointerDown;
                _slotPointerInputView.onPointerUp -= OnPointerUp;
            }
        }

        public void Bind(IAbilityItemViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            _viewModel = viewModel;
            Refresh();
        }

        public void SetInputHandlers(
            Action<string> onHoverEnter,
            Action<string> onHoverExit,
            Action<string, PointerEventData> onPointerDown,
            Action<string, PointerEventData> onPointerUp)
        {
            _onHoverEnter = onHoverEnter;
            _onHoverExit = onHoverExit;
            _onPointerDown = onPointerDown;
            _onPointerUp = onPointerUp;
        }

        public void HandleHoverEnter()
        {
            if (_viewModel == null)
                return;

            Action<string> handler = _onHoverEnter;
            if (handler != null)
                handler.Invoke(_viewModel.Id);
        }

        public void HandleHoverExit()
        {
            if (_viewModel == null)
                return;

            Action<string> handler = _onHoverExit;
            if (handler != null)
                handler.Invoke(_viewModel.Id);
        }

        public void HandlePointerDown(PointerEventData eventData)
        {
            if (_viewModel == null)
                return;

            Action<string, PointerEventData> handler = _onPointerDown;
            if (handler != null)
                handler.Invoke(_viewModel.Id, eventData);
        }

        public void HandlePointerUp(PointerEventData eventData)
        {
            if (_viewModel == null)
                return;

            Action<string, PointerEventData> handler = _onPointerUp;
            if (handler != null)
                handler.Invoke(_viewModel.Id, eventData);
        }

        public void Unbind()
        {
            _viewModel = null;
            _onHoverEnter = null;
            _onHoverExit = null;
            _onPointerDown = null;
            _onPointerUp = null;
        }

        public void Refresh()
        {
            if (_viewModel == null)
                return;

            if (_iconImage != null)
                _iconImage.sprite = _viewModel.Icon;

            if (_nameText != null)
                _nameText.text = _viewModel.Name;

            RefreshDropPreview();
            RefreshAppliedSlot();
        }

        private void RefreshDropPreview()
        {
            if (_frameColorTarget != null)
            {
                if (_viewModel.IsCompatibleDropTarget)
                    _frameColorTarget.color = ComposeColor(_viewModel.DropTargetColor, _initialFrameColor, _hasInitialFrameColor);
                else if (_hasInitialFrameColor)
                    _frameColorTarget.color = _initialFrameColor;
            }

            if (_viewModel.HasAppliedModification)
            {
                ApplyColorToDropFields(_dropFieldEnabledColor);
                return;
            }

            if (_viewModel.IsCompatibleDropTarget)
            {
                ApplyColorToDropFields(_viewModel.DropTargetColor);
                return;
            }

            RestoreInitialDropFieldColors();
        }

        private void RefreshAppliedSlot()
        {
            if (!_viewModel.HasAppliedModification)
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
                Sprite appliedIcon = _viewModel.AppliedModificationIcon;
                bool hasAppliedIcon = appliedIcon != null;
                _slotIconImage.sprite = appliedIcon;
                _slotIconImage.gameObject.SetActive(hasAppliedIcon);
            }

            if (_slotColorTarget != null)
                _slotColorTarget.color = ComposeColor(_viewModel.AppliedModificationColor, _initialSlotColor, _hasInitialSlotColor);
        }

        private void CacheInitialDropFieldColors()
        {
            if (_dropFieldColorTargets == null)
            {
                _initialDropFieldColors = new Color[0];
                _hasInitialDropFieldColors = new bool[0];
                return;
            }

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

        private void OnPointerEnter(PointerEventData eventData)
        {
            _ = eventData;
            HandleHoverEnter();
        }

        private void OnPointerExit(PointerEventData eventData)
        {
            _ = eventData;
            HandleHoverExit();
        }

        private void OnPointerDown(PointerEventData eventData)
        {
            HandlePointerDown(eventData);
        }

        private void OnPointerUp(PointerEventData eventData)
        {
            HandlePointerUp(eventData);
        }
    }
}
