using System;
using Feature.Party.Presentation.Binding.Contracts;
using Feature.Common.Presentation.Input;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Feature.Party.Presentation.Views
{

    /// <summary>
    /// Отвечает за отображение и обработку UI-блока Party Character Item.
    /// </summary>
    public sealed class PartyCharacterItemView : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private GameObject _selectedFrame;
        [SerializeField] private UiPointerInputView _pointerInputView;

        private IPartyCharacterItemViewModel _viewModel;
        private Action<string> _onClick;

        private void Awake()
        {
            if (_pointerInputView == null)
                _pointerInputView = GetComponent<UiPointerInputView>();

            if (_pointerInputView != null)
                _pointerInputView.onClick += OnPointerClick;
        }

        private void OnDestroy()
        {
            if (_pointerInputView != null)
                _pointerInputView.onClick -= OnPointerClick;
        }

        public void Bind(IPartyCharacterItemViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            _viewModel = viewModel;
            Refresh();
        }

        public void SetInputHandlers(Action<string> onClick)
        {
            _onClick = onClick;
        }

        public void HandleClick()
        {
            if (_viewModel == null)
                return;

            Action<string> handler = _onClick;
            if (handler != null)
                handler.Invoke(_viewModel.Id);
        }

        public void Unbind()
        {
            _viewModel = null;
            _onClick = null;
        }

        public void Refresh()
        {
            if (_viewModel == null)
                return;

            if (_iconImage != null)
                _iconImage.sprite = _viewModel.Icon;

            if (_selectedFrame != null)
                _selectedFrame.SetActive(_viewModel.IsSelected);
        }

        private void OnPointerClick(PointerEventData eventData)
        {
            _ = eventData;
            HandleClick();
        }
    }
}
