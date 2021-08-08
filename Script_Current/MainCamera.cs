using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
public class MainCamera : MonoBehaviour
{
    public static MainCamera instance;
    public Transform target;
    public float followSpeed = 4;

    public float stopScale = 0.2f;
    public float stopDuration=0.2f;
    [FoldoutGroup("Shake")]
    public GameObject shake;

    [FoldoutGroup("Shake")] public float shakeDuration,shakeScale;
    [FoldoutGroup("Shake")] public int shakeVibrato;
    
    
    
    
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime*followSpeed);
    }

    public void Stop()
    {
        Time.timeScale = stopScale;
        StartCoroutine("TimeReset");
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void Shake()
    {
        DOTween.Kill("Cam_Shake");
        shake.transform.localPosition = Vector3.zero;
        shake.transform.DOShakePosition(shakeDuration, shakeScale, shakeVibrato).SetId("Cam_Shake").SetUpdate(true);
    }
    IEnumerator TimeReset()
    {
        yield return new WaitForSeconds(stopScale*stopDuration);
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
