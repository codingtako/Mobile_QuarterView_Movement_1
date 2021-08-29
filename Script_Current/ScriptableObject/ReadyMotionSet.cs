using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New ReadyMotionSet", menuName = "ReadyMotionSet")]

public class ReadyMotionSet : ScriptableObject

{
    public enum MotionType { LS_LF=0, LS_RF=1, RS_LF=2, RS_RF=3}
    //-------------------------------
    public AnimationClip LS_LF__LS;
    public AnimationClip LS_LF__RS;
    public float LS_LF__LS_Speed=1;
    public float LS_LF__RS_Speed=1;
    //--------------------------------
    public AnimationClip LS_RF__LS;
    public AnimationClip LS_RF__RS;
    public float LS_RF__LS_Speed=1;
    public float LS_RF__RS_Speed=1;
    //--------------------------------
    public AnimationClip RS_LF__LS;
    public AnimationClip RS_LF__RS;
    public float RS_LF__LS_Speed=1;
    public float RS_LF__RS_Speed=1;
    //--------------------------------
    public AnimationClip RS_RF__LS;
    public AnimationClip RS_RF__RS;
    public float RS_RF__LS_Speed=1;
    public float RS_RF__RS_Speed=1;
    //-------------------------------
    public AnimationClip Get_ReadyClip(MotionType start, MotionType end )
    {
        switch (start)
        {
            case MotionType.LS_LF:
                switch (end)
                {
                    case MotionType.LS_LF:
                        return LS_LF__LS;
                        break;
                    case MotionType.LS_RF:
                        return LS_LF__LS;
                        break;
                    case MotionType.RS_LF:
                        return LS_LF__RS;
                        break;
                    case MotionType.RS_RF:
                        return LS_LF__RS;
                        break;
                }
                break;
            case MotionType.LS_RF:
                switch (end)
                {
                    case MotionType.LS_LF:
                        return LS_RF__LS;
                        break;
                    case MotionType.LS_RF:
                        return LS_RF__LS;
                        break;
                    case MotionType.RS_LF:
                        return LS_RF__RS;
                        break;
                    case MotionType.RS_RF:
                        return LS_RF__RS;
                        break;
                }
                break;
            case MotionType.RS_LF:
                switch (end)
                {
                    case MotionType.LS_LF:
                        return RS_LF__LS;
                        break;
                    case MotionType.LS_RF:
                        return RS_LF__LS;
                        break;
                    case MotionType.RS_LF:
                        return RS_LF__RS;
                        break;
                    case MotionType.RS_RF:
                        return RS_LF__RS;
                        break;
                }
                break;
            case MotionType.RS_RF:
                switch (end)
                {
                    case MotionType.LS_LF:
                        return RS_RF__LS;
                        break;
                    case MotionType.LS_RF:
                        return RS_RF__LS;
                        break;
                    case MotionType.RS_LF:
                        return RS_RF__RS;
                        break;
                    case MotionType.RS_RF:
                        return RS_RF__RS;
                        break;
                }
                break;
        }

        return null;
    }
    public AnimationClip Get_FirstReadyClip(MotionType start)
    {
        switch (start)
        {
            case MotionType.LS_LF:
                return RS_LF__LS;
                break;
            case MotionType.LS_RF:
                return RS_RF__LS;
                break;
            case MotionType.RS_LF:
                return LS_LF__RS;
                break;
            case MotionType.RS_RF:
                return LS_RF__RS;
                break;
        }

        return null;
    }
}