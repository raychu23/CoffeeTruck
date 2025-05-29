using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DisplayAverageResults : MonoBehaviour
{
    public static DisplayAverageResults This() { return obj; }
    private static DisplayAverageResults obj;

    public List<Text> categoryTexts, row1Texts, row2Texts, row3Texts, row4Texts;

    void Start()
    {
        obj = this;

        SetTableByCategory(0);
    }

    public void SetTableByCategory(int newCategoryNum)
    {
        ConfigureTableBasedOnCategory(newCategoryNum);
    }

    private void ConfigureTableBasedOnCategory(int categoryNum)
    {
        //public void GenerateAverages(int catNum)
        SimArchive.This().GenerateAverages(categoryNum);

        // set name of the category texts as well as the text itself
        for (int i = 0; i < 4; i++)
        {
            if (i < SimSetup.This().cats[categoryNum].options.Count)
            {
                categoryTexts[i].text = SimSetup.This().cats[categoryNum].options[i].text;
            }
            else
            {
                categoryTexts[i].text = "";
            }

            row1Texts[i].text = RoundToDecimalPoint(SimArchive.This().GetAverage(0, i, categoryNum), 2);
            row2Texts[i].text = RoundToDecimalPoint(SimArchive.This().GetAverage(1, i, categoryNum), 2);
            row3Texts[i].text = RoundToDecimalPoint(SimArchive.This().GetAverage(2, i, categoryNum), 2);
            row4Texts[i].text = RoundToDecimalPoint(SimArchive.This().GetAverage(3, i, categoryNum), 2);
        }
    }

    private string RoundToDecimalPoint(string stringValue, int decimalPos)
    {
        float floatValue = -500;
        float.TryParse(stringValue, out floatValue);

        if (floatValue == 0) return "";

        return System.Math.Round((double)floatValue, decimalPos, System.MidpointRounding.AwayFromZero).ToString();
    }

    public void GetGameData()
    {
        CalcUtil.OpenGameData();
    }

    public void RestartTrial()
    {
        SimScreenManager.instance.ShowSimSettings();
    }

    public void BusinessPlan()
    {
        SimScreenManager.instance.ShowActualRunSettings();
    }
}