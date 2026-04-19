using System;
using Feature.CharacterPaper.Presentation.Binding.Contracts;
using Feature.Common.Presentation.Views;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Feature.CharacterPaper.Presentation.Views
{

    /// <summary>
    /// Отвечает за отображение и обработку UI-блока Character Paper.
    /// </summary>
    public sealed class CharacterPaperView : MonoBehaviour
    {
        [SerializeField] private Image _characterImage;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _hpText;
        [SerializeField] private TMP_Text _armorText;
        [SerializeField] private UiPointerInputView _pointerInputView;

        private ICharacterPaperViewModel _viewModel;
        private Action _onHoverEnter;
        private Action _onHoverExit;

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

            Unbind();
        }

        public void Bind(ICharacterPaperViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            Unbind();

            _viewModel = viewModel;
            _viewModel.onStateChanged += OnViewModelStateChanged;

            Refresh();
        }

        public void SetInputHandlers(Action onHoverEnter, Action onHoverExit)
        {
            _onHoverEnter = onHoverEnter;
            _onHoverExit = onHoverExit;
        }

        public void Unbind()
        {
            if (_viewModel != null)
                _viewModel.onStateChanged -= OnViewModelStateChanged;

            _viewModel = null;
            _onHoverEnter = null;
            _onHoverExit = null;
        }

        public void Refresh()
        {
            if (_viewModel == null)
                return;

            if (_characterImage != null)
                _characterImage.sprite = _viewModel.CharacterSprite;

            if (_nameText != null)
                _nameText.text = _viewModel.Name;

            if (_hpText != null)
                _hpText.text = BuildCurrentAndMaxText(_viewModel.Hp, _viewModel.MaxHp);

            if (_armorText != null)
                _armorText.text = BuildCurrentAndMaxText(_viewModel.Armor, _viewModel.Armor);
        }

        private void OnViewModelStateChanged()
        {
            Refresh();
        }

        private string BuildCurrentAndMaxText(int currentValue, int maxValue)
        {
            return currentValue + "/" + maxValue;
        }

        private void OnPointerEnter(PointerEventData eventData)
        {
            _ = eventData;

            Action handler = _onHoverEnter;
            if (handler != null)
                handler.Invoke();
        }

        private void OnPointerExit(PointerEventData eventData)
        {
            _ = eventData;

            Action handler = _onHoverExit;
            if (handler != null)
                handler.Invoke();
        }
    }
}
