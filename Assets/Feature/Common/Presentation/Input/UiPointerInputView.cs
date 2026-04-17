using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Feature.Common.Presentation.Input
{

    /// <summary>
    /// Отвечает за отображение и обработку UI-блока Ui Pointer Input.
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
            _ = eventData;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _ = eventData;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _ = eventData;
        }
    }
}
