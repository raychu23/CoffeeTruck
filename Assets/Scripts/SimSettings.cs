using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class SimSettings : MonoBehaviour
{

    public GameObject toggleOptionPrefab;
    public List<GameObject> toggleGroupList;
    public string numberOfSimulationsAsString;
    public int designType;
    public InputField numberOfSimsInputField;
    public Text numberOfDaysText;

    public SimBottomDetailController detailController;

    void Start()
    {
        ConfigureSimSettingsUI();
    }

    public void SetNumberOfSimulations(string value)
    {
        numberOfSimulationsAsString = value;
        SetNumberOfDays();
    }

    private void SetNumberOfDays()
    {
        int numberOfSims = 0;
        int.TryParse(numberOfSimulationsAsString, out numberOfSims);
        numberOfDaysText.text = SimSetup.This().NumberOfDays(SimSetup.This().cats, designType, numberOfSims).ToString();
    }

    public void SetDesignType(int value)
    {
        designType = value;
        SetNumberOfDays();
    }

    private void ConfigureSimSettingsUI()
    {
        int toggleGroupCount = 0;

        numberOfSimsInputField.text = numberOfSimulationsAsString;// = "0";

        // for each setting category that we are looking at
        foreach (Category cat in SimSetup.This().cats)
        {
            // lazy hack for now
            if (toggleGroupCount == 4) break;

            // set the title

            // for each option we have
            foreach (Category.Option option in cat.options)
            {
                // create a toggle group in the proper toggle group
                GameObject newToggle = GameObject.Instantiate(toggleOptionPrefab);
                newToggle.transform.SetParent(toggleGroupList[toggleGroupCount].transform);
                newToggle.transform.localScale = Vector3.one;

                // set the toggle text to the right text
                newToggle.GetComponentInChildren<Text>().text = option.text;

                // get the toggle and tell it if it should be on or off
                Toggle curToggle = newToggle.GetComponent<Toggle>();
                curToggle.isOn = option.active;

                // get our toggle group and assign it
                //ToggleGroup thisToggleGroup = toggleGroupList[toggleGroupCount].GetComponent<ToggleGroup>();
                //curToggle.group = thisToggleGroup;

                // add a listener to that toggle button
                //newToggle.GetComponent<Toggle>().onValueChanged.AddListener(SelectOption(option), value);
                Category.Option thisOption = option;
                curToggle.onValueChanged.AddListener(delegate { ToggleClicked(curToggle.isOn, thisOption); });
            }

            // increment our group count for the next one
            toggleGroupCount++;
        }
    }

    private void ToggleClicked(bool value, Category.Option curOption)
    {
        curOption.SetValue(value);

        detailController.Reconfigure();
        SetNumberOfDays();
    }

    public void StartSim()
    {
        if (!CalcUtil.AtleastOneThingSelected(SimSetup.This().cats))
        {
            SimScreenManager.instance.CreateErrorWindow("You didn't select an option in each category.");
            return;
        }

        if (!CalcUtil.NoMoreThanThreeShift(SimSetup.This().cats))
        {
            SimScreenManager.instance.CreateErrorWindow("You can't select more than three shifts.");
            return;
        }

        int numberOfSims = 0;
        int.TryParse(numberOfSimulationsAsString, out numberOfSims);
        //Debug.Log(numberOfSims);
        if (numberOfSims < 1 || numberOfSims == null)
        {
            SimScreenManager.instance.CreateErrorWindow("Please enter a number of simulations.");
            return;
        }

        int numberOfDays = SimSetup.This().NumberOfDays(SimSetup.This().cats, designType, numberOfSims);

        if (numberOfDays > 60)
        {
            SimScreenManager.instance.CreateErrorWindow("Jo can't work more than 60 days in a row.");
            return;
        }

        SimSetup.This().saveData = TopBarSaveController.instance.saveData;
        SimSetup.This().playerName =TopBarSaveController.instance.playerName;
        SimSetup.This().groupName = TopBarSaveController.instance.groupName;
        Debug.Log("SimSetting" + SimSetup.This().playerName);
        SimScreenManager.instance.PerformOurSimulation(numberOfSims, designType, false);
        //SimSetup.This().StartSim(numberOfSimulations, false);
    }

    public void SkipToBusinessPlan()
    {
        SimSetup.This().saveData = TopBarSaveController.instance.saveData;
        SimSetup.This().playerName = TopBarSaveController.instance.playerName;
        SimSetup.This().groupName  = TopBarSaveController.instance.groupName;
        SimScreenManager.instance.ShowActualRunSettings();
    }
}