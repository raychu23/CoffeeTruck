using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RunSettings : MonoBehaviour {

    public GameObject toggleOptionPrefab;
    public List<GameObject> toggleGroupList1;
    public List<GameObject> toggleGroupList2;

    public List<int> finalOps1, finalOps2;


	void Start () {
        finalOps1 = new List<int>() { 0, 0, 0, 0, 0 };
        finalOps2 = new List<int>() { 0, 0, 0, 1, 0 };

        ConfigureRunSettingsUI(toggleGroupList1, 0);
        ConfigureRunSettingsUI(toggleGroupList2, 1);
	}

    private void ConfigureRunSettingsUI(List<GameObject> curToggleGroupList, int listNum)
    {
        int toggleGroupCount = 0;

        // for each setting category that we are looking at
        foreach (Category cat in SimSetup.This().cats)
        {
            // hardcoded lame fix, i know
            if (toggleGroupCount == 4) break;

            // set the title

            int optionCount = 0;

            // for each option we have
            foreach (Category.Option option in cat.options)
            {
                // create a toggle group in the proper toggle group
                GameObject newToggle = GameObject.Instantiate(toggleOptionPrefab);
                newToggle.transform.SetParent(curToggleGroupList[toggleGroupCount].transform);
                newToggle.transform.localScale = Vector3.one;

                // set the toggle text to the right text
                newToggle.GetComponentInChildren<Text>().text = option.text;

                // get the toggle and tell it if it should be on or off
                Toggle curToggle = newToggle.GetComponent<Toggle>();
                curToggle.isOn = listNum == 0 ?
                    (finalOps1[toggleGroupCount] == optionCount ? true : false) :
                    (finalOps2[toggleGroupCount] == optionCount ? true : false);

                // get our toggle group and assign it
                ToggleGroup thisToggleGroup = curToggleGroupList[toggleGroupCount].GetComponent<ToggleGroup>();
                curToggle.group = thisToggleGroup;

                // add a listener to that toggle button
                int groupNumber = toggleGroupCount;
                int optionNumber = optionCount;
                Category.Option thisOption = option;
                curToggle.onValueChanged.AddListener(delegate { ToggleClicked(curToggle.isOn, listNum, groupNumber, optionNumber); });

                optionCount++;
            }

            // increment our group count for the next one
            toggleGroupCount++;
        }
    }

    private void ToggleClicked(bool value, int listNum, int groupNum, int optionNum)
    {
        // set our value in our list

        // if this is turning on, lets set it
        if (value)
        {
            if (listNum == 0)
                finalOps1[groupNum] = optionNum;
            else
                finalOps2[groupNum] = optionNum;
        }
    }

    public void RunYearSim()
    {
        if (finalOps1[3] == finalOps2[3])
            SimScreenManager.instance.CreateErrorWindow("The two shifts need to occur at different times of day.");

        else
        {
            SimScreenManager.instance.RunSim(finalOps1, finalOps2);
        }
    }

    public void BackToTest()
    {
        SimScreenManager.instance.ShowSimSettings();
    }
}