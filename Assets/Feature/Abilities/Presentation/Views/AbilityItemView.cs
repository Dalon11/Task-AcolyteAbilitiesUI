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
        [SerializeField] private TMP_Text _abilityTypeText;
        [SerializeField] private TMP_Text _supportedTypesText;
        [SerializeField] private UiPointerInputView _pointerInputView;

        private IAbilityItemViewModel _viewModel;
        private Action<string> _onHoverEnter;
        private Action<string> _onHoverExit;

        private void Awake()
        {
            if (_pointerInputView == null)
                _pointerInputView = GetComponent<UiPointerInputView>();

            if (_pointerInputView != null)
            {
                _pointerInputView.onPointerEnter += OnPointerEnter;
                _pointerInputView.onPointerExit += OnPointerExit;
            }
        }

        private void OnDestroy()
        {
            if (_pointerInputView != null)
            {
                _pointerInputView.onPointerEnter -= OnPointerEnter;
                _pointerInputView.onPointerExit -= OnPointerExit;
            }
        }

        public void Bind(IAbilityItemViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            _viewModel = viewModel;
            Refresh();
        }

        public void SetInputHandlers(Action<string> onHoverEnter, Action<string> onHoverExit)
        {
            _onHoverEnter = onHoverEnter;
            _onHoverExit = onHoverExit;
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

        public void Unbind()
        {
            _viewModel = null;
            _onHoverEnter = null;
            _onHoverExit = null;
        }

        public void Refresh()
        {
            if (_viewModel == null)
                return;

            if (_iconImage != null)
                _iconImage.sprite = _viewModel.Icon;

            if (_nameText != null)
                _nameText.text = _viewModel.Name;

            if (_abilityTypeText != null)
                _abilityTypeText.text = _viewModel.AbilityType.ToString();

            if (_supportedTypesText != null)
                _supportedTypesText.text = string.Join(", ", _viewModel.SupportedModificationTypes);
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
    }
}
