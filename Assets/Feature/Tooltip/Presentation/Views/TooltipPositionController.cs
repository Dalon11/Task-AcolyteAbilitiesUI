using UnityEngine;

namespace Feature.Tooltip.Presentation.Views
{
    /// <summary>
    /// ��������� ����������������� tooltip ������������ ������� � ������ ������.
    /// </summary>
    public sealed class TooltipPositionController
    {
        private static readonly Vector2 CursorAnchorPivot = new Vector2(0f, 1f);

        private readonly RectTransform _panelRect;
        private readonly Canvas _rootCanvas;
        private readonly Vector2 _cursorOffset;
        private readonly float _screenPadding;

        public TooltipPositionController(
            RectTransform panelRect,
            Canvas rootCanvas,
            Vector2 cursorOffset,
            float screenPadding)
        {
            _panelRect = panelRect;
            _rootCanvas = rootCanvas;
            _cursorOffset = cursorOffset;
            _screenPadding = screenPadding;

            if (_panelRect != null)
                _panelRect.pivot = CursorAnchorPivot;
        }

        public void UpdatePosition()
        {
            if (_panelRect == null)
                return;

            Vector2 screenPosition = (Vector2)UnityEngine.Input.mousePosition + _cursorOffset;
            Vector2 clampedScreenPosition = ClampToScreen(screenPosition);

            RectTransform parentRect = _panelRect.parent as RectTransform;
            if (parentRect == null)
            {
                _panelRect.position = clampedScreenPosition;
                return;
            }

            Camera eventCamera = null;
            if (_rootCanvas != null && _rootCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
                eventCamera = _rootCanvas.worldCamera;

            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, clampedScreenPosition, eventCamera, out localPoint))
                _panelRect.anchoredPosition = localPoint;
        }

        private Vector2 ClampToScreen(Vector2 screenPosition)
        {
            if (_panelRect == null)
                return screenPosition;

            Rect rect = _panelRect.rect;
            Vector2 pivot = _panelRect.pivot;

            float minX = (rect.width * pivot.x) + _screenPadding;
            float maxX = Screen.width - (rect.width * (1f - pivot.x)) - _screenPadding;
            float minY = (rect.height * pivot.y) + _screenPadding;
            float maxY = Screen.height - (rect.height * (1f - pivot.y)) - _screenPadding;

            float clampedX = Mathf.Clamp(screenPosition.x, minX, maxX);
            float clampedY = Mathf.Clamp(screenPosition.y, minY, maxY);

            return new Vector2(clampedX, clampedY);
        }
    }
}



