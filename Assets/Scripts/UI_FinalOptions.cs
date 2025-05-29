using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_FinalOptions : MonoBehaviour
{
    private static UI_FinalOptions obj;
    public static UI_FinalOptions This() { return obj; }
    public Texture2D bgTex;
    public GUIStyle btnStyle, nameStyle, toggleOnStyle, toggleOffStyle;
    private Rect bgRect, nameRect, groupRect, backBtnRect, continueBtnRect;
    private Rect[] shiftRects, optionRect, btnSet1Rect, btnSet2Rect;

    private List<int> finalOps1;
    private List<int> finalOps2;

    void Start()
    {
        obj = this;

        finalOps1 = new List<int>() { 0, 0, 0, 0, 0 };
        finalOps2 = new List<int>() { 0, 0, 0, 1, 0 };

        bgRect = UIUtil.NewRect(0, 0, 1024, 768);
        nameRect = UIUtil.NewRect(870, 10, 120, 30);
        groupRect = UIUtil.NewRect(870, 27, 120, 30);
        backBtnRect = UIUtil.NewRect(14, 726, 210, 34);
        continueBtnRect = UIUtil.NewRect(814, 726, 189, 34);
        shiftRects = new Rect[2];
        shiftRects[0] = UIUtil.NewRect(16, 208, 990, 191);
        shiftRects[1] = UIUtil.NewRect(16, 507, 990, 191);
        optionRect = new Rect[5];
        btnSet1Rect = new Rect[4];
        btnSet2Rect = new Rect[4];

        for (int i = 0; i < 5; i++)
        {
            optionRect[i] = UIUtil.NewRect(154 + 202 * i, 0, 26, 191);
        }
        for (int j = 0; j < 4; j++)
        {
            btnSet1Rect[j] = UIUtil.NewRect(4, 9 + 32 * j, 18, 17);
            btnSet2Rect[j] = UIUtil.NewRect(4, 14 + 46 * j, 18, 17);
        }

        btnStyle = UIUtil.FontFix(btnStyle);
        nameStyle = UIUtil.FontFix(nameStyle);
        toggleOnStyle = UIUtil.FontFix(toggleOnStyle);
        toggleOffStyle = UIUtil.FontFix(toggleOffStyle);
    }

    public void Draw()
    {
        DrawGeneralStuff();
        DrawFinalOptions(0);
        DrawFinalOptions(1);
        DrawSettings();
    }

    private void DrawSettings()
    {
        // note: steal all these things from UI_Setup because im lazy
        GUI.Toggle(UI_Setup.This().saveDataBtnRect, SimSetup.This().saveData, "", new GUIStyle());

        if (SimSetup.This().saveData)
        {
            GUI.DrawTexture(UI_Setup.This().saveDataRect, UI_Setup.This().saveDataImg);

            GUI.Label(UI_Setup.This().playerNameRect, SimSetup.This().playerName, UI_Setup.This().nameStyle);

            GUI.Label(UI_Setup.This().groupNameRect, SimSetup.This().groupName, UI_Setup.This().nameStyle);
        }
    }

    private void DrawSettingsOld()
    {
        // note: steal all these things from UI_Setup because im lazy
        SimSetup.This().saveData = GUI.Toggle(UI_Setup.This().saveDataBtnRect, SimSetup.This().saveData, "", new GUIStyle());

        if (SimSetup.This().saveData)
        {
            GUI.DrawTexture(UI_Setup.This().saveDataRect, UI_Setup.This().saveDataImg);

            SimSetup.This().playerName =
                GUI.TextField(UI_Setup.This().playerNameRect, SimSetup.This().playerName, UI_Setup.This().nameStyle);

            SimSetup.This().groupName =
                GUI.TextField(UI_Setup.This().groupNameRect, SimSetup.This().groupName, UI_Setup.This().nameStyle);
        }
    }

    private List<int> OptionSet(int num)
    {
        if (num == 0) return finalOps1;
        return finalOps2;
    }

    private void DrawFinalOptions(int shift)
    {
        int cNum = 0;
        List<Category> cats = SimSetup.This().cats;
        // for each category
        //GUI.Box(shiftRects[shift], "");
        GUI.BeginGroup(shiftRects[shift]);

        foreach (Category cat in cats)
        {
            if (cNum == 4) break;

            // gui group
            //GUI.Box(optionRect[cNum], "");
            GUI.BeginGroup(optionRect[cNum]);

            // for each item in category
            //foreach (Category.Option op in cat.options)
            for (int oNum = 0; oNum < cat.options.Count; oNum++)
            {
                // draw checkbox button
                if (GUI.Button((cNum == 3 ? btnSet2Rect : btnSet1Rect)[oNum], "",
                    ((shift == 0 ? finalOps1 : finalOps2)[cNum] == oNum ? toggleOnStyle : toggleOffStyle)))
                {
                    SelectOp(oNum, cNum, shift);
                }
            }

            GUI.EndGroup();
            cNum++;
        }

        GUI.EndGroup();
    }

    private void SelectOp(int oNum, int cNum, int shift)
    {
        if (shift == 0)
            finalOps1[cNum] = oNum;

        else
            finalOps2[cNum] = oNum;
    }

    private void DrawGeneralStuff()
    {
        GUI.DrawTexture(bgRect, bgTex);

        if (GUI.Button(continueBtnRect, "Run for 13 Weeks", btnStyle))
        {
            RunYearSim();
        }

        if (!GameControlMaster.This().DidWeSkip())
        {
            if (GUI.Button(backBtnRect, "Review Sim Results", btnStyle))
            {
                GameControlMaster.This().BackToResults();
            }
        }
        else
        {
            if (GUI.Button(backBtnRect, "Perform a Test", btnStyle))
            {
                GameControlMaster.This().SetupTest();
            }
        }

        if (SimSetup.This().saveData)
        {
            GUI.Label(nameRect, SimSetup.This().playerName, nameStyle);
            GUI.Label(groupRect, SimSetup.This().groupName, nameStyle);
        }
    }

        //    finalOps1 = new List<int>() { 0, 0, 0, 0, 0 };
        //finalOps2 = new List<int>() { 0, 0, 0, 1, 0 };

    private void RunYearSim()
    {
        if (OptionSet(0)[3] == OptionSet(1)[3])
            GameControlMaster.This().ErrorWindowOpen("The two shifts need to occur at different times of day.");

        else
        {
            SimArchive.This().Restart();
            SimulationContainer simContainer = new SimulationContainer(OptionSet(0), OptionSet(1));
            SimArchive.This().RunThroughSims(simContainer, 2);
            GameControlMaster.This().YearStarted();
        }
    }
}