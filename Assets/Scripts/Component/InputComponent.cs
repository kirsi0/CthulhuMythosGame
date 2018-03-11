using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType
{
    Bag=100,
    StopTurn,
    NextOne,
    UseItem
}

public class InputComponent : BasicComponent
{
    //当前点击键位
    public int currentKey=0;
    public Vector3 currentPos;
    public bool leftButtonDown=false;
    public bool rightButtonDown=false;
    public bool midButtonDown = false;
    [HideInInspector]
    public float afkTime=0;
}