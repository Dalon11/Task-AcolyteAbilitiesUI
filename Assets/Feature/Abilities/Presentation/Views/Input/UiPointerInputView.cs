using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Feature.Abilities.Presentation.Views.Input
{
    /// <summary>
    /// Переиспользуемый компонент, который пробрасывает pointer-события UI-элемента в подписки.
    /// </summary>
    public sealed class UiPointerInputView : MonoBehaviour,
        IPointerClickHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler
    {
        public event Action<PointerEventData> onClick;
        public event Action<PointerEventData> onPointerEnter;
        public event Action<PointerEventData> onPointerExit;
        public event Action<PointerEventData> onPointerDown;
        public event Action<PointerEventData> onPointerUp;
        public event Action<PointerEventData> onBeginDrag;
        public event Action<PointerEventData> onDrag;
        public event Action<PointerEventData> onEndDrag;

        public void OnPointerClick(PointerEventData eventData)
        {
            Action<PointerEventData> handler = onClick;
            if (handler != null)
                handler.Invoke(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Action<PointerEventData> handler = onPointerEnter;
            if (handler != null)
                handler.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Action<PointerEventData> handler = onPointerExit;
            if (handler != null)
                handler.Invoke(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Action<PointerEventData> handler = onPointerDown;
            if (handler != null)
                handler.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Action<PointerEventData> handler = onPointerUp;
            if (handler != null)
                handler.Invoke(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Action<PointerEventData> handler = onBeginDrag;
            if (handler != null)
                handler.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Action<PointerEventData> handler = onDrag;
            if (handler != null)
                handler.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Action<PointerEventData> handler = onEndDrag;
            if (handler != null)
                handler.Invoke(eventData);
        }
    }
}
