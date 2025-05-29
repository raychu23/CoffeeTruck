using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TopBarSaveController : MonoBehaviour {

    public static TopBarSaveController instance;
    //public Toggle saveDataToggle;
    public InputField nameInput, groupInput;
    public GameObject saveDataGroup;
    public bool saveData;
    public string playerName;
    public string groupName;

    void Start () {
       
        instance = this;
        UpdateNamesFromSaveData();
        SetToggleCorrectly();
	}

    public void ToggleSavingOurData(bool value)
    {
        // set our group to be visible if we are saving data
        saveDataGroup.SetActive(true);

        // tell the controller that we are saving
        //SimSetup.This().saveData = value;
    }

    private void SetToggleCorrectly()
    {
        saveData = SimSetup.This().saveData;
    }

    public void SaveOurData(bool value)
    {
        if (value)
        {
            SimSetup.This().saveData = true;
        }
    }

    public void DontSaveOurData(bool value)
    {
        if (value)
        {
            SimSetup.This().saveData = false;
        }
    }

    private void UpdateNamesFromSaveData()
    {
        nameInput.text = SimSetup.This().playerName;
        playerName = SimSetup.This().playerName;
        playerName = PlayerPrefs.GetString("NameID","");
        groupInput.text = SimSetup.This().groupName;
        groupName = SimSetup.This().groupName;
        groupName = PlayerPrefs.GetString("GroupID","");
    }

    public void SetSaveName(string value)
    {
        PlayerPrefs.SetString("NameID",value);
        SimSetup.This().playerName = value;
    }

    public void SetSaveGroupName(string value)
    {
        PlayerPrefs.SetString("GroupID",value);
        SimSetup.This().groupName = value;
    }
}