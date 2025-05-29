using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DisplayResults : MonoBehaviour {

    public static DisplayResults This() { return obj; }
    private static DisplayResults obj;

    private List<GameObject> curPrefabs;
    public GameObject titlePrefab;
    public GameObject canvasObject;
    public GameObject rowPrefab;
    public GameObject rowHolderInScene;
    public GameObject topRowHolderInScene;
    public GameObject colorizeInstructionText;

    public GameObject nextDayButton, allDaysButton, continueButton;

    private int curDataShown;

    void Start()
    {
        obj = this;
        curPrefabs = new List<GameObject>();
        canvasObject.SetActive(false);
        nextDayButton.GetComponent<Button>().interactable = true;
        allDaysButton.GetComponent<Button>().interactable = true;
        continueButton.GetComponent<Button>().interactable = false;

        SetupData();
    }

    public void SetupData()
    {
        SetupData(
            SimSetup.This().GetMostRecentSimData(),
            SimSetup.This().GetMostRecentCategories(),
            SimSetup.This().GetNumberOfVariablesPerCategory(),
            SimSetup.This().GetMostRecentDataType()
        );
    }

    public void SetupData(List<List<string>> dataList, List<string> categoryList, List<int> numberOfVariablesPerCategory, int dataType)
    {
        // if this is our first piece of data, enable our canvas
        canvasObject.SetActive(true);
		ResetScrollPosition();

        string temp = "";
        float count = 0;

        // turn on coloring instruction if we have enough categories to need it
        colorizeInstructionText.SetActive(false);
        foreach (int num in numberOfVariablesPerCategory)
            if (num > 1) colorizeInstructionText.SetActive(true);

        // our titles first
        //GameObject tempObj1 = (GameObject)Instantiate(rowPrefab);
        //curPrefabs.Add(tempObj1);
        //tempObj1.transform.SetParent(topRowHolderInScene.transform);
		//tempObj1.transform.localScale = new Vector3(1,1,1);
        DataRowPrefabHolder titleHolderScript = titlePrefab.GetComponent<DataRowPrefabHolder>();
        titleHolderScript.SetText(
            "Day",
            numberOfVariablesPerCategory[0] > 1 ? "Location" : null,
            numberOfVariablesPerCategory[1] > 1 ? "Music" : null,
            numberOfVariablesPerCategory[2] > 1 ? "Price" : null,
            numberOfVariablesPerCategory[3] > 1 ? "Time" : null,
            "Sales");
        titleHolderScript.FadeInSetupInfo(2f);
        //titleHolderScript.SetBarColor(new Color(1, 1, 1, 1));

        foreach (List<string> data in dataList)
        {
            // make sure our lists are the same length. sanity reasons
            if (data.Count != categoryList.Count)
            {
                Debug.Log("data and categories do not line up!");
                break;
            }

            GameObject tempObj = (GameObject)Instantiate(rowPrefab);
            curPrefabs.Add(tempObj);
            tempObj.transform.SetParent(rowHolderInScene.transform);
			tempObj.transform.localScale = new Vector3(1,1,1);
            DataRowPrefabHolder holderScript = tempObj.GetComponent<DataRowPrefabHolder>();
            holderScript.SetText(
                data[0],
                GetDataIfVaried(numberOfVariablesPerCategory[0], data, 1),
                GetDataIfVaried(numberOfVariablesPerCategory[1], data, 2),
                GetDataIfVaried(numberOfVariablesPerCategory[2], data, 3),
                GetDataIfVaried(numberOfVariablesPerCategory[3], data, 4),
                data[7]);
            //holderScript.FadeInSales(dataList.Count * (1 / (count + 1)), 2f);
            // we want our text to be transparent
            holderScript.HideSales();
            holderScript.FadeInSetupInfo(2f);
            //holderScript.SetBarColor(data[1]);
            //holderScript.SetBarColor(count % 2 == 0 ? new Color(.85f, .85f, .85f, 1) : new Color(.82f, .94f, .99f));

            count++;
        }

        curDataShown = 0;

        // lastly, set our text at the top
        SetupOurInstructions(numberOfVariablesPerCategory, dataType, dataList);
    }

    public void ColorOurRows(int dataNumberToColor)
    {
        //// if dataNumberToColor = -1, turn everything grey
        //if (dataNumberToColor == -1)
        //    foreach (GameObject 

        // otherwise

        //Debug.Log("Coloring data: " + dataNumberToColor);

        List<List<string>> data = SimSetup.This().GetMostRecentSimData();

        // for each prefab
        for (int dataNumber = 0; dataNumber < curPrefabs.Count; dataNumber++)
        {
            // get script for that prefab
            DataRowPrefabHolder holderScript = curPrefabs[dataNumber].GetComponent<DataRowPrefabHolder>();
            
            // get the data associated to that prefab and color it
            holderScript.SetBarColor(data[dataNumber][dataNumberToColor + 1], dataNumberToColor);
        }
    }

    public Text instructionsText;

    private void SetupOurInstructions(List<int> numberOfVariablesPerCategory, int dataType, List<List<string>> dataList)
    {
        string instructionString = "";

        List<string> listOfCategories = GetListOfCategories(numberOfVariablesPerCategory);

        for (int num = 0; num < listOfCategories.Count; num++)
        {
            // do this our first time through only
            if (num == 0) instructionString += "You selected ";

            // add each of our chosen things to the instruction string

            // if this was our only category, just do this

            // if this was not our only category and not our last, and an "and"
            if (listOfCategories.Count > 1 && num + 1 == listOfCategories.Count)
                instructionString += " and ";

            instructionString += AddColorToText(listOfCategories[num]);

            // if this was not our only category but not the last or second to last, add a comma and a space
            if (listOfCategories.Count > 1 && num + 2 < listOfCategories.Count)
                instructionString += ", ";

            // do this if this is our last one!
            if (num+1 == listOfCategories.Count) instructionString += ". ";
        }

        // random sample
        if (dataType == 0)
        {
            string correctWording = GetCorrectWording(numberOfVariablesPerCategory);

            if (correctWording != "empty")
            {
                instructionString += "Each of these " +
                    AddColorToText(GetTotalCombinationsCount(numberOfVariablesPerCategory)) + " " +
                    AddColorToText(correctWording) +
                    " will be randomly allocated to a day in your study. ";
            }
            
            instructionString += "Since there are " +
                AddColorToText(GetTotalSimulations(dataList, numberOfVariablesPerCategory)) +
                " simulations, a total of " +
                AddColorToText(GetTotalDays(dataList)) +
                " days (" +
                AddColorToText(GetTotalShifts(dataList, numberOfVariablesPerCategory)) +
                " shifts) will be used to collect data.";
        }

        // sequential
        else if (dataType == 1)
        {
            instructionString += "Each of these " +
                AddColorToText(GetTotalCombinationsCount(numberOfVariablesPerCategory)) + " " +
                AddColorToText(GetCorrectWording(numberOfVariablesPerCategory)) + 
                " will be assigned in order to a day in your study. Since there are " +
                AddColorToText(GetTotalSimulations(dataList, numberOfVariablesPerCategory)) +
                " simulations, a total of " +
                AddColorToText(GetTotalDays(dataList)) +
                " days (" +
                AddColorToText(GetTotalShifts(dataList, numberOfVariablesPerCategory)) +
                " shifts) will be used to collect data.";
        }

        // Unbalanced
        else if (dataType == 2)
        {
            instructionString += "Each day (and shift) that the study is conducted, one of the possible combinations is randomly selected. In the unbalanced design, there is no guarantee that there will be equal sample sizes.";
        }

        StartCoroutine("FadeInstructionsIn");
        instructionsText.text = instructionString;
    }

    private string AddColorToText(int textAsInt)
    {
        return AddColorToText(textAsInt.ToString());
    }

    private string AddColorToText(string text)
    {
        return "<b><color=#f8c54c>" + text + "</color></b>";
    }

    private IEnumerator FadeInstructionsIn()
    {
        instructionsText.transform.localPosition += new Vector3(10f, 0, 0);

        instructionsText.color = new Color(1, 1, 1, 0);

        while (instructionsText.color.a < 1)
        {
            instructionsText.color = new Color(1, 1, 1, instructionsText.color.a + Time.deltaTime);
            instructionsText.transform.localPosition -= new Vector3(Time.deltaTime * 10f, 0, 0);
            yield return false;
        }

        instructionsText.color = new Color(1, 1, 1, 1);
    }

    private List<string> GetListOfCategories(List<int> numberOfVariablesPerCategory)
    {
        List<string> foo = new List<string>();

        for (int i = 0; i < 4; i++)
        {
            if (numberOfVariablesPerCategory[i] > 1)
            {
                foo.Add(numberOfVariablesPerCategory[i] + " " + GetName(i, numberOfVariablesPerCategory[i]));
            }
        }

        return foo;
    }

    private string GetName(int categoryNumber, int totalNumber)
    {
        if (categoryNumber == 0)
        {
            return (totalNumber == 1 ? "location" : "locations");
        }

        if (categoryNumber == 1)
        {
            return (totalNumber == 1 ? "type of music" : "types of music");
        }

        if (categoryNumber == 2)
        {
            return (totalNumber == 1 ? "price" : "prices");
        }

        if (categoryNumber == 3)
        {
            return (totalNumber == 1 ? "shift" : "shifts");
        }

        Debug.LogWarning("We got here and shouldnt have with number " + categoryNumber);
        return "N/A";
    }

    private int GetTotalCombinationsCount(List<int> numberOfVariablesPerCategory)
    {
        int num = 1;
        foreach (int foo in numberOfVariablesPerCategory) num *= foo;

        return num;
    }

    private int GetTotalSimulations(List<List<string>> dataList, List<int> numberOfVariablesPerCategory)
    {
        // getting lazier. Working backwards
        return (dataList.Count / GetTotalCombinationsCount(numberOfVariablesPerCategory));
    }

    private int GetTotalDays(List<List<string>> dataList)
    {
        int largestDay = 0;

        foreach (List<string> strList in dataList)
        {
            if (largestDay < int.Parse(strList[0]))
                largestDay = int.Parse(strList[0]);
        }

        return largestDay;
    }

    private int GetTotalShifts(List<List<string>> dataList, List<int> numberOfVariablesPerCategory)
    {
        // lazy way to do this
        return GetTotalSimulations(dataList, numberOfVariablesPerCategory) * GetTotalCombinationsCount(numberOfVariablesPerCategory);
    }

    private string GetCorrectWording(List<int> numberOfVariablesPerCategory)
    {
        string resultWording = "empty";

        for (int i = 0; i < numberOfVariablesPerCategory.Count; i++)
        {
            resultWording = WordingCheck(resultWording, numberOfVariablesPerCategory[i], i);
        }

        return resultWording;
    }

    private string WordingCheck(string currentWording, int numberOfVariables, int position)
    {
        List<string> wording = new List<string>() { "locations", "music types", "prices", "shifts" };

        // if our number of variables is 1, return the string
        if (numberOfVariables == 1) return currentWording;

        // if our number of variables is more than 1
        else if (numberOfVariables > 1)
        {
            // if the word is "empty", set it to the right word
            if (currentWording == "empty") currentWording = wording[position];
            // if the word wasn't "empty"
            else currentWording = "combinations";
        }

        // return our word!
        return currentWording;
    }

    private string GetDataIfVaried(int dataCount, List<string> dataList, int position)
    {
        if (dataCount > 1)
            return dataList[position];

        return null;
    }

    private List<bool> WhereDoWeHaveVariedData(List<List<string>> dataList)
    {
        List<string> initialValues = dataList[0];

        List<bool> differenceList = new List<bool>();

        for (int totalCategories = 0; totalCategories < initialValues.Count; totalCategories++)
            differenceList.Add(false);

        // for all of our data sets
        foreach (List<string> values in dataList)
        {
            // compare each value to our initialValues
            for (int i = 0; i < values.Count; i++)
            {
                // if any of the values are not equal to the initial values,
                if (values[i] != initialValues[i])
                    // set difference list for that spot to true
                    differenceList[i] = true;
            }
        }

        // return our list
        return differenceList;
    }

    public void GetNextSalesData()
    {
        // sanity check
        if (curDataShown < curPrefabs.Count)
            curPrefabs[curDataShown].GetComponent<DataRowPrefabHolder>().FadeInSales(0, 2f);

        ScrollToPos(curDataShown, curPrefabs.Count);

        curDataShown++;

        if (curDataShown >= curPrefabs.Count)
        {
            TurnOnContinueButton();
        }
    }

    private void TurnOnContinueButton()
    {
        nextDayButton.GetComponent<Button>().interactable = false;
        allDaysButton.GetComponent<Button>().interactable = false;
        continueButton.GetComponent<Button>().interactable = true;
    }

    public Scrollbar windowScroll;
    private void ScrollToPos(int positionToScrollTo, int totalPiecesOfData)
    {
		float setToValue = 1f;
		int minMaxNumber = 5;

		// if value position is 5 or less, scroll to top
		if (positionToScrollTo <= minMaxNumber)
		{
			setToValue = 1f;
		}

		// if value position is 5 or less from bottom, scroll to bottom
		else if (positionToScrollTo >= totalPiecesOfData - minMaxNumber)
		{
			setToValue = 0f;
		}

		else
		{
			// dont want to divide by zero!
			if (totalPiecesOfData != 0)
				setToValue = 1 - ((float)(positionToScrollTo - minMaxNumber) / (float)(totalPiecesOfData - (minMaxNumber * 2f)));
		}

		//windowScroll.value = setToValue;
		StopCoroutine("ScrollToPosCoroutine");
		StartCoroutine("ScrollToPosCoroutine", setToValue);
    }

	public void ResetScrollPosition()
	{
		StopCoroutine("ScrollToPosCoroutine");
		StopCoroutine("GetAllSalesDataCoroutine");
		windowScroll.value = 1;
	}

	private IEnumerator ScrollToPosCoroutine(float targetPos)
	{
		while (Mathf.Abs(windowScroll.value - targetPos) > .001f)
		{
			windowScroll.value = Mathf.Lerp(windowScroll.value, targetPos, Time.deltaTime * 5f);

			yield return false;
		}

		windowScroll.value = targetPos;
	}

    public void GetAllSalesData()
    {
		StartCoroutine("GetAllSalesDataCoroutine");
	}

	private IEnumerator GetAllSalesDataCoroutine()
	{
		while (curDataShown < curPrefabs.Count)
		{
			GetNextSalesData();

			yield return new WaitForSeconds(.2f);
		}
    }

    public void Continue()
    {
        // JEFFNOTE: unncesesary. handled by simscreenmanager
        //// empty our objects
        //foreach (GameObject prefab in curPrefabs)
        //{
        //    Destroy(prefab);
        //}

        //// reset our buttons
        //nextDayButton.GetComponent<Button>().interactable = true;
        //allDaysButton.GetComponent<Button>().interactable = true;
        //continueButton.GetComponent<Button>().interactable = false;

        //curPrefabs = new List<GameObject>();

        // report that we are continuing

        //GameControlMaster.This().SimFinished();

        SimScreenManager.instance.ShowSimAverageResults();
    }

    public void RestartTrial()
    {
        SimScreenManager.instance.ShowSimSettings();
    }
}