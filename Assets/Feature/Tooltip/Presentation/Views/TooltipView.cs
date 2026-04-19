using System;
using Feature.Tooltip.Presentation.Binding.Contracts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.Tooltip.Presentation.Views
{

    /// <summary>
    /// Отвечает за отображение и обработку UI-блока Tooltip.
    /// </summary>
    public sealed class TooltipView : MonoBehaviour
    {
        [SerializeField] private RectTransform _panelRect;
        [SerializeField] private Canvas _rootCanvas;
        [SerializeField] private TMP_Text _headerText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Vector2 _cursorOffset = Vector2.zero;
        [SerializeField] private float _screenPadding = 10f;

        private ITooltipViewModel _viewModel;
        private TooltipPositionController _positionController;
        private string _lastHeader;
        private string _lastDescription;
        private bool _wasVisible;

        private bool IsAnyMouseButtonDown =>
            Input.GetMouseButtonDown(0) ||
            Input.GetMouseButtonDown(1) ||
            Input.GetMouseButtonDown(2);

        private void Awake()
        {
            float resolvedScreenPadding = Mathf.Max(0f, _screenPadding);

            _positionController = new TooltipPositionController(_panelRect, _rootCanvas, _cursorOffset, resolvedScreenPadding);
        }

        private void Update()
        {
            if (_viewModel == null)
                return;

            if (!_viewModel.IsVisible)
                return;

            if (IsAnyMouseButtonDown)
                _viewModel.Hide();
        }

        public void Bind(ITooltipViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            Unbind();

            _viewModel = viewModel;
            _viewModel.onStateChanged += OnViewModelStateChanged;

            Refresh();
        }

        public void Unbind()
        {
            if (_viewModel == null)
                return;

            _viewModel.onStateChanged -= OnViewModelStateChanged;
            _viewModel = null;
            _lastHeader = null;
            _lastDescription = null;
            _wasVisible = false;
        }

        public void Refresh()
        {
            if (_viewModel == null)
                return;

            if (_panelRect != null)
                _panelRect.gameObject.SetActive(_viewModel.IsVisible);

            ITooltipContentViewModel content = _viewModel.Content;
            bool isVisible = _viewModel.IsVisible;
            bool isContentChanged =
                !string.Equals(_lastHeader, content.Header, StringComparison.Ordinal) ||
                !string.Equals(_lastDescription, content.Description, StringComparison.Ordinal);

            if (_headerText != null)
                _headerText.text = content.Header;

            if (_descriptionText != null)
                _descriptionText.text = content.Description;

            if (_panelRect != null && isVisible)
            {
                if (isContentChanged || !_wasVisible)
                    RebuildTooltipLayout();

                UpdateTooltipPosition();
            }

            _lastHeader = content.Header;
            _lastDescription = content.Description;
            _wasVisible = isVisible;
        }

        private void OnDestroy()
        {
            Unbind();
        }

        private void OnViewModelStateChanged()
        {
            Refresh();
        }

        private void UpdateTooltipPosition()
        {
            if (_positionController == null)
                return;

            _positionController.UpdatePosition();
        }

        private void RebuildTooltipLayout()
        {
            if (_headerText != null)
                _headerText.ForceMeshUpdate();

            if (_descriptionText != null)
                _descriptionText.ForceMeshUpdate();

            if (_panelRect != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(_panelRect);
        }

    }
}
