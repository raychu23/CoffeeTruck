using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameControlMaster : MonoBehaviour
{
    public int curState;

    private static GameControlMaster obj;
    public static GameControlMaster This() { return obj; }

    public bool errorOpen, cautionOpen, skipCheck, loading;

    public void Start()
    {
        DontDestroyOnLoad(this);
        obj = this;
        loading = false;
        skipCheck = false;
        curState = 0;
        StartCoroutine("LoadLevel", 1);
        //Application.LoadLevel(1);
    }


    public bool DidWeSkip()
    {
        return skipCheck;
    }

    // we started our original testing simulation
    public void SimStarted(int numberOfSims, bool overworked)
    {
        //skipCheck = false;
        //if (!overworked)
        //    SimSetup.This().StartSim(numberOfSims, overworked);
        //// set our state so that we know we are drawing the "simulating" UI 
        //curState = 1;


        //// instead of a delay, start our new script
        //DisplayResults.This().SetupData();

        ////// set a delay before we continue
        ////StartCoroutine("TimerDealio");
    }

    // a 3 second delay until we tell the game our 
    private IEnumerator TimerDealio()
    {
        // start sliding the UI truck thing
        UI_Simulating.This().StartSlide();

        yield return new WaitForSeconds(3);

        // after 3 seconds, we are done! Continue
        SimFinished();
    }

    public void SetupTest()
    {
        curState = 0;
    }

    // finished our sim (both the first simulation and the actual test)
    public void SimFinished()
    {
        curState++;
        
        // upload our sim data if we were told to upload it
        if (SimSetup.This().saveData)
            SimSetup.This().Upload();
        //else
        //    SimSetup.This().FakeUpload();

        if (curState == 2)
        {
            UI_Result.This().Setup();
            SimSetup.This().totalTests++;
        }

        //Debug.Log(Time.time + "    " + curState + "    " + SimSetup.This().totalTests + "     " + SimArchive.This().curMoney);
    }

    public void BackToResults()
    {
        curState = 2;
    }

    public void PickFinalOptions()
    {
        curState = 3;
    }

    public void SkipToFinalOptions()
    {
        curState = 3;
        skipCheck = true;
    }

    public void RestartTrial()
    {
        curState = 0;
        SimArchive.This().Restart();
    }

    public void YearStarted()
    {
        curState = 4;
        StartCoroutine("TimerDealio");
        UI_FinalResults.This().SetupGraph(SimArchive.This().archiveList);
    }

    public void ErrorWindowOpen(string errorTxt)
    {
        UI_Error.This().SetErrorText(errorTxt);
        errorOpen = true;
    }

    public void CautionWindowOpen(string cautionTxt)
    {
        UI_Error.This().SetErrorText(cautionTxt);
        cautionOpen = true;
    }

    public void ErrorWindowClosed()
    {
        errorOpen = false;
        cautionOpen = false;
    }

    public void RestartGame()
    {
        StartCoroutine("LoadLevel", 1);
        //Application.LoadLevel(1);
    }

    public void StartStory()
    {
        curState = 0;
        StartCoroutine("LoadLevel", 2);
        //Application.LoadLevel(2);
    }

    public IEnumerator LoadLevel(int levelNum)
    {
        loading = true;

#if UNITY_WEBPLAYER
        while (Application.GetStreamProgressForLevel(levelNum) < 1)
            yield return false;
#else
        // webgl and editor
        yield return new WaitForEndOfFrame();
#endif

        loading = false;
        if(levelNum == 1)
        {
            SceneManager.LoadScene("StartScene");
        }
        Application.LoadLevel(levelNum);
    }
}