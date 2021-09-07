using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Aya.TweenPro
{
    public enum UIEventType
    {
        None = 0,
        Enter = 1,
        Leave = 2,
        Hover = 3,  // Enter - Exit
        Down = 4,
        Up = 5,
        Click = 6,
        DoubleClick = 7
    }

    [Serializable]
    public class UIEventTrigger : MonoBehaviour, ITweenAutoPlayTrigger,
        IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public TweenData Data { get; set; }
        public UIEventType Type;

        public void Register(TweenData tweenData)
        {
            Data = tweenData;
        }

        public void Play(bool forward = true)
        {
            Data.Play(forward);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Type != UIEventType.Click) return;
            Data.Play();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Type != UIEventType.Down) return;
            Data.Play();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Type == UIEventType.Enter || Type == UIEventType.Hover) Data.Play();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (Type == UIEventType.Leave) Data.Play();
            if (Type == UIEventType.Hover) Data.Play(false);

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (Type != UIEventType.Up) return;
            Data.Play();
        }
    }
}