using System;
using UnityEngine;

public class InputUtils
{
    public static KeyCode GetAnyKeyCodeDwon()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    //Debug.Log(keyCode.ToString());
                    return keyCode;
                }
            }
        }
        return KeyCode.None;
    }
}