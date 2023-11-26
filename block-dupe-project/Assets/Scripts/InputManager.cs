using System;
using UnityEngine;
public class InputManager
{
    public string Jump;
    public string Action;
    public string Left;
    public string Right;
    public string Up;
    public string Down;
    public string Map;
    public string Pause;
    
    public void LoadControlsFromPlayerPrefs()
    {
        Jump    = PlayerPrefs.GetString("Jump_Button");
        Action  = PlayerPrefs.GetString("Action_Button");
        Left    = PlayerPrefs.GetString("Left_Button");
        Right   = PlayerPrefs.GetString("Right_Button");
        Up      = PlayerPrefs.GetString("Up_Button");
        Down    = PlayerPrefs.GetString("Down_Button");
        Map     = PlayerPrefs.GetString("Map_Button");
        Pause   = PlayerPrefs.GetString("Pause_Button");
    }
    public void SaveControlsToPlayerPrefs()
    {
        PlayerPrefs.SetString("Jump_Button", Jump);
        PlayerPrefs.SetString("Action_Button", Action);
        PlayerPrefs.SetString("Left_Button", Left);
        PlayerPrefs.SetString("Right_Button", Right);
        PlayerPrefs.SetString("Up_Button", Up);
        PlayerPrefs.SetString("Down_Button", Down);
        PlayerPrefs.SetString("Map_Button", Map);
        PlayerPrefs.SetString("Pause_Button", Pause);
    }
    public bool IsJumpDown => Input.GetButtonDown(Jump);
    public bool IsActionDown => Input.GetButtonDown(Action);
    public bool IsMapDown => Input.GetButtonDown(Map);
    public bool IsPausedDown => Input.GetButtonDown(Pause);
    public bool IsJumpPressed => Input.GetButton(Jump);
    //public bool IsActionPressed => Input.GetButton(Action);
    //public bool IsJumpUp=> Input.GetButtonUp(Jump);
    public bool IsActionUp => Input.GetButtonUp(Action);

    //takes joystick input from all sticks.
    public bool IsLeftPressed => Input.GetAxis("Horizontal") < 0 || Input.GetButton(Left);
    public bool IsRightPressed => Input.GetAxis("Horizontal") > 0 || Input.GetButton(Right);
    public bool IsUpPressed => Input.GetAxis("Vertical") < 0 || Input.GetButton(Up);
    public bool IsDownPressed => Input.GetAxis("Vertical") < 0 || Input.GetButton(Down);
    public int GetHorizontalValue()
    {
        if(IsLeftPressed && !IsRightPressed)
        {
            return -1;
        }
        else if(IsRightPressed && !IsLeftPressed)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    public int GetVerticalValue()
    {
        if(IsUpPressed && !IsDownPressed)
        {
            return -1;
        }
        else if(IsDownPressed && !IsUpPressed)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    

}