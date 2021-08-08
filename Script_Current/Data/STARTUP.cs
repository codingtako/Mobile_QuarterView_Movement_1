using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STARTUP : MonoBehaviour
{
    /// <summary>
    /// 위에서 봤을 경우의 벡터 사이의 각도 반환
    /// </summary>
    /// <param name="vec1"></param>
    /// <param name="vec2"></param>
    /// <returns></returns>
    public static float Angle(Vector3 vec1, Vector3 vec2)
    {
        float angle1 = Mathf.Atan2(vec1.z, vec1.x) * Mathf.Rad2Deg;
        float angle2 = Mathf.Atan2(vec2.z, vec2.x) * Mathf.Rad2Deg;
        float angle = angle1 - angle2;
        while (angle < -180) angle += 360;
        while (angle > 180) angle -= 360;

        return angle;
    }

    public static int FirstDigit(int targetInt)
    {
        float f=targetInt;
        while (f>10)
        {
            f *= 0.1f;
        }

        return Mathf.FloorToInt(f);
    }
    public static int FirstDigit(float f)
    {
        while (f>10)
        {
            f *= 0.1f;
        }

        return Mathf.FloorToInt(f);
    }
}


