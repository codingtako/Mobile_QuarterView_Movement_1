using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using TrailsFX;

public class Weapon : MonoBehaviour
{
    public enum WeaponTag { One_Hand=1<<0,Two_Hand=1<<1,Blade=1<<2,Axe=1<<3,Blunt=1<<4 }
    protected Player player;
    [BoxGroup("EquipSpeed")] public float UnsheathReadySpeed = 1, UnsheathSpeed = 1, SheathReadySpeed = 1, SheathSpeed = 1;

    [HideInInspector] public TrailEffect trail;
    [System.Serializable]
    public class TargetTransform
    {
        [HideInInspector]
        public Transform parent;

        
        public Vector3 targetPos = Vector3.one;
        public Vector3 targetRot = Vector3.zero;
        public Vector3 targetScale = Vector3.one;
        public float fadeDuration = 0.1f;
        public float ikDuration = 0.3f;
        public float ikActicateWeight = 1.0f;
        public float layerDuration = 0.2f;
        public float moveDuration = 0.2f;
        public float moveDelay = 0.0f;
        [HideInInspector]
        public int hash_Pos,hash_Rot,hash_Scale;

        public TargetTransform()
        {
            hash_Pos = Animator.StringToHash(GetHashCode().ToString() + "Pos");
            hash_Rot = Animator.StringToHash(GetHashCode().ToString() + "Rot");
            hash_Scale = Animator.StringToHash(GetHashCode().ToString() + "Scale");
        }
    }
    
    public TargetTransform sheathReadyTransform;
    public TargetTransform sheathTransform;
    public TargetTransform unsheathReadyTransform;
    public TargetTransform unsheathTransform;
    [HideInInspector]
    public Transform holdTransform;
    public virtual void Move(TargetTransform tt,float delay=0.0f)
    {
        DOTween.Kill(tt.hash_Pos);
        DOTween.Kill(tt.hash_Rot);
        DOTween.Kill(tt.hash_Scale);
        
        transform.parent = tt.parent;

        transform.DOLocalMove(tt.targetPos, tt.moveDuration).SetId(tt.hash_Pos).SetDelay(delay);
        transform.DOLocalRotateQuaternion(Quaternion.Euler(tt.targetRot), tt.moveDuration).SetId(tt.hash_Rot).SetDelay(delay);
        transform.DOScale(tt.targetScale, tt.moveDuration).SetId(tt.hash_Scale).SetDelay(delay);
    }

    public virtual void StartSetting(EquipManager e)
    {
        player = GameObject.FindObjectOfType<Player>();
        trail = GetComponent<TrailEffect>();
        holdTransform = transform.Find("Hold");
    }
    /*
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Move(sheathTransform);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Move(unsheathTransform);
        }
    }
    */
}
