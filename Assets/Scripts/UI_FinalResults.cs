using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_FinalResults : MonoBehaviour
{
    private static UI_FinalResults obj;
    public static UI_FinalResults This() { return obj; }

    private List<Vector2> graphData;
    private float timeCatch, minMoney, maxMoney;

    private Rect bgRect, startFundsRect, endFundsRect, restartBtnRect, gameDataBtnRect,
        nameRect, groupRect, graphGpRect, graphMasterGpRect;
    private Rect[] xAxisRect, yAxisRect, xTickRect, yTickRect;

    public Texture2D bgTex, tickHorizTex, tickVertTex, graphDataTex;
    public GUIStyle nameStyle, fundsStyle, btnStyle, labelStyle, labelStyle2;
    
    void Start()
    {
        obj = this;

        bgRect = UIUtil.NewRect(0, 0, 1024, 768);
        nameRect = UIUtil.NewRect(870, 10, 120, 30);
        groupRect = UIUtil.NewRect(870, 27, 120, 30);
        startFundsRect = UIUtil.NewRect(275, 694, 140, 50);
        endFundsRect = UIUtil.NewRect(239, 730, 140, 50);
        restartBtnRect = UIUtil.NewRect(836, 703, 130, 34);
        gameDataBtnRect = UIUtil.NewRect(836, 644, 130, 34);
        graphGpRect = UIUtil.NewRect(76, 7, 883, 468);
        graphMasterGpRect = UIUtil.NewRect(44, 110, 965, 540);

        xAxisRect = new Rect[14];
        xTickRect = new Rect[14];
        yTickRect = new Rect[7];
        yAxisRect = new Rect[7];
        for (int i = 0; i < 14; i++)
        {
            xTickRect[i] = UIUtil.NewRect(116 + i * 68, 594, 3, 10);
            xAxisRect[i] = UIUtil.NewRect(95 + i * 68, 606, 40, 30);
        }
        for (int j = 0; j < 7; j++)
        {
            yTickRect[j] = UIUtil.NewRect(101, 581 - j * 77, 10, 4);
            yAxisRect[j] = UIUtil.NewRect(97, 576 - j * 77, 0, 0);
        }

        nameStyle = UIUtil.FontFix(nameStyle);
        fundsStyle = UIUtil.FontFix(fundsStyle);
        btnStyle = UIUtil.FontFix(btnStyle);
        labelStyle = UIUtil.FontFix(labelStyle);
        labelStyle2 = UIUtil.FontFix(labelStyle2);
    }

    public void SetupGraph(List<SimArchive.ArchiveEntry> data)
    {
        graphData = new List<Vector2>();

        // figure out how tall graph is
        maxMoney = minMoney = data[0].currentMoney;
        float maxData = 0;
        foreach (SimArchive.ArchiveEntry dat in data)
        {
            maxData++;
            if (dat.currentMoney > maxMoney)
                maxMoney = dat.currentMoney;
            if (dat.currentMoney < minMoney)
                minMoney = dat.currentMoney;
        }

        // add each point of data to vector2 where 0 = first point of data and 731 = last point
        for (int i = 0; i < data.Count; i+=2)
        {
            //if (i % 2 == 0)
                graphData.Add(new Vector2(i / 130f, (data[i].currentMoney - minMoney) / (maxMoney - minMoney)));
        }

        timeCatch = Time.time + 3;
    }

    public void Draw()
    {
        DrawGeneralThings();
        DrawGraph();
    }

    private void DrawGraph()
    {
        // draw graph axis thing
        GUI.Label(xAxisRect[0], "0", labelStyle);
        GUI.Label(xAxisRect[1], "5", labelStyle);
        GUI.Label(xAxisRect[2], "10", labelStyle);
        GUI.Label(xAxisRect[3], "15", labelStyle);
        GUI.Label(xAxisRect[4], "20", labelStyle);
        GUI.Label(xAxisRect[5], "25", labelStyle);
        GUI.Label(xAxisRect[6], "30", labelStyle);
        GUI.Label(xAxisRect[7], "35", labelStyle);
        GUI.Label(xAxisRect[8], "40", labelStyle);
        GUI.Label(xAxisRect[9], "45", labelStyle);
        GUI.Label(xAxisRect[10], "50", labelStyle);
        GUI.Label(xAxisRect[11], "55", labelStyle);
        GUI.Label(xAxisRect[12], "60", labelStyle);
        GUI.Label(xAxisRect[13], "65", labelStyle);

        for (int i = 0; i < 7; i++)
        {
            GUI.Label(yAxisRect[i], FormatMoneyNoCents(minMoney + (maxMoney - minMoney) * i / 6f), labelStyle2);
        }

        foreach (Rect r in xTickRect)
        {
            GUI.DrawTexture(r, tickHorizTex);
        }

        foreach (Rect r in yTickRect)
        {
            GUI.DrawTexture(r, tickHorizTex);
        }

        // start group
        GUI.BeginGroup(graphMasterGpRect);
        
        // for each point of data, draw it
        GUI.BeginGroup(graphGpRect);

        for (int i = 0; i < graphData.Count; i++)
        {
            if (Time.time - timeCatch > i / 50f)
            {
                //if (i % 5 == 0)
                GUI.DrawTexture(new Rect(graphData[i].x * (graphGpRect.width - UIUtil.ScaleFloat(4)), (1 - graphData[i].y) * graphGpRect.height, UIUtil.ScaleFloat(12), UIUtil.ScaleFloat(11)), graphDataTex);
            }
        }

        // end group
        GUI.EndGroup();
        GUI.EndGroup();
    }

    private void DrawGeneralThings()
    {
        GUI.DrawTexture(bgRect, bgTex);

        // draw final money
        GUI.Label(startFundsRect, FormatMoney(SimArchive.This().startingMoney), fundsStyle);
        GUI.Label(endFundsRect, FormatMoney(SimArchive.This().curMoney), fundsStyle);

        // draw restart button
        if (GUI.Button(restartBtnRect, "Replay", btnStyle))
        {
            GameControlMaster.This().RestartGame();
        }

        if (GUI.Button(gameDataBtnRect, "Game Data", btnStyle))
        {
            CalcUtil.OpenGameData();
        }

        if (SimSetup.This().saveData)
        {
            GUI.Label(nameRect, SimSetup.This().playerName, nameStyle);
            GUI.Label(groupRect, SimSetup.This().groupName, nameStyle);
        }
    }

    public string FormatMoney(float foo)
    {
        return string.Format("{0:C}", foo);
    }

    public string FormatMoneyNoCents(float foo)
    {
        return string.Format("{0:C0}", foo);
    }
}
