using UnityEngine;
using System.Collections;

public class UI_StartScene : MonoBehaviour
{
    private static UI_StartScene obj;
    public static UI_StartScene This() { return obj; }

    public Texture2D bgImg;
    public GUIStyle startBtnStyle;
    private Rect bgRect, startBtnRect;
    private int curState;

    void Start()
    {
        obj = this;
        curState = 0;

        bgRect = UIUtil.NewRect(0, 0, 1024, 768);
        startBtnRect = UIUtil.NewRect(605, 511, 170, 107);

        startBtnStyle = UIUtil.FontFix(startBtnStyle);
    }

    public void OnGUI()
    {
        if (GameControlMaster.This().loading)
            GUI.enabled = false;

        GUI.DrawTexture(bgRect, bgImg);

        if (curState == 0)
        {
            if (GUI.Button(startBtnRect, "", startBtnStyle))
            {
                //curState++;
                Begin();
            }
        }

        else
        {

        }
    }

    private void Begin()
    {
        GameControlMaster.This().StartStory();
    }
}