using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class NormalButton : PreInput,
                            IPointerDownHandler,
                            IPointerUpHandler
{
    [HideInInspector]
    public UnityEvent onButtonDown;
    [HideInInspector]
    public UnityEvent onButtonUp;

    public bool pressed = false;

    public string ButtonKey = "";
    void Update()
    {
        if(Input.GetButtonDown(ButtonKey))
        {
            Pressed();
            isCalled = true;
            pressed = true;
            onButtonDown.Invoke();
        }
        if(Input.GetButtonUp(ButtonKey))
        {
            Released();
            pressed = false;
            onButtonUp.Invoke();
        }
    }
    public virtual void OnPointerDown(PointerEventData data)
    {
        Pressed();
        isCalled = true;
        pressed = true;
        onButtonDown.Invoke();
    }
    public virtual void OnPointerUp(PointerEventData data)
    {
        Released();
        pressed = false;
        onButtonUp.Invoke();
    }
}
