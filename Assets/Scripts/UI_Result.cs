using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Result : MonoBehaviour
{
    private static UI_Result obj;
    public static UI_Result This() { return obj; }

    private int curCat;
    private float offset;
    private Rect topLabelRect, slidingGpHolderRect, bgRect, graphRect,
        restartBtnRect, gameDataBtnRect, continueBtnRect, nameRect, groupRect;
    private Rect[] categoryRect, timeRect, tabBtnRect, column1Rects, column2Rects, column3Rects, column4Rects;

    public Texture2D bgTex, graphTex;
    public GUIStyle buttonStyle, nameStyle, graphStyle, tabBtnActiveStyle, tabBtnInactiveStyle;

    void Start()
    {
        obj = this;

        bgRect = UIUtil.NewRect(0, 0, 1024, 768);
        graphRect = UIUtil.NewRect(72, 202, 880, 332);
        categoryRect = new Rect[4];
        timeRect = new Rect[4];
        column1Rects = new Rect[4];
        column2Rects = new Rect[4];
        column3Rects = new Rect[4];
        column4Rects = new Rect[4];
        for (int i = 0; i < 4; i++)
        {
            categoryRect[i] = UIUtil.NewRect(3, 66 * (1 + i), 172, 63);
            timeRect[i] = UIUtil.NewRect(178 + 176 * i, 3, 173, 59);
            column1Rects[i] = UIUtil.NewRect(0, i * 66, 173, 63);
            column2Rects[i] = UIUtil.NewRect(176, i * 66, 173, 63);
            column3Rects[i] = UIUtil.NewRect(352, i * 66, 173, 63);
            column4Rects[i] = UIUtil.NewRect(528, i * 66, 173, 63);
        }
        topLabelRect = UIUtil.NewRect(3, 3, 172, 59);
        slidingGpHolderRect = UIUtil.NewRect(178, 66, 750, 300);
        gameDataBtnRect = UIUtil.NewRect(800, 580, 166, 34);
        restartBtnRect = UIUtil.NewRect(800, 625, 166, 34);
        continueBtnRect = UIUtil.NewRect(800, 710, 166, 34);
        nameRect = UIUtil.NewRect(870, 10, 120, 30);
        groupRect = UIUtil.NewRect(870, 27, 120, 30);

        tabBtnRect = new Rect[3];
        for (int j = 0; j < 3; j++)
        {
            tabBtnRect[j] = UIUtil.NewRect(18 + 146 * j, 152, 145, 35);
        }

        buttonStyle = UIUtil.FontFix(buttonStyle);
        nameStyle = UIUtil.FontFix(nameStyle);
        graphStyle = UIUtil.FontFix(graphStyle);
        tabBtnActiveStyle = UIUtil.FontFix(tabBtnActiveStyle);
        tabBtnInactiveStyle = UIUtil.FontFix(tabBtnInactiveStyle);
    }

    public void Setup()
    {
        curCat = 0;
        SimArchive.This().GenerateAverages(curCat);
    }

    public void SetCat(int num)
    {
        curCat = num;
        SimArchive.This().GenerateAverages(curCat);
    }

    public void Draw()
    {
        // draw bg
        GUI.DrawTexture(bgRect, bgTex);

        // draw name and group thing (if they saved data)
        if (SimSetup.This().saveData)
        {
            GUI.Label(nameRect, SimSetup.This().playerName, nameStyle);
            GUI.Label(groupRect, SimSetup.This().groupName, nameStyle);
        }

        GUI.skin.button.wordWrap = true;
        if (GUI.Button(continueBtnRect, "Business Plan", buttonStyle))
        {
            GameControlMaster.This().PickFinalOptions();
        }

        if (GUI.Button(gameDataBtnRect, "Game Data", buttonStyle))
        {
            CalcUtil.OpenGameData();
        }

        if (GUI.Button(restartBtnRect, "Restart Trial", buttonStyle))
        {
            GameControlMaster.This().RestartTrial();
        }


        // draw.. something else
        DrawTable(SimArchive.This().archiveList, SimSetup.This().cats);
    }

    private void DrawTable(List<SimArchive.ArchiveEntry> entries, List<Category> cats)
    {
        // draw graph
        GUI.DrawTexture(graphRect, graphTex);

        // DrawHeaderTabs
        for (int i = 0; i < 3; i++)
        {
            if (curCat == i)
                GUI.Label(tabBtnRect[i], cats[i].name, tabBtnActiveStyle);
            else
                if (GUI.Button(tabBtnRect[i], cats[i].name, tabBtnInactiveStyle))
                {
                    SetCat(i);
                }
        }

        // begin group
        GUI.BeginGroup(graphRect);

        // draw categories that were options on side
        for (int j = 0; j < cats[curCat].options.Count; j++)
        {
            GUI.Label(categoryRect[j], cats[curCat].options[j].text, graphStyle);
        }
        for (int k = 0; k < 4; k++)
        {
            GUI.Label(timeRect[k], cats[3].options[k].text.Replace(": ", ":\n"), graphStyle);
        }

        // draw top label (cups sold)
        GUI.Label(topLabelRect, "Average Number\nof Cups Sold:", graphStyle);

        // begin graph group
        GUI.BeginGroup(slidingGpHolderRect);

        for (int i = 0; i < SimSetup.This().cats[curCat].options.Count; i++)
        {
            GUI.Box(column1Rects[i], SimArchive.This().GetAverage(i, 0, curCat), graphStyle);
            GUI.Box(column2Rects[i], SimArchive.This().GetAverage(i, 1, curCat), graphStyle);
            GUI.Box(column3Rects[i], SimArchive.This().GetAverage(i, 2, curCat), graphStyle);
            GUI.Box(column4Rects[i], SimArchive.This().GetAverage(i, 3, curCat), graphStyle);
        }

        GUI.EndGroup();

        // end total group
        GUI.EndGroup();
    }

    private float MaxSliderSize(List<int> catTotal)
    {
        float maxSize = 0;
        foreach (int foo in catTotal)
        {
            if (foo > maxSize) maxSize = foo;
        }

        return Mathf.Max((maxSize - 8f) * 100, 5);
    }
}