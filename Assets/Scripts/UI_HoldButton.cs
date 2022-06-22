using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool enableButton = true;

    public UnityEvent OnHold;
    public UnityEvent OnRelease;
    public Sprite despulsado;
    public Sprite pulsado;
    public Image image;

    void Awake()
    {
        if (image==null) image = GetComponent<Image>();
    }

    public void OnPointerDown (PointerEventData eventData)
    {
        if (enableButton)
        {
            OnHold.Invoke();
            image.sprite = pulsado;
        }
    }

    public void OnPointerUp (PointerEventData eventData)
    {
        if (enableButton)
        {
            OnRelease.Invoke();
            image.sprite = despulsado;
        }
    }
}