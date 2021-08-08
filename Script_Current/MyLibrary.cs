using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLibrary : MonoBehaviour
{
    public static float ClampDegree(float a,float b,float value)
    {
        float min = Mathf.Min(a, b);
        float max = Mathf.Max(a, b);
        float dist = max - min;
        while (value < min)
        {
            value += dist;
        }
        while (value > max)
        {
            value -= dist;
        }
        return value;
    }

    public static float AngleBetweenVec(Vector3 v1, Vector3 v2)
    {
        v1.y = 0;
        v2.y = 0;
        return Mathf.Acos(Vector3.Dot(v1,v2)/v1.magnitude/v2.magnitude);
    }

    public static bool IsBetweenVec(Vector3 v1, Vector3 v2, Vector3 middleVec)
    {
        float dot1 = Vector3.Dot(v1, middleVec);
        float dot2 = Vector3.Dot(v2, middleVec);
        
        return dot1>0&&dot2>=0;
    }

    public static float VecAngle(Vector3 vec)
    {
        return Mathf.Atan2(vec.z, vec.x)%2;
    }
    public static float BetweenDegree(float _degree)
    {
        while (_degree < -180) _degree += 360;
        while (_degree > 180) _degree -= 360;
        return _degree;
    }
}
