using System;
using Feature.Loadout.Presentation.Binding.Contracts;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.Loadout.Presentation.Views
{

    /// <summary>
    /// Отвечает за отображение и обработку UI-блока Dragged Modification Slot.
    /// </summary>
    public sealed class DraggedModificationSlotView : MonoBehaviour
    {
        [SerializeField] private RectTransform _slotRoot;
        [SerializeField] private Canvas _rootCanvas;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _colorTarget;

        private IModificationDragSlotViewModel _viewModel;
        private Color _initialColor;
        private bool _hasInitialColor;

        private void Awake()
        {
            if (_slotRoot == null)
                _slotRoot = transform as RectTransform;

            if (_colorTarget != null)
            {
                _initialColor = _colorTarget.color;
                _hasInitialColor = true;
            }
        }

        private void LateUpdate()
        {
            if (_viewModel == null || !_viewModel.IsActive)
                return;

            RectTransform rootCanvasRect = GetRootCanvasRect();
            if (_slotRoot == null || rootCanvasRect == null)
                return;

            RectTransform positionSpace = _slotRoot.parent as RectTransform;
            if (positionSpace == null)
                positionSpace = rootCanvasRect;

            Vector2 localPoint;
            Camera uiCamera = null;
            if (_rootCanvas != null && _rootCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
                uiCamera = _rootCanvas.worldCamera;

            bool converted = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                positionSpace,
                UnityEngine.Input.mousePosition,
                uiCamera,
                out localPoint);
            if (!converted)
                return;

            _slotRoot.anchoredPosition = localPoint;
        }

        private RectTransform GetRootCanvasRect()
        {
            if (_rootCanvas == null)
                return null;

            return _rootCanvas.transform as RectTransform;
        }

        public void Bind(IModificationDragSlotViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            Unbind();

            _viewModel = viewModel;
            _viewModel.onStateChanged += OnStateChanged;
            Refresh();
        }

        public void Unbind()
        {
            if (_viewModel != null)
                _viewModel.onStateChanged -= OnStateChanged;

            _viewModel = null;

            if (_slotRoot != null)
                _slotRoot.gameObject.SetActive(false);
        }

        public void Refresh()
        {
            if (_viewModel == null || _slotRoot == null)
                return;

            _slotRoot.gameObject.SetActive(_viewModel.IsActive);
            if (!_viewModel.IsActive)
                return;

            if (_iconImage != null)
                _iconImage.sprite = _viewModel.Icon;

            if (_colorTarget != null)
            {
                Color color = _viewModel.Color;
                if (_hasInitialColor)
                    color.a = _initialColor.a;
                else if (color.a <= 0f)
                    color.a = 1f;

                _colorTarget.color = color;
            }
        }

        private void OnDestroy()
        {
            Unbind();
        }

        private void OnStateChanged()
        {
            Refresh();
        }
    }
}
