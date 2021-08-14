using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RootMotion.FinalIK;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EquipManager : MonoBehaviour
{
//----------------------------------------------------------------------------------------------------------------------    
    private Player player;
    #region 무기 모음
    public Weapon_Main testMain;
    public Weapon_Sub testSub;
    [LabelText("생성된 메인 무기")]
    [ReadOnly]
    public Weapon_Main weaponMain;
    [LabelText("생성된 서브 무기")]
    [ReadOnly]
    public Weapon_Sub weaponSub;
    #endregion

    #region IK,Layer관련 변수

    public FullBodyBipedIK ik;
    public int str_MoveIK_LeftHand;
    public int str_MoveIK_RightHand;
    public int str_MoveLayer_LeftHand;
    public int str_MoveLayer_RightHand;
    public string str_Unsheath;
    public string str_Unsheath_Ready;
    public string str_Sheath;
    public string str_Sheath_Ready;
    public string str_Unsheath_Left;
    public string str_Unsheath_Ready_Left;
    public string str_Sheath_Left;
    public string str_Sheath_Ready_Left;
    public string str_Unsheath_Right;
    public string str_Unsheath_Ready_Right;
    public string str_Sheath_Right;
    public string str_Sheath_Ready_Right;
    public string str_None;
    public Animator anim;
    #endregion

    #region 착용 애니메이션
        #region 오른손
            #region 한손무기
            [FoldoutGroup("오른손")]
            [FoldoutGroup("오른손/한손무기")] 
            public AnimationClip oneHand_Unsheath_Ready,oneHand_Unsheath,oneHand_Sheath_Ready,oneHand_Sheath;
            #endregion
            #region 양손무기
            [FoldoutGroup("오른손")]
            [FoldoutGroup("오른손/양손무기")] 
            public AnimationClip twoHand_Unsheath_Ready,twoHand_Unsheath,twoHand_Sheath_Ready,twoHand_Sheath;
            #endregion
    #endregion
        #region 왼손
            #region 방패
            [FoldoutGroup("왼손")]
            [FoldoutGroup("왼손/방패")] 
            public AnimationClip shield_Unsheath_Ready,shield_Unsheath,shield_Sheath_Ready,shield_Sheath;
            #endregion
        #endregion
    #endregion
    public enum EquipState
    {
        None =0,Unsheath_Ready =1,Unsheath =2, Sheath_Ready=3,Sheath=4,Unarm =5
    }
    public EquipState equipState_Main;
    public EquipState equipState_Sub; 
//----------------------------------------------------------------------------------------------------------------------
    public void Start()
    {
        anim=GetComponent<Animator>();
        player = GetComponent<Player>();
        
        ik = GetComponent<FullBodyBipedIK>();
        str_MoveIK_LeftHand = Animator.StringToHash(GetHashCode().ToString()+"str_MoveIK_LeftHand");
        str_MoveIK_RightHand = Animator.StringToHash(GetHashCode().ToString()+"str_MoveIK_RightHand");
        str_MoveLayer_LeftHand = Animator.StringToHash(GetHashCode().ToString()+"str_MoveLayer_LeftHand");
        str_MoveLayer_RightHand = Animator.StringToHash(GetHashCode().ToString()+"str_MoveLayer_RightHand");
        str_Sheath = "Sheath";
        str_Sheath_Ready ="Sheath_Ready";
        str_Unsheath = "Unsheath";
        str_Unsheath_Ready = "Unsheath_Ready";
        str_Sheath_Left = "Sheath_Left";
        str_Sheath_Ready_Left = "Sheath_Ready_Left";
        str_Unsheath_Left = "Unsheath_Left";
        str_Unsheath_Ready_Left ="Unsheath_Ready_Left";
        str_Sheath_Right = "Sheath_Right";
        str_Sheath_Ready_Right = "Sheath_Ready_Right";
        str_Unsheath_Right = "Unsheath_Right";
        str_Unsheath_Ready_Right = "Unsheath_Ready_Right";
        
        Create_Weapon(testMain,testSub);
    }
//----------------------------------------------------------------------------------------------------------------------
    public void Create_Weapon(Weapon_Main main,Weapon_Sub sub)
    {
        weaponMain = Instantiate(main);
        weaponMain.StartSetting(this);
        weaponMain.Move(weaponMain.sheathTransform);
        ik.solver.rightHandEffector.target = weaponMain.holdTransform;
        if (sub != null)
        {
            weaponSub = Instantiate(sub);
            weaponSub.StartSetting(this);
            weaponSub.Move(weaponSub.sheathTransform);
            ik.solver.leftHandEffector.target = weaponSub.holdTransform;
        }
        UpdateAnimator();
    }
    public void Remove_Weapon()
    {
        if(weaponMain!=null) Destroy(weaponMain);
        if(weaponSub!=null) Destroy(weaponSub);

        weaponMain = null;
        weaponSub = null;
    }
    #region 상태 설정 함수
    [Button]
    public void Set_EquipState_Main(EquipState state)
    {
        bool moveMain = true;
        float layerDuration=0.2f,ikDuration=0.3f, fadeDuration = 0.1f,delay = 0.2f;
        #region 각 무기들을 보고, 메인,서브 움직일지 확인

        

        #endregion
        equipState_Main = state;
        switch (state)
        {
            case EquipState.None:
                MoveLayer_RightHand(0.01f,layerDuration);
                MoveIK_RightHand(0.01f,ikDuration);
                anim.CrossFadeInFixedTime(str_None,fadeDuration,3);
                
                break;
            case EquipState.Unsheath_Ready:
                if (moveMain)
                {
                    MoveLayer_RightHand(1,layerDuration);
                    MoveIK_RightHand(1,ikDuration);
                    anim.CrossFadeInFixedTime(str_Unsheath_Ready,fadeDuration,3);
                }
                break;
            case EquipState.Unsheath:
                if (moveMain)
                {
                    weaponMain.Move(weaponMain.unsheathTransform);
                    MoveLayer_RightHand(0.01f,layerDuration);
                    MoveIK_RightHand(0.01f,ikDuration);
                    anim.CrossFadeInFixedTime(str_Unsheath,fadeDuration,3);
                }
                break;
            case EquipState.Sheath_Ready:
                if (moveMain)
                {
                    MoveLayer_RightHand(0.01f,layerDuration);
                    MoveIK_RightHand(0.01f,ikDuration);
                    anim.CrossFadeInFixedTime(str_Sheath_Ready,fadeDuration,3);
                }
                break;
            case EquipState.Sheath:
                if (moveMain)
                {
                    weaponMain.Move(weaponMain.sheathTransform,delay);
                    MoveLayer_RightHand(0.01f,layerDuration);
                    MoveIK_RightHand(0.01f,ikDuration);
                    anim.CrossFadeInFixedTime(str_Sheath,fadeDuration,3);
                }
                break;
            case EquipState.Unarm:
                break;
        }
        print("Main");
    }
    [Button]
    public void Set_EquipState_Sub(EquipState state)
    {
        bool moveSub = true;
        float layerDuration=0.2f,ikDuration=0.3f, fadeDuration = 0.1f;
        #region 각 무기들을 보고, 메인,서브 움직일지 확인

        

        #endregion
        equipState_Sub = state;
        switch (state)
        {
            case EquipState.None:
                MoveLayer_LeftHand(0.01f,layerDuration);
                //MoveIK_LeftHand(0.01f,ikDuration);
                anim.CrossFadeInFixedTime(str_None,fadeDuration,2);
                break;
            case EquipState.Unsheath_Ready:
                if (moveSub)
                {
                    MoveLayer_LeftHand(1,layerDuration);
                    MoveIK_LeftHand(1,ikDuration);
                    anim.CrossFadeInFixedTime(str_Unsheath_Ready,fadeDuration,2);
                }
                break;
            case EquipState.Unsheath:
                if (moveSub)
                {
                    weaponSub.Move(weaponSub.unsheathTransform);
                    MoveLayer_LeftHand(0.01f,layerDuration);
                    MoveIK_LeftHand(0.01f,ikDuration);
                    anim.CrossFadeInFixedTime(str_Unsheath,fadeDuration,2);
                }
                break;
            case EquipState.Sheath_Ready:
                if (moveSub)
                {
                    MoveLayer_LeftHand(0.01f,layerDuration);
                    MoveIK_LeftHand(0.01f,ikDuration);
                    anim.CrossFadeInFixedTime(str_Sheath_Ready,fadeDuration,2);
                }
                break;
            case EquipState.Sheath:
                if (moveSub)
                {
                    weaponSub.Move(weaponSub.sheathTransform);
                    MoveLayer_LeftHand(0.01f,layerDuration);
                    MoveIK_LeftHand(0.01f,ikDuration);
                    anim.CrossFadeInFixedTime(str_Sheath,fadeDuration,2);
                }
                break;
            case EquipState.Unarm:
                break;
        }
        print("Sub");
    }
    [Button]
    public void Change()
    {
        if (weaponMain.transform.parent == weaponMain.unsheathTransform.parent)
        {
            weaponMain.Move(weaponMain.sheathTransform);
            weaponSub.Move(weaponSub.sheathTransform);
        }
        else
        {
            weaponMain.Move(weaponMain.unsheathTransform);
            weaponSub.Move(weaponSub.unsheathTransform);
        }
    }
    #endregion

    #region IK,Layer설정 함수

    public void MoveIK_LeftHand(float targetRatio,float duration)
    {
        DOTween.Kill(str_MoveIK_LeftHand);
        float testF=ik.solver.leftHandEffector.positionWeight;
        
        DOTween.To(()=> testF, x=> testF = x, targetRatio, duration)
            .SetId(str_MoveIK_LeftHand).OnUpdate(() =>
            {
                ik.solver.leftHandEffector.positionWeight = testF;
                ik.solver.leftHandEffector.rotationWeight = testF;
            });


    }
    public void MoveIK_RightHand(float targetRatio,float duration)
    {
        DOTween.Kill(str_MoveIK_RightHand);
        float testF=ik.solver.rightHandEffector.positionWeight;
        
        DOTween.To(()=> testF, x=> testF = x, targetRatio, duration)
            .SetId(str_MoveIK_RightHand).OnUpdate(() =>
            {
                ik.solver.rightHandEffector.positionWeight = testF;
                ik.solver.rightHandEffector.rotationWeight = testF;
            });


    }
    public void MoveLayer_RightHand(float targetRatio,float duration)
    {
        DOTween.Kill(str_MoveLayer_RightHand);
        float testF=anim.GetLayerWeight(3);
        
        DOTween.To(()=> testF, x=> testF = x, targetRatio, duration)
            .SetId(str_MoveLayer_RightHand).OnUpdate(() =>
            {
                anim.SetLayerWeight(3,testF);
            });


    }
    public void MoveLayer_LeftHand(float targetRatio,float duration)
    {
        DOTween.Kill(str_MoveLayer_LeftHand);
        float testF=anim.GetLayerWeight(3);
        
        DOTween.To(()=> testF, x=> testF = x, targetRatio, duration)
            .SetId(str_MoveLayer_RightHand).OnUpdate(() =>
            {
                anim.SetLayerWeight(3,testF);
            });


    }
    #endregion

    #region Animator관련 함수

    public void UpdateAnimator()
    {
        RuntimeAnimatorController myController = anim.runtimeAnimatorController;

        AnimatorOverrideController myOverrideController = myController as AnimatorOverrideController;
        if (myOverrideController != null) myController = myOverrideController.runtimeAnimatorController;

        AnimatorOverrideController animatorOverride = new AnimatorOverrideController();
        animatorOverride.runtimeAnimatorController = myController;

        #region 장착 모션
        switch (weaponMain.equipType)
        {
            case Weapon_Main.EquipType.oneHand_Sword:
                animatorOverride[str_Sheath_Ready_Right] = oneHand_Sheath_Ready;
                animatorOverride[str_Sheath_Right] = oneHand_Sheath;
                animatorOverride[str_Unsheath_Ready_Right] = oneHand_Unsheath_Ready;
                animatorOverride[str_Unsheath_Right] = oneHand_Unsheath;
                break;
            case Weapon_Main.EquipType.twoHand_Sword:
                animatorOverride[str_Sheath_Ready_Right] = twoHand_Sheath_Ready;
                animatorOverride[str_Sheath_Right] = twoHand_Sheath;
                animatorOverride[str_Unsheath_Ready_Right] = twoHand_Unsheath_Ready;
                animatorOverride[str_Unsheath_Right] = twoHand_Unsheath;
                break;
        }
        if (weaponSub != null)
        {
            switch (weaponSub.equipType)
            {
                case Weapon_Sub.EquipType.bow:
                    break;
                case Weapon_Sub.EquipType.dagger:
                    break;
                case Weapon_Sub.EquipType.shield:
                    animatorOverride[str_Sheath_Ready_Left] = shield_Sheath_Ready;
                    animatorOverride[str_Sheath_Left] = shield_Sheath;
                    animatorOverride[str_Unsheath_Ready_Left] = shield_Unsheath_Ready;
                    animatorOverride[str_Unsheath_Left] = shield_Unsheath;
                    break;
            }
        }
        #endregion
        
        
        
        anim.runtimeAnimatorController = animatorOverride;
    }

    #endregion
//----------------------------------------------------------------------------------------------------------------------    
}
