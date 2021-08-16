using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Joystick : MonoBehaviour,
                            IPointerDownHandler,
                            IPointerUpHandler,
                            IDragHandler
{
    public Vector3 input;
    public Vector3 lastInput;
    public bool canceled;
    [HideInInspector]
    public UnityEvent onPressed;
    [HideInInspector]
    public UnityEvent onDragBegin;
    [HideInInspector]
    public UnityEvent onReleased;
    [HideInInspector]
    public UnityEvent onCanceled;
    public float stickSpeed=30;
    [Range(0,1)]
    public float stickMove_MaxRatio;
    [Range(0,1)]
    public float stickMove_MinRatio;

    public string horizontalAxis;
    public string verticalAxis;

    public Transform img_Button;
    public Transform img_BG;
    private Vector3 startPos;
    private float maxDist;
    private float minDist;
    private Vector3 targetVec;
    private bool lastPressed;
    public bool touchControl = true;
    public bool checkMinDist = true;
    public bool isPressing = false;
    
    public virtual void Start()
    {
        startPos = img_BG.transform.position;
        maxDist = img_Button.GetComponent<Image>().rectTransform.sizeDelta.x * stickMove_MaxRatio;
        minDist = img_Button.GetComponent<Image>().rectTransform.sizeDelta.x * stickMove_MinRatio;
    }

    public void Update()
    {
        if (touchControl) return;
        bool pressed = Input.GetAxisRaw(horizontalAxis)!=0 || Input.GetAxisRaw(verticalAxis)!=0;
        if (pressed)
        {
            
            Vector3 targetInput = new Vector3(Input.GetAxisRaw(horizontalAxis), Input.GetAxisRaw(verticalAxis), 0).normalized;
            img_Button.position = Vector3.Lerp(img_Button.position,img_BG.position+targetInput*maxDist,stickSpeed*Time.deltaTime);
            input = new Vector3(Input.GetAxisRaw(horizontalAxis),0,Input.GetAxisRaw(verticalAxis)).normalized;
            lastInput = input;
            if(!lastPressed) onDragBegin.Invoke();
        }
        else
        {
            
            img_Button.position = Vector3.Lerp(img_Button.position,img_BG.position,stickSpeed*Time.deltaTime);

            input = Vector3.zero;
            if (lastPressed)onReleased.Invoke();
        }

        isPressing = pressed;
        lastPressed = pressed;
    }

    public virtual void OnPointerDown(PointerEventData data)
    {
        if (!touchControl) return;
        isPressing = true;
        canceled = false;
        img_BG.position = data.position;
        img_Button.position = data.position;
        
        input = Vector3.zero;
        
    }
    public virtual void OnDrag(PointerEventData data)
    {
        if (!touchControl) return;
        bool dragBegin = false;
        if (img_BG.position == img_Button.position) dragBegin = true;
        targetVec = data.position;
        float dist = Vector3.Distance(targetVec, img_BG.position); 
        //멀리 이동 한 경우
        if (dist > maxDist)
        {
            img_Button.position = data.position;
            Vector3 moveDir = (img_Button.position - img_BG.position).normalized;
            img_BG.position += moveDir * (dist - maxDist);

            Vector3 inputDist = new Vector3( data.position.x, data.position.y,0) - img_BG.position;
            input = new Vector3(inputDist.x,0,inputDist.y).normalized;
            lastInput = input;
            canceled = false;
        }
        //적당히 이동한 경우
        else if (dist > minDist||!checkMinDist)
        {
            img_Button.position = data.position;
            
            Vector3 inputDist = new Vector3( data.position.x, data.position.y,0) - img_BG.position;
            input = new Vector3(inputDist.x,0,inputDist.y).normalized;
            lastInput = input;
            canceled = false;
        }
        //너무 조금 이동한 경우
        else
        {
            if(Vector3.Distance(img_Button.position,img_BG.position)>minDist) canceled = true;
            img_Button.position = img_BG.position;
            input = Vector3.zero;
        }
        if(dragBegin)onDragBegin.Invoke();
    }
    public virtual void OnPointerUp(PointerEventData data)
    {
        if (!touchControl) return;
        isPressing = false;
        img_BG.position = startPos;
        img_Button.position = startPos;

        input = Vector3.zero;
        onReleased.Invoke();
    }

}
