using UnityEngine;
using System.Collections;

public class UI_Loading : MonoBehaviour {

    private static UI_Loading obj;
    public static UI_Loading This() { return obj; }

    public Texture2D windowTex, dimTex;
    public GUIStyle textStyle;
    private Rect windowGpRect, windowBgRect, textRect, buttonRect, dimRect,
        backRect, continueRect;
    private string errorString;

    void Start()
    {
        obj = this;

        windowGpRect = UIUtil.NewRect(312, 244, 400, 200);
        windowBgRect = UIUtil.NewRect(0,0,400,200);
        textRect = UIUtil.NewRect(15, 30, 370, 150);
        dimRect = UIUtil.NewRect(0, 0, 1024, 768);

        textStyle = UIUtil.FontFix(textStyle);
    }

    public void Draw()
    {
        // draw dim thing
        GUI.DrawTexture(dimRect, dimTex);

        // begin group
        GUI.BeginGroup(windowGpRect);

        // draw window
        GUI.DrawTexture(windowBgRect, windowTex);

        // draw window text
        GUI.Label(textRect, "Loading...", textStyle);

        // end group
        GUI.EndGroup();
    }
}
