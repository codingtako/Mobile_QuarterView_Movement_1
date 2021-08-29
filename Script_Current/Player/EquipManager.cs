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
    //WeaponUsage를 체크해서 스테이트를 바꾼다.
    public enum JS_Type{Normal=0,Strong=1}

    public JS_Type js_Type;
    public enum WeaponUsage {None=0,Main=1,Sub=2,Both=3}
    public WeaponUsage weaponUsage= WeaponUsage.None;
    #region 컴포넌트
    private FullBodyBipedIK ik;
    private Animator anim;
    #endregion
    #region 무기 모음
    public Weapon_Main testMain;
    public Weapon_Sub testSub;
    [HideInInspector]
    public Weapon_Main weaponMain;
    [HideInInspector]
    public Weapon_Sub weaponSub;
    #endregion

    #region IK,Layer관련 변수
    private int str_MoveIK_LeftHand;
    private int str_MoveIK_RightHand;
    private int str_MoveLayer_LeftHand;
    private int str_MoveLayer_RightHand;
    private string str_Unsheath;
    private string str_Unsheath_Ready;
    private string str_Sheath;
    private string str_Sheath_Ready;
    private string str_Unsheath_Left;
    private string str_Unsheath_Ready_Left;
    private string str_Sheath_Left;
    private string str_Sheath_Ready_Left;
    private string str_Unsheath_Right;
    private string str_Unsheath_Ready_Right;
    private string str_Sheath_Right;
    private string str_Sheath_Ready_Right;
    private string str_None;
    
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
        //-----------------------------------
        private AnimationClip L_UnsheathReady;
        private AnimationClip L_Unsheath;
        private AnimationClip L_SheathReady;
        private AnimationClip L_Sheath;
        //-----------------------------------
        private AnimationClip R_UnsheathReady;
        private AnimationClip R_Unsheath;
        private AnimationClip R_SheathReady;
        private AnimationClip R_Sheath;
        //-----------------------------------
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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) weaponUsage = (WeaponUsage) (((int) weaponUsage + 1) % 4);
        if (Input.GetKeyDown(KeyCode.O)) equipState_Main = (EquipState) (((int) equipState_Main + 1) % 6);
        
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
        float testF=anim.GetLayerWeight(2);
        
        DOTween.To(()=> testF, x=> testF = x, targetRatio, duration)
            .SetId(str_MoveLayer_LeftHand).OnUpdate(() =>
            {
                anim.SetLayerWeight(2,testF);
            });


    }
    #endregion

    #region Animator관련 함수

    public AnimatorOverrideController UpdatedAnimatorOverrideController()
    {
        RuntimeAnimatorController myController = anim.runtimeAnimatorController;

        AnimatorOverrideController myOverrideController = myController as AnimatorOverrideController;
        if (myOverrideController != null) myController = myOverrideController.runtimeAnimatorController;

        AnimatorOverrideController animatorOverride = new AnimatorOverrideController();
        animatorOverride.runtimeAnimatorController = myController;

        #region 장착 모션

        if (weaponMain != null)
        {
            switch (weaponMain.equipType)
            {
                case Weapon_Main.EquipType.oneHand_Sword:
                    animatorOverride[str_Sheath_Ready_Right] = oneHand_Sheath_Ready;
                    animatorOverride[str_Sheath_Right] = oneHand_Sheath;
                    animatorOverride[str_Unsheath_Ready_Right] = oneHand_Unsheath_Ready;
                    animatorOverride[str_Unsheath_Right] = oneHand_Unsheath;
                    R_SheathReady = oneHand_Sheath_Ready;
                    R_Sheath = oneHand_Sheath;
                    R_UnsheathReady = oneHand_Unsheath_Ready;
                    R_Unsheath = oneHand_Unsheath;
                    break;
                case Weapon_Main.EquipType.twoHand_Sword:
                    animatorOverride[str_Sheath_Ready_Right] = twoHand_Sheath_Ready;
                    animatorOverride[str_Sheath_Right] = twoHand_Sheath;
                    animatorOverride[str_Unsheath_Ready_Right] = twoHand_Unsheath_Ready;
                    animatorOverride[str_Unsheath_Right] = twoHand_Unsheath;
                    R_SheathReady = twoHand_Sheath_Ready;
                    R_Sheath = twoHand_Sheath;
                    R_UnsheathReady = twoHand_Unsheath_Ready;
                    R_Unsheath = twoHand_Unsheath;
                    break;
            }
            anim.SetFloat("R_UnsheathReady",weaponMain.UnsheathReadySpeed);
            anim.SetFloat("R_Unsheath",weaponMain.UnsheathSpeed);
            anim.SetFloat("R_SheathReady",weaponMain.SheathReadySpeed);
            anim.SetFloat("R_Sheath",weaponMain.SheathSpeed);
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
                    L_SheathReady = shield_Sheath_Ready;
                    L_Sheath = shield_Sheath;
                    L_UnsheathReady = shield_Unsheath_Ready;
                    L_Unsheath = shield_Unsheath;
                    break;
            }
            anim.SetFloat("L_UnsheathReady",weaponSub.UnsheathReadySpeed);
            anim.SetFloat("L_Unsheath",weaponSub.UnsheathSpeed);
            anim.SetFloat("L_SheathReady",weaponSub.SheathReadySpeed);
            anim.SetFloat("L_Sheath",weaponSub.SheathSpeed);
        }
        
        
        #endregion
        
        
        
        return animatorOverride;
    }

    #endregion
    
     #region SuperState
        #region Left
            #region FlowState
            public void Unsheath_Ready_L_Enter()
            {
                anim.CrossFadeInFixedTime("Unsheath_Ready",
                    weaponSub.unsheathReadyTransform.fadeDuration,2);
                MoveIK_LeftHand(weaponSub.unsheathReadyTransform.ikActicateWeight,weaponSub.unsheathReadyTransform.ikDuration);
                MoveLayer_LeftHand(1,weaponSub.unsheathReadyTransform.layerDuration);
                weaponSub.Move(weaponSub.unsheathReadyTransform,weaponSub.unsheathReadyTransform.moveDelay);
            }
            public void Unsheath_L_Enter()
            {
                anim.CrossFadeInFixedTime("Unsheath",
                    weaponSub.unsheathTransform.fadeDuration,2);
                MoveIK_LeftHand(0.01f,weaponSub.unsheathTransform.ikDuration);
                MoveLayer_LeftHand(0.01f,weaponSub.unsheathTransform.layerDuration);
                weaponSub.Move(weaponSub.unsheathTransform,weaponSub.unsheathTransform.moveDelay);
            }
            public void Sheath_Ready_L_Enter()
            {
                anim.CrossFadeInFixedTime("Sheath_Ready",
                    weaponSub.sheathReadyTransform.fadeDuration,2);
                MoveIK_LeftHand( weaponSub.sheathReadyTransform.ikActicateWeight,weaponSub.sheathReadyTransform.ikDuration);
                MoveLayer_LeftHand(1,weaponSub.sheathReadyTransform.layerDuration);
                weaponSub.Move(weaponSub.sheathReadyTransform,weaponSub.sheathReadyTransform.moveDelay);
            }
            public void Sheath_L_Enter()
            {
                anim.CrossFadeInFixedTime("Sheath",
                    weaponSub.sheathTransform.fadeDuration,2);
                MoveIK_LeftHand(0.01f,weaponSub.sheathTransform.ikDuration);
                MoveLayer_LeftHand(0.01f,weaponSub.sheathTransform.layerDuration);
                weaponSub.Move(weaponSub.sheathTransform,weaponSub.sheathTransform.moveDelay);
            }
            public void Unsheath_Ready_L()
            {
                
            }
            public void Unsheath_L()
            {
                
            }
            public void Sheath_Ready_L()
            {
                
            }
            public void Sheath_L()
            {
                
            }

            public void None_Enter_L()
            {
                
            }

            public void None_L()
            {
                
            }
            #endregion
            #region Transition
            //최초로 발도 시작 할 때
            public bool L_None2UnsheathReady()
            {
                //왼손을 안쓰는 경우 false
                if (weaponUsage != WeaponUsage.Both && weaponUsage != WeaponUsage.Sub) return false;
                //조이스틱을 누르고 있는 경우 true
                if (InputManager.IsPressingJS())
                {
                    return true;
                }
                //선입력 있는 경우 true + jsType 설정
                else if(InputManager.PreInput_JS_Normal())
                {
                    js_Type = JS_Type.Normal;
                    return true;
                }
                else if(InputManager.PreInput_JS_Strong())
                {
                    js_Type = JS_Type.Strong;
                    return true;
                }
                return false;
            }
            //발도
            public bool L_UnsheathReady2Unsheath()
            {
                if (player.playerState == Player.PlayerState.Attack
                    && weaponUsage != WeaponUsage.None && weaponUsage != WeaponUsage.Main)
                {
                    return true;
                }
                return false;
            }
            //발도 캔슬 시
            public bool L_UnsheathReady2Sheath()
            {
                switch (js_Type)
                {
                    case JS_Type.Normal:
                        if (!InputManager.JS_MainWaepon.isPressing 
                            && InputManager.JS_MainWaepon.canceled) return true;
                        break;
                    case JS_Type.Strong:
                        if (!InputManager.JS_Skill.isPressing 
                            && InputManager.JS_Skill.canceled) return true;
                        break;
                }
                return false;
            }
            //납도준비 애니메이션 플레이 후
            public bool L_SheathReady2Sheath()
            {
                if (anim.GetCurrentAnimatorStateInfo(2).IsName("Sheath_Ready")
                    && !anim.IsInTransition(2)
                    && anim.GetCurrentAnimatorStateInfo(2).normalizedTime > 0.9f) return true;
                return false;
            }
            //납도 애니메이션 플레이 후
            public bool L_Sheath2None()
            {  
                if (anim.GetCurrentAnimatorStateInfo(2).IsName("Sheath")
                   && !anim.IsInTransition(2)
                   && anim.GetCurrentAnimatorStateInfo(2).normalizedTime > 0.9f) return true;
                return false;
            }
            //발도상태에서 납도 할 때
            public bool L_Unsheath2SheathReady()
            {
                if (player.playerState != Player.PlayerState.Attack) return true;
                return false;
            }
            #endregion
        #endregion
        #region Right
            #region FlowState
            public void Unsheath_Ready_R_Enter()
            {
                anim.CrossFadeInFixedTime("Unsheath_Ready",
                    weaponMain.unsheathReadyTransform.fadeDuration,3);
                MoveIK_RightHand(weaponMain.unsheathReadyTransform.ikActicateWeight,weaponMain.unsheathReadyTransform.ikDuration);
                MoveLayer_RightHand(1,weaponMain.unsheathReadyTransform.layerDuration);
                weaponMain.Move(weaponMain.unsheathReadyTransform,weaponMain.unsheathReadyTransform.moveDelay);
            }
            public void Unsheath_R_Enter()
            {
                anim.CrossFadeInFixedTime("Unsheath",
                    weaponMain.unsheathTransform.fadeDuration,3);
                MoveIK_RightHand(0.01f,weaponMain.unsheathTransform.ikDuration);
                MoveLayer_RightHand(0.01f,weaponMain.unsheathTransform.layerDuration);
                weaponMain.Move(weaponMain.unsheathTransform,weaponMain.unsheathTransform.moveDelay);
            }
            public void Sheath_Ready_R_Enter()
            {
                anim.CrossFadeInFixedTime("Sheath_Ready",
                    weaponMain.sheathReadyTransform.fadeDuration,3);
                MoveIK_RightHand(weaponMain.sheathReadyTransform.ikActicateWeight,weaponMain.sheathReadyTransform.ikDuration);
                MoveLayer_RightHand(1,weaponMain.sheathReadyTransform.layerDuration);
                weaponMain.Move(weaponMain.sheathReadyTransform,weaponMain.sheathReadyTransform.moveDelay);
            }
            public void Sheath_R_Enter()
            {
                anim.CrossFadeInFixedTime("Sheath",
                    weaponMain.sheathTransform.fadeDuration,3);
                MoveIK_RightHand(0.01f,weaponMain.sheathTransform.ikDuration);
                MoveLayer_RightHand(0.01f,weaponMain.sheathTransform.layerDuration);
                weaponMain.Move(weaponMain.sheathTransform,weaponMain.sheathTransform.moveDelay);
            }
            public void Unsheath_Ready_R()
            {
                
            }
            public void Unsheath_R()
            {
                
            }
            public void Sheath_Ready_R()
            {
                
            }
            public void Sheath_R()
            {
                
            }
            
            public void None_Enter_R()
            {
                
            }

            public void None_R()
            {
                
            }
            #endregion
            #region Transition
            //최초로 발도 시작 할 때
            public bool R_None2UnsheathReady()
            {
                //오른손을 안쓰는 경우 false
                if (weaponUsage != WeaponUsage.Both && weaponUsage != WeaponUsage.Main) return false;
                //조이스틱을 누르고 있는 경우 true
                if (InputManager.IsPressingJS())
                {
                    return true;
                }
                //선입력 있는 경우 true + jsType 설정
                else if(InputManager.PreInput_JS_Normal())
                {
                    js_Type = JS_Type.Normal;
                    return true;
                }
                else if(InputManager.PreInput_JS_Strong())
                {
                    js_Type = JS_Type.Strong;
                    return true;
                }
                return false;
            }
            //발도
            public bool R_UnsheathReady2Unsheath()
            {
                if (player.playerState == Player.PlayerState.Attack
                    && weaponUsage != WeaponUsage.None && weaponUsage != WeaponUsage.Sub)
                {
                    return true;
                }
                return false;
            }
            //발도 캔슬 시
            public bool R_UnsheathReady2Sheath()
            {
                switch (js_Type)
                {
                    case JS_Type.Normal:
                        if (!InputManager.JS_MainWaepon.isPressing 
                            && InputManager.JS_MainWaepon.canceled) return true;
                        break;
                    case JS_Type.Strong:
                        if (!InputManager.JS_Skill.isPressing 
                            && InputManager.JS_Skill.canceled) return true;
                        break;
                }
                return false;
            }
            //납도준비 애니메이션 플레이 후
            public bool R_SheathReady2Sheath()
            {
                if (anim.GetCurrentAnimatorStateInfo(3).IsName("Sheath_Ready")
                    && !anim.IsInTransition(3)
                    && anim.GetCurrentAnimatorStateInfo(3).normalizedTime > 0.9f) return true;
                return false;
            }
            //납도 애니메이션 플레이 후
            public bool R_Sheath2None()
            {
                if (anim.GetCurrentAnimatorStateInfo(3).IsName("Sheath")
                    && !anim.IsInTransition(3)
                    && anim.GetCurrentAnimatorStateInfo(3).normalizedTime > 0.9f) return true;
                return false;
            }
            //발도상태에서 납도 할 때
            public bool R_Unsheath2SheathReady()
            {
                if (player.playerState != Player.PlayerState.Attack) return true;
                return false;
            }
            #endregion
        #endregion

        public void Attack()
        {
            
        }
    #endregion

    public bool IsAttackReady()
    {
        if (weaponUsage == WeaponUsage.None) return false;
        //캔슬된 경우 false
        if (InputManager.JS_MainWaepon.canceled || InputManager.JS_Skill.canceled) return false;
        //print("1");
        //왼손 애니메이션 플레이 안된 경우 false
        if (weaponUsage != WeaponUsage.None && weaponUsage != WeaponUsage.Main)
        {
            bool checkState = anim.GetCurrentAnimatorStateInfo(2).IsName("Unsheath_Ready");
            bool checkFinish = anim.GetCurrentAnimatorStateInfo(2).normalizedTime> 0.9f;
            if (!checkState || !checkFinish || anim.IsInTransition(2)) return false;
        }
        //print("2");
        //오른손 애니메이션 플레이 안된 경우 false
        if (weaponUsage != WeaponUsage.None && weaponUsage != WeaponUsage.Sub)
        {
            bool checkState = anim.GetCurrentAnimatorStateInfo(3).IsName("Unsheath_Ready");
            bool checkFinish = anim.GetCurrentAnimatorStateInfo(3).normalizedTime> 0.9f;
            if (!checkState || !checkFinish || anim.IsInTransition(3)) return false;
        }
        //print("3");
        //선입력 있는 경우 true
        if (InputManager.Have_JS_PreInput())
        {
            return true;
        }
        
        return false;
    }
//----------------------------------------------------------------------------------------------------------------------    
}
