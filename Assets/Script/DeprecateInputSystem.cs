using System;
using UnityEngine;
using MPJamPack;

public class DeprecateInputSystem : AbstractCharacterInput {
    private KeyCode jumpKey = KeyCode.Space;
    private KeyCode upKey = KeyCode.W;
    private KeyCode downKey = KeyCode.S;
    private KeyCode leftKey = KeyCode.A;
    private KeyCode rightKey = KeyCode.D;
    private KeyCode runKey = KeyCode.LeftShift;

    public override bool Up { get { return !locked && Input.GetKey(upKey); } }
    public override bool Down { get { return !locked && Input.GetKey(downKey); } }
    public override bool Left { get { return !locked && Input.GetKey(leftKey); } }
    public override bool Right { get { return !locked && Input.GetKey(rightKey); } }

    private bool upDown, downDown, leftDown, rightDown;
    private Timer upDownTimer = new Timer(0.1f),
                  downDownTimer = new Timer(0.1f),
                  leftDownTimer = new Timer(0.1f),
                  rightDownTimer = new Timer(0.1f);

    public override bool UpDown { get { 
        if (upDown && !upDownTimer.Ended) {
            upDown = false;
            upDownTimer.Reset();
            return true;
        }
        return false;
    } }
    public override bool DownDown { get { 
        if (downDown && !downDownTimer.Ended) {
            downDown = false;
            downDownTimer.Reset();
            return true;
        }
        return false;
    } }
    public override bool LeftDown { get { 
        if (leftDown && !leftDownTimer.Ended) {
            leftDown = false;
            leftDownTimer.Reset();
            return true;
        }
        return false;
    } }
    public override bool RightDown { get {
        if (rightDown && !rightDownTimer.Ended) {
            rightDown = false;
            rightDownTimer.Reset();
            return true;
        }
        return false;
    } }

    public override bool UpUp { get { return Input.GetKeyUp(upKey); } }
    public override bool DownUp { get { return Input.GetKeyUp(downKey); } }
    public override bool LeftUp { get { return Input.GetKeyUp(leftKey); } }
    public override bool RightUp { get { return Input.GetKeyUp(rightKey); } }

    private bool locked;

    private void Update() {
        if (upDownTimer.Running && upDownTimer.UpdateEnd)
        {
            upDown = false;
            upDownTimer.Reset();
        }
        if (downDownTimer.Running && downDownTimer.UpdateEnd)
        {
            downDown = false;
            downDownTimer.Reset();
        }
        if (leftDownTimer.Running && leftDownTimer.UpdateEnd)
        {
            leftDown = false;
            leftDownTimer.Reset();
        }
        if (rightDownTimer.Running && rightDownTimer.UpdateEnd)
        {
            rightDown = false;
            rightDownTimer.Reset();
        }

        if (locked) {
            return;
        }

        if (Input.GetKeyDown(upKey))
        {
            upDown = true;
            upDownTimer.Running = true;
        }
        if (Input.GetKeyDown(downKey))
        {
            downDown = true;
            downDownTimer.Running = true;
        }
        if (Input.GetKeyDown(leftKey))
        {
            leftDown = true;
            leftDownTimer.Running = true;
        }
        if (Input.GetKeyDown(rightKey))
        {
            rightDown = true;
            rightDownTimer.Running = true;
        }
    }

    public override void Lock()
    {
        locked = true;
    }

    public override void Unlock()
    {
        locked = false;
    }
}