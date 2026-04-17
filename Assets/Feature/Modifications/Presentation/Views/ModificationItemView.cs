using System;
using Feature.Modifications.Presentation.Binding.Contracts;
using Feature.Common.Presentation.Input;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Feature.Modifications.Presentation.Views
{
    /// <summary>
    /// ������ bind-���� �������� Modification.
    /// </summary>
    public sealed class ModificationItemView : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _typeDisplayNameText;
        [SerializeField] private Image _typeIconImage;
        [SerializeField] private Image _typeColorTarget;
        [SerializeField] private GameObject _dimOverlay;
        [SerializeField] private UiPointerInputView _pointerInputView;

        private IModificationItemViewModel _viewModel;
        private Action<string> _onHoverEnter;
        private Action<string> _onHoverExit;
        private Action<string, PointerEventData> _onPointerDown;
        private Action<string, PointerEventData> _onPointerUp;
        private Color _initialTypeColor;
        private bool _hasInitialTypeColor;

        private void Awake()
        {
            if (_pointerInputView == null)
                _pointerInputView = GetComponent<UiPointerInputView>();

            if (_pointerInputView != null)
            {
                _pointerInputView.onPointerEnter += OnPointerEnter;
                _pointerInputView.onPointerExit += OnPointerExit;
                _pointerInputView.onPointerDown += OnPointerDown;
                _pointerInputView.onPointerUp += OnPointerUp;
            }

            if (_typeColorTarget != null)
            {
                _initialTypeColor = _typeColorTarget.color;
                _hasInitialTypeColor = true;
            }
        }

        private void OnDestroy()
        {
            if (_pointerInputView != null)
            {
                _pointerInputView.onPointerEnter -= OnPointerEnter;
                _pointerInputView.onPointerExit -= OnPointerExit;
                _pointerInputView.onPointerDown -= OnPointerDown;
                _pointerInputView.onPointerUp -= OnPointerUp;
            }
        }

        public void Bind(IModificationItemViewModel viewModel)
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

            if (!_viewModel.IsInteractable)
                return;

            Action<string> handler = _onHoverEnter;
            if (handler != null)
                handler.Invoke(_viewModel.Id);
        }

        public void HandleHoverExit()
        {
            if (_viewModel == null)
                return;

            if (!_viewModel.IsInteractable)
                return;

            Action<string> handler = _onHoverExit;
            if (handler != null)
                handler.Invoke(_viewModel.Id);
        }

        public void HandlePointerDown(PointerEventData eventData)
        {
            if (_viewModel == null)
                return;

            if (!_viewModel.IsInteractable)
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

            if (_typeDisplayNameText != null)
                _typeDisplayNameText.text = _viewModel.TypeDisplayName;

            if (_typeIconImage != null)
                _typeIconImage.sprite = _viewModel.TypeIcon;

            if (_typeColorTarget != null)
            {
                Color typeColor = _viewModel.TypeColor;
                if (_hasInitialTypeColor)
                    typeColor.a = _initialTypeColor.a;
                else if (typeColor.a <= 0f)
                    typeColor.a = 1f;

                _typeColorTarget.color = typeColor;
            }

            if (_dimOverlay != null)
                _dimOverlay.SetActive(_viewModel.IsDimmed);
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



