using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimScreenManager : MonoBehaviour {

    public static SimScreenManager instance;

    // sim settings
    public GameObject SimSettingsPrefab;
    
    // sim results
    public GameObject SimResultsPrefab;

    // sim average graph
    public GameObject SimAverageResultsPrefab;

    // run settings
    public GameObject RunSettingsPrefab;

    // run results
    public GameObject RunResultsPrefab;

    // loading prefab
    public GameObject RunLoadPrefab;

    // error prefab
    public GameObject ErrorPrefab;

    public GameObject curPrefabInScene;


	void Start () {
        instance = this;

        ShowSimSettings();
	}

    public void ShowSimSettings()
    {
        EmptyCurPrefab();

        LoadPrefab(SimSettingsPrefab);
    }

    public void PerformOurSimulation(int numberOfSims, int designType, bool overworked)
    {
        // actually run our sim
        SimSetup.This().ClearSimData();
        SimSetup.This().StartSim(numberOfSims, designType, overworked);

        // show settings
        EmptyCurPrefab();

        LoadPrefab(SimResultsPrefab);
        // upload our sim data if we were told to upload it
        if (SimSetup.This().saveData || TopBarSaveController.instance.saveData)
        {
            SimSetup.This().Upload();
        }
            
    }

    public void ShowSimAverageResults()
    {
        EmptyCurPrefab();

        LoadPrefab(SimAverageResultsPrefab);
    }
    
    public void ShowActualRunSettings()
    {
        EmptyCurPrefab();

        // show run settings
        LoadPrefab(RunSettingsPrefab);
    }

    public void RunSim(List<int> optionSet1, List<int> optionSet2)
    {
        SimArchive.This().Restart();
        SimulationContainer simContainer = new SimulationContainer(optionSet1, optionSet2);
        SimArchive.This().RunThroughSims(simContainer, 2);

        ShowActualResults();
        // upload our sim data if we were told to upload it
        if (SimSetup.This().saveData)
        {
            SimSetup.This().Upload(true);
        }
            

        SimSetup.This().totalTests++;
    }

    public void ShowActualResults()
    {
        EmptyCurPrefab();

        // load our loading screen
        GameObject.Instantiate(RunLoadPrefab);

        // create results graph
        LoadPrefab(RunResultsPrefab);
    }

    private void EmptyCurPrefab()
    {
        if (curPrefabInScene != null)
            Destroy(curPrefabInScene);

        curPrefabInScene = null;
    }

    private void LoadPrefab(GameObject newPrefab)
    {
        curPrefabInScene = GameObject.Instantiate(newPrefab);
    }

    public void CreateErrorWindow(string errorMessageText)
    {
        GameObject errorObjInSecene = GameObject.Instantiate(ErrorPrefab);

        errorObjInSecene.GetComponent<ErrorWindow>().SetMessage(errorMessageText);
    }
}