using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class UI_Setup : MonoBehaviour
{
    public string numSims;
    private int numDays;
    private static UI_Setup obj;
    public static UI_Setup This() { return obj; }

    public Texture2D bgImg, topBgImg, foodTruckImg, saveDataImg;
    public Texture2D[] locationImgs, priceImgs;
    private Rect bgRect, topBgRect, foodTruckRect, simNumberRect,
        locationRect, priceRect, startBtnRect, numDaysRect, skipBtnRect;
    public Rect playerNameRect, groupNameRect, saveDataRect, saveDataBtnRect;
    private Rect[] locationsRect, pricesRect, musicsRect, timesRect;
    public GUIStyle startBtnStyle, simNumberStyle, nameStyle, checkBtnActiveStyle,
        checkBtnInactiveStyle;

    void Start()
    {
        obj = this;
        numSims = "1";

        bgRect = UIUtil.NewRect(0, 413, 1024, 355);
        topBgRect = UIUtil.NewRect(0, 0, 1024, 412);
        foodTruckRect = UIUtil.NewRect(365, 473, 659, 295);
        locationRect = UIUtil.NewRect(0, 413, 918, 355);
        priceRect = UIUtil.NewRect(649, 657, 63, 64);
        startBtnRect = UIUtil.NewRect(896, 389, 116, 34);
        simNumberRect = UIUtil.NewRect(952, 328, 46, 21);
        saveDataBtnRect = UIUtil.NewRect(915, 17, 92, 28);
        playerNameRect = UIUtil.NewRect(454, 24, 160, 30);
        groupNameRect = UIUtil.NewRect(737, 24, 160, 30);
        saveDataRect = UIUtil.NewRect(397, 0, 627, 49);
        numDaysRect = UIUtil.NewRect(950, 362, 100, 30);
        skipBtnRect = UIUtil.NewRect(20, 389, 231, 34);

        locationsRect = new Rect[4];
        pricesRect = new Rect[4];
        musicsRect = new Rect[4];
        timesRect = new Rect[4];

        for (int i = 0; i < 4; i++)
        {
            locationsRect[i] = UIUtil.NewRect(168, 100 + 31 * i, 30, 30);
            musicsRect[i] = UIUtil.NewRect(370, 100 + 31 * i, 30, 30);
            pricesRect[i] = UIUtil.NewRect(574, 100 + 31 * i, 30, 30);
            timesRect[i] = UIUtil.NewRect(774, 106 + 45 * i, 30, 30);
        }

        startBtnStyle = UIUtil.FontFix(startBtnStyle);
        simNumberStyle = UIUtil.FontFix(simNumberStyle);
        nameStyle = UIUtil.FontFix(nameStyle);
        checkBtnActiveStyle = UIUtil.FontFix(checkBtnActiveStyle);
        checkBtnInactiveStyle = UIUtil.FontFix(checkBtnInactiveStyle);
    }

    public void Draw()
    {
        DrawBg();
        DrawCategoryOptions();
        DrawSimSettings();
        UI_SetupPulldown.This().Draw();
    }

    private void DrawBg()
    {
        GUI.DrawTexture(bgRect, bgImg);
        GUI.DrawTexture(topBgRect, topBgImg);

        // draw location behind food truck
        GUI.DrawTexture(locationRect, GetLocationTex());

        // draw foodtruck
        GUI.DrawTexture(foodTruckRect, foodTruckImg);

        // draw prices
        GUI.DrawTexture(priceRect, GetPriceTex());
    }

    private Texture2D GetLocationTex()
    {
        for (int i = 0; i < SimSetup.This().cats[0].options.Count; i++)
        {
            if (SimSetup.This().cats[0].options[i].active)
                return locationImgs[i];
        }

        // dont have one selected? butts
        return locationImgs[0];
    }

    private Texture2D GetPriceTex()
    {
        for (int i = 0; i < SimSetup.This().cats[2].options.Count; i++)
        {
            if (SimSetup.This().cats[2].options[i].active)
                return priceImgs[i];
        }

        // dont have one selected? butts
        return priceImgs[0];
    }

    private void DrawCategoryOptions()
    {
        int cNum = 0;
        List<Category> cats = SimSetup.This().cats;
        // for each category
        foreach (Category cat in cats)
        {
            int oNum = 0;
            // for each item in category
            foreach (Category.Option op in cat.options)
            {
                // draw text of item
                //GUI.Label(new Rect(20, 40 + 35 * oNum, 180, 30), op.text);
                // draw checkbox button
                if (CheckBoxButton(cNum, oNum, op.active))
                    op.Select();


                oNum++;
            }

            //GUI.EndGroup();
            cNum++;
        }
    }

    private Rect CheckmarkRect(int cNum, int oNum)
    {
        if (cNum == 0) return locationsRect[oNum];

        else if (cNum == 1) return musicsRect[oNum];

        else if (cNum == 2) return pricesRect[oNum];

        else return timesRect[oNum];
    }

    private bool CheckBoxButton(int cNum, int oNum, bool active)
    {
        if (cNum == 4) return false;

        if (GUI.Button(CheckmarkRect(cNum, oNum), "", active ? checkBtnActiveStyle : checkBtnInactiveStyle))
            return true;

        return false;
    }

    private void DrawSimSettings()
    {
        numSims = Regex.Replace(GUI.TextField(simNumberRect, numSims, simNumberStyle), "[^0-9]", "");
        int numberOfDays = SimSetup.This().NumberOfDays(SimSetup.This().cats, UI_SetupPulldown.This().comboBoxControl.SelectedItemIndex, numSims != "" ? int.Parse(numSims) : 0);
        GUI.Label(numDaysRect, numberOfDays.ToString(), simNumberStyle);

        // SKIP!
        if (GUI.Button(skipBtnRect, "Skip to Business Plan", startBtnStyle))
        {
            GameControlMaster.This().SkipToFinalOptions();
        }

        if (GUI.Button(startBtnRect, "Start", startBtnStyle))
        {
            if (!CalcUtil.AtleastOneThingSelected(SimSetup.This().cats))
                GameControlMaster.This().ErrorWindowOpen("You didn't select an option in each category.");

            else if (numSims == "")
                GameControlMaster.This().ErrorWindowOpen("Please enter a number of simulations.");

            else if (numberOfDays > 60)
                GameControlMaster.This().ErrorWindowOpen("Jo can't work more than 60 days in a row.");

            //else if (TotalDayCheck(SimSetup.This().cats, int.Parse(numSims), UI_SetupPulldown.This().comboBoxControl.SelectedItemIndex))
            //    GameControlMaster.This().ErrorWindowOpen("Joe can't test more than 20 days in a row.");

            //else if (OverworkedCheck(SimSetup.This().cats, int.Parse(numSims), UI_SetupPulldown.This().comboBoxControl.SelectedItemIndex))
            //    GameControlMaster.This().CautionWindowOpen("You may be overworking Joe. Are you sure this is a good plan?");            

            else
                RunSim();                
        }

        //SimSetup.This().saveData = GUI.Toggle(saveDataBtnRect, SimSetup.This().saveData, "", new GUIStyle());

        //if (SimSetup.This().saveData)
        //{
        //    GUI.DrawTexture(saveDataRect, saveDataImg);

        //    SimSetup.This().playerName =
        //        GUI.TextField(playerNameRect, SimSetup.This().playerName, nameStyle);

        //    SimSetup.This().groupName =
        //        GUI.TextField(groupNameRect, SimSetup.This().groupName, nameStyle);
        //}

        GUI.Toggle(UI_Setup.This().saveDataBtnRect, SimSetup.This().saveData, "", new GUIStyle());

        if (SimSetup.This().saveData)
        {
            GUI.DrawTexture(saveDataRect, saveDataImg);

            GUI.Label(playerNameRect, SimSetup.This().playerName, nameStyle);

            GUI.Label(groupNameRect, SimSetup.This().groupName, nameStyle);
        }
    }

    //private bool OverworkedCheck(int shiftsPerDay, int days, int designType)
    //{
    //    //if (designType == 0) // if we are randomly doing one thing a day
    //    if (shiftsPerDay > 3 && days >= 8)
    //        return true;

    //    if (shiftsPerDay > 2 && days >= 20)
    //        return true;

    //    return false;
    //}

    private bool OverworkedCheck(List<Category> cats, int reps, int designType)
    {
        //int totalVariety = HowManyDays(cats, reps, designType);
        int totalVariety = SimSetup.This().NumberOfDays(cats, designType, reps);

        //if (cats[3].NumSelected() == 3 && totalVariety >= 20)
        //    return true;

        if (cats[3].NumSelected() == 4 && totalVariety >= 20)
            return true;

        return false;
    }

    private bool TotalDayCheck(List<Category> cats, int reps, int designType)
    {
        if (SimSetup.This().NumberOfDays(cats, designType, reps) > 20)
            return true;

        return false;
    }

    private void RunSim()
    {
        //GameControlMaster.This().SimStarted(false);
    }
}