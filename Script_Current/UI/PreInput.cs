using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PreInput : MonoBehaviour
{
    public float pressedTime;
    public float releasedTime;
    public bool isCalled;
    public float delay = 2.0f;
    public delegate bool CanCall();
    public void Pressed()
    {
        pressedTime = Time.time;
    }

    public void Released()
    {
        releasedTime = Time.time;
    }

}
