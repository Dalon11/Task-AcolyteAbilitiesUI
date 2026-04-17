using UnityEngine;
using UnityEngine.UI;

namespace Feature.Abilities.Presentation.Views
{
    /// <summary>
    /// Настраивает tooltip как неинтерактивный слой, не блокирующий raycast.
    /// </summary>
    public sealed class TooltipRaycastConfigurator
    {
        private readonly RectTransform _panelRect;
        private CanvasGroup _canvasGroup;

        public TooltipRaycastConfigurator(RectTransform panelRect)
        {
            _panelRect = panelRect;
        }

        public void EnsureIgnoresRaycasts()
        {
            if (_panelRect == null)
                return;

            if (_canvasGroup == null)
                _canvasGroup = _panelRect.GetComponent<CanvasGroup>();

            if (_canvasGroup == null)
                _canvasGroup = _panelRect.gameObject.AddComponent<CanvasGroup>();

            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            Graphic[] graphics = _panelRect.GetComponentsInChildren<Graphic>(true);
            for (int i = 0; i < graphics.Length; i++)
            {
                Graphic graphic = graphics[i];
                if (graphic == null)
                    continue;

                graphic.raycastTarget = false;
            }
        }
    }
}
