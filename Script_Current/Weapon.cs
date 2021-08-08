using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Weapon : MonoBehaviour
{
    protected Player player;
    [System.Serializable]
    public class TargetTransform
    {
        public Transform parent;
        
        public Vector3 targetPos = Vector3.one;
        public Vector3 targetRot = Vector3.zero;
        public Vector3 targetScale = Vector3.one;
        public float duration=0.2f;
        [HideInInspector]
        public int hash_Pos,hash_Rot,hash_Scale;

        public TargetTransform()
        {
            hash_Pos = Animator.StringToHash(GetHashCode().ToString() + "Pos");
            hash_Rot = Animator.StringToHash(GetHashCode().ToString() + "Rot");
            hash_Scale = Animator.StringToHash(GetHashCode().ToString() + "Scale");
        }
    }

    public TargetTransform sheathTransform;
    public TargetTransform unsheathTransform;
    
    public virtual void Move(TargetTransform tt)
    {
        DOTween.Kill(tt.hash_Pos);
        DOTween.Kill(tt.hash_Rot);
        DOTween.Kill(tt.hash_Scale);
        
        transform.parent = tt.parent;

        transform.DOLocalMove(tt.targetPos, tt.duration).SetId(tt.hash_Pos);
        transform.DOLocalRotateQuaternion(Quaternion.Euler(tt.targetRot), tt.duration).SetId(tt.hash_Rot);
        transform.DOScale(tt.targetScale, tt.duration).SetId(tt.hash_Scale);
    }

    public virtual void Start()
    {
        player = GetComponent<Player>();
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
