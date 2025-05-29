using UnityEngine;
using System.Collections;

public class UI_Error : MonoBehaviour
{
    private static UI_Error obj;
    public static UI_Error This() { return obj; }

    public Texture2D windowTex, dimTex;
    public GUIStyle buttonStyle, textStyle;
    private Rect windowGpRect, windowBgRect, textRect, buttonRect, dimRect,
        backRect, continueRect;
    private string errorString;

    void Start()
    {
        obj = this;

        windowGpRect = UIUtil.NewRect(312, 244, 400, 280);
        windowBgRect = UIUtil.NewRect(0,0,400,280);
        textRect = UIUtil.NewRect(15, 30, 370, 150);
        buttonRect = UIUtil.NewRect(125, 218, 150, 40);
        dimRect = UIUtil.NewRect(0, 0, 1024, 768);
        backRect = UIUtil.NewRect(25, 218, 150, 40);
        continueRect = UIUtil.NewRect(225, 218, 150, 40);

        buttonStyle = UIUtil.FontFix(buttonStyle);
        textStyle = UIUtil.FontFix(textStyle);
    }

    public void SetErrorText(string txt)
    {
        errorString = txt;
    }

    public void Draw(int num)
    {
        // draw dim thing
        GUI.DrawTexture(dimRect, dimTex);

        // begin group
        GUI.BeginGroup(windowGpRect);

        // draw window
        GUI.DrawTexture(windowBgRect, windowTex);

        // draw window text
        GUI.Label(textRect, errorString, textStyle);

        //
        if (num == 0)
        {
            // draw okay button
            if (GUI.Button(buttonRect, "Okay", buttonStyle))
                CloseWindow();
        }

        if (num == 1)
        {
            if (GUI.Button(backRect, "Back", buttonStyle))
                CloseWindow();

            if (GUI.Button(continueRect, "Continue", buttonStyle))
                Continue();
        }

        // end group
        GUI.EndGroup();
    }

    private void CloseWindow()
    {
        GameControlMaster.This().ErrorWindowClosed();
    }

    // hardcoded to one thing. fix this later
    private void Continue()
    {
        CloseWindow();
        GameControlMaster.This().SimStarted(1, true);
    }
}