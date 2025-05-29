using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class SimSetup : MonoBehaviour
{
    public List<Category> cats;
    public int designType;
    public string playerName;
    public string groupName;
    public bool saveData;
    public int totalTests;

    private static SimSetup obj;
    public static SimSetup This() { return obj; }
    public static SimulationContainer simContainer;

    private void Start()
    {
        DontDestroyOnLoad(this);
        obj = this;
        Setup();
    }

    public void Setup()
    {
        CreateCategories();
        designType = 0;
        saveData = true;
       // playerName = "";
       // groupName = "";
        totalTests = 0;
    }

    public void StartSim(int numSims, int curDesignType, bool overworked)
    {
        // need to add variables in here
        int testsPerDay = cats[3].NumSelected();
        designType = curDesignType;
        simContainer = new SimulationContainer(cats, numSims, designType, overworked);

        SimArchive.This().RunThroughSims(simContainer, testsPerDay);

        //// upload our sim data if we were told to upload it
        //if (SimSetup.This().saveData)
        //    SimSetup.This().Upload();

        //if (curState == 2)
        //{
        //    UI_Result.This().Setup();
        //    SimSetup.This().totalTests++;
        //}
    }

    public void ClearSimData()
    {
        SimArchive.This().Restart();
    }

    public int NumberOfDays(List<Category> cats, int designType, int numSims)
    {
        int num = 1;

        // if random, number of days = number of simulations

        // if not random, number of days = each num options * itself * number of simulations
        if (designType != 2)
        {
            num *= cats[0].NumSelected();
            num *= cats[1].NumSelected();
            num *= cats[2].NumSelected();
            num *= cats[4].NumSelected();
        }

        return num * numSims;
    }

    public string NumberOfDaysStr(List<Category> cats, int designType, string numSims)
    {
        if (numSims == "") numSims = "0";
        int num = int.Parse(numSims);
        return NumberOfDays(cats, designType, num).ToString();
    }

    private void CreateCategories()
    {
        cats = CategorySetup.cats;
    }

    private string DesignTypeStr(int dt)
    {
        if (dt == 2) return "Unbalanced";
        if (dt == 1) return "Sequential";
        return "Random Sample";
    }

    // call this when we are uploading a sim with it's own design type
    public void Upload()
    {
        Upload(false);
    }
    public void Upload(bool designTypeAsBusinessPlan)
    {
        StartCoroutine("UploadDataRoutine", designTypeAsBusinessPlan);
    }

    public IEnumerator UploadDataRoutine(bool designTypeAsBusinessPlan)
    {
        if (!designTypeAsBusinessPlan)
        {
            int gameNum = 0;

            WWWForm numForm = new WWWForm();
            numForm.AddField("PlayerID", TopBarSaveController.instance.playerName);
            numForm.AddField("GroupID", TopBarSaveController.instance.groupName);

            //Fetch game number
            using (var www = UnityWebRequest.Post("https://stat2games.sites.grinnell.edu/php/getstatisticallygroundednum.php", numForm))
            {
                //Debug.Log("starting fetching game num");
                yield return www.SendWebRequest();
                //Debug.Log("fetched");
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log("Fetching game number failed.  Error message: ");
                }
                else
                {
                    gameNum = int.Parse(www.downloadHandler.text);
                    Debug.Log("gamenum: " + gameNum);
                }
            }

            foreach (SimArchive.ArchiveEntry entry in SimArchive.This().archiveList)
            {
                WWWForm form = new WWWForm();
                form.AddField("GameNum", gameNum);
                form.AddField("PlayerID", TopBarSaveController.instance.playerName);
                form.AddField("GroupID", TopBarSaveController.instance.groupName);
                if (designTypeAsBusinessPlan)
                    form.AddField("DesignType", "Business Plan");
                else
                    form.AddField("DesignType", DesignTypeStr(designType));
                form.AddField("TotalTests", totalTests);

                form.AddField("Day", entry.day);
                form.AddField("Location", CategorySetup.cats[0].options[entry.options[0]].databaseText);
                form.AddField("Music", CategorySetup.cats[1].options[entry.options[1]].databaseText);
                form.AddField("Price", CategorySetup.cats[2].options[entry.options[2]].databaseText);
                form.AddField("TimeOfDay", CategorySetup.cats[3].options[entry.options[3]].databaseText);
                form.AddField("Weather", CategorySetup.cats[4].options[entry.options[4]].databaseText);
                form.AddField("Temperature", entry.temp);
                form.AddField("Sales", entry.sales);
                form.AddField("GrossIncome", entry.grossIncome.ToString());
                form.AddField("Costs", entry.costs.ToString());
                form.AddField("Profit", entry.profit.ToString());
                form.AddField("CurrentMoney", entry.currentMoney.ToString());

                //form.AddField("GameNum", gameNum);
                //form.AddField("PlayerID", data.dataValues[0].Value.ToString());
                //form.AddField("GroupID", data.dataValues[1].Value.ToString());
                //form.AddField("DesignType", data.dataValues[2].Value.ToString());
                //form.AddField("TotalTests", int.Parse(data.dataValues[3].Value.ToString()));
                //form.AddField("Day", int.Parse(data.dataValues[4].Value.ToString()));
                //form.AddField("Location", data.dataValues[5].Value.ToString());
                //form.AddField("Music", data.dataValues[6].Value.ToString());
                //form.AddField("Price", data.dataValues[7].Value.ToString());
                //form.AddField("TimeOfDay", data.dataValues[8].Value.ToString());
                //form.AddField("Weather", data.dataValues[9].Value.ToString());
                //form.AddField("Temperature", int.Parse(data.dataValues[10].Value.ToString()));
                //form.AddField("Sales", int.Parse(data.dataValues[11].Value.ToString()));
                //form.AddField("GrossIncome", data.dataValues[12].Value.ToString());
                //form.AddField("Costs", data.dataValues[13].Value.ToString());
                //form.AddField("Profit", data.dataValues[14].Value.ToString());
                //form.AddField("CurrentMoney", data.dataValues[15].Value.ToString());



                //foreach (DataContainer.KeyValue kv in data.dataValues) {
                //             if (kv.Value is List<DataContainer>) {
                //                 List<DataContainer> dclist = (List<DataContainer>)kv.Value;
                //                 foreach (DataContainer dc in dclist) {
                //                     form.AddField("subcols[]", kv.Key);
                //                     form.AddField("subentry[]", dc.ToString());
                //                 }
                //             }
                //             else
                //             {
                //		form.AddField("subcols[]", kv.Key);
                //		form.AddField("subentry[]", kv.Value.ToString());

                //	}
                //         }
                using (var www = UnityWebRequest.Post("https://stat2games.sites.grinnell.edu/php/sendstatisticallygroundedgameinfo.php", form))
                {

                    yield return www.SendWebRequest();

                    if (www.downloadHandler.text == "0")
                    {
                        Debug.Log("Player data created successfully.");
                    }
                    else
                    {
                        Debug.Log("Player data creation failed. Error # " + www.downloadHandler.text);
                    }
                }
            }
        }
        else
        {
            int gameNum = 0;

            WWWForm numForm = new WWWForm();
            numForm.AddField("PlayerID", TopBarSaveController.instance.playerName);
            numForm.AddField("GroupID", TopBarSaveController.instance.groupName);

            //Fetch game number
            using (var www = UnityWebRequest.Post("https://stat2games.sites.grinnell.edu/php/getstatisticallygroundedbusinessnum.php", numForm))
            {
                //Debug.Log("starting fetching game num");
                yield return www.SendWebRequest();
                //Debug.Log("fetched");
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log("Fetching game number failed.  Error message: ");
                }
                else
                {
                    gameNum = int.Parse(www.downloadHandler.text);
                    Debug.Log("gamenum: " + gameNum);
                }
            }
            foreach (SimArchive.ArchiveEntry entry in SimArchive.This().archiveList)
            {
                WWWForm form = new WWWForm();
                form.AddField("GameNum", gameNum);
                form.AddField("PlayerID", TopBarSaveController.instance.playerName);
                form.AddField("GroupID", TopBarSaveController.instance.groupName);
                if (designTypeAsBusinessPlan)
                    form.AddField("DesignType", "Business Plan");
                else
                    form.AddField("DesignType", DesignTypeStr(designType));
                form.AddField("TotalTests", totalTests);

                form.AddField("Day", entry.day);
                form.AddField("Location", CategorySetup.cats[0].options[entry.options[0]].databaseText);
                form.AddField("Music", CategorySetup.cats[1].options[entry.options[1]].databaseText);
                form.AddField("Price", CategorySetup.cats[2].options[entry.options[2]].databaseText);
                form.AddField("TimeOfDay", CategorySetup.cats[3].options[entry.options[3]].databaseText);
                form.AddField("Weather", CategorySetup.cats[4].options[entry.options[4]].databaseText);
                form.AddField("Temperature", entry.temp);
                form.AddField("Sales", entry.sales);
                form.AddField("GrossIncome", entry.grossIncome.ToString());
                form.AddField("Costs", entry.costs.ToString());
                form.AddField("Profit", entry.profit.ToString());
                form.AddField("CurrentMoney", entry.currentMoney.ToString());
                using (var www = UnityWebRequest.Post("https://stat2games.sites.grinnell.edu/php/sendstatisticallygroundedbusinessplaninfo.php", form))
                {

                    yield return www.SendWebRequest();

                    if (www.downloadHandler.text == "0")
                    {
                        Debug.Log("Player data created successfully.");
                    }
                    else
                    {
                        Debug.Log("Player data creation failed. Error # " + www.downloadHandler.text);
                    }
                }
            }

            //WWW servReq = new WWW(URLlocations.instance.GetServiceUrl(), form);

            //      StartCoroutine(WaitForRequest(servReq));
        }
    }
    

    //public void Upload(bool designTypeAsBusinessPlan)
    //{


    //    // add strings to datacontainer
    //    //dc.addPair("PlayerID", playerName);
    //    //dc.addPair("GroupID", groupName);
    //    //if (designTypeAsBusinessPlan)
    //    //    dc.addPair("DesignType", "Business Plan");
    //    //else
    //    //    dc.addPair("DesignType", DesignTypeStr(designType));
    //    //dc.addPair("TotalTests", totalTests);

    //    //List<DataContainer> salesData = new List<DataContainer>();

    //    //foreach (SimArchive.ArchiveEntry entry in SimArchive.This().archiveList)
    //    //{
    //    //    DataContainer innerdc = new DataContainer();
    //    //    innerdc.addPair("Day", entry.day);
    //    //    innerdc.addPair("Location", CategorySetup.cats[0].options[entry.options[0]].databaseText);
    //    //    innerdc.addPair("Music", CategorySetup.cats[1].options[entry.options[1]].databaseText);
    //    //    innerdc.addPair("Price", CategorySetup.cats[2].options[entry.options[2]].databaseText);
    //    //    innerdc.addPair("TimeOfDay", CategorySetup.cats[3].options[entry.options[3]].databaseText);
    //    //    innerdc.addPair("Weather", CategorySetup.cats[4].options[entry.options[4]].databaseText);
    //    //    innerdc.addPair("Temperature", entry.temp);
    //    //    innerdc.addPair("Sales", entry.sales);
    //    //    innerdc.addPair("GrossIncome", entry.grossIncome);
    //    //    innerdc.addPair("Costs", entry.costs);
    //    //    innerdc.addPair("Profit", entry.profit);
    //    //    innerdc.addPair("CurrentMoney", entry.currentMoney);
    //    //    salesData.Add(innerdc);
    //    //}

    //    //dc.addPair("Sales Data", salesData);

    //    //DataUploader du = GetComponent<DataUploader>();
    //    //du.UploadData(dc);
    //    int i = 0;
    //    foreach (SimArchive.ArchiveEntry entry in SimArchive.This().archiveList)
    //    {
    //        DataContainer dc = new DataContainer();
    //        Debug.Log("entry day: " + entry.day);
    //        Debug.Log("sales: " + entry.sales);
    //        dc.addPair("PlayerID", playerName);
    //        dc.addPair("GroupID", groupName);
    //        if (designTypeAsBusinessPlan)
    //            dc.addPair("DesignType", "Business Plan");
    //        else
    //            dc.addPair("DesignType", DesignTypeStr(designType));
    //        dc.addPair("TotalTests", totalTests);

    //        dc.addPair("Day", entry.day);
    //        dc.addPair("Location", CategorySetup.cats[0].options[entry.options[0]].databaseText);
    //        dc.addPair("Music", CategorySetup.cats[1].options[entry.options[1]].databaseText);
    //        dc.addPair("Price", CategorySetup.cats[2].options[entry.options[2]].databaseText);
    //        dc.addPair("TimeOfDay", CategorySetup.cats[3].options[entry.options[3]].databaseText);
    //        dc.addPair("Weather", CategorySetup.cats[4].options[entry.options[4]].databaseText);
    //        dc.addPair("Temperature", entry.temp);
    //        dc.addPair("Sales", entry.sales);
    //        Debug.Log("dc sales: " + dc.dataValues[11].Value.ToString());
    //        dc.addPair("GrossIncome", entry.grossIncome);
    //        dc.addPair("Costs", entry.costs);
    //        dc.addPair("Profit", entry.profit);
    //        dc.addPair("CurrentMoney", entry.currentMoney);
    //        if(i == 0)
    //        {
    //            dc.addPair("FirstData", 0);
    //            i++;
    //        }
    //        else { dc.addPair("FirstData", 1); }

    //        DataUploader du = GetComponent<DataUploader>();
    //        du.UploadData(dc);
    //    }
        /* How to use:
         * Add this script to a game object
         * Build a DataContainer object by adding key/value pairs.
         * Values may be strings or a list of DataContainers
         * 
                    DataContainer dc = new DataContainer();
                    dc.addPair("Player Name", "Player One");
                    dc.addPair("Group Name", "Team Alpha");

                    List<DataContainer> dclist = new List<DataContainer>();
                    DataContainer innerdc = new DataContainer();
                    innerdc.addPair("Weather", "Rainy");
                    innerdc.addPair("Infostuff", "$600");
                    innerdc.addPair("Hey", "Listen");
                    dclist.Add(innerdc);

                    innerdc = new DataContainer();
                    innerdc.addPair("Weather", "Moar Rain");
                    innerdc.addPair("Infostuff", "$1000");
                    innerdc.addPair("Hey", "HEY");
                    dclist.Add(innerdc);
			
                    dc.addPair("DataPoints", dclist);
         * 
         * Then upload the data to the server:
         * DataUploader du = GetComponent<DataUploader>();
         * du.UploadData(dc);
         */

    

    public List<List<string>> GetMostRecentSimData()
    {
        // we want to grab the data we just calculated

        // do we assume the data was already calculated? we should probably check

        List<List<string>> totalDataCollected = new List<List<string>>();

        // for each entry we have
        foreach (SimArchive.ArchiveEntry entry in SimArchive.This().archiveList)
        {
            List<string> entryList = new List<string>();

            // add each piece of data we have
            entryList.Add(entry.day.ToString());
            entryList.Add(CategorySetup.cats[0].options[entry.options[0]].databaseText);
            entryList.Add(CategorySetup.cats[1].options[entry.options[1]].databaseText);
            entryList.Add(CategorySetup.cats[2].options[entry.options[2]].databaseText);
            entryList.Add(CategorySetup.cats[3].options[entry.options[3]].databaseText);
            entryList.Add(CategorySetup.cats[4].options[entry.options[4]].databaseText);
            entryList.Add(entry.temp.ToString());
            entryList.Add(entry.sales.ToString());
            entryList.Add(entry.grossIncome.ToString());
            entryList.Add(entry.costs.ToString());
            entryList.Add(entry.profit.ToString());
            entryList.Add(entry.currentMoney.ToString());

            totalDataCollected.Add(entryList);
        }

        if (totalDataCollected.Count == 0)
        {
            Debug.LogWarning("We wanted sim data but we didnt have any. What is the deal?");
        }

        return totalDataCollected;
    }

    public List<string> GetMostRecentCategories()
    {
        return new List<string>() {
            "Day",
            "Location",
            "Music",
            "Price",
            "Time of Day",
            "Weather",
            "Temperature",
            "Sales",
            "Gross Income",
            "Costs",
            "Profit",
            "Current Money"
        };
    }

    public List<int> GetNumberOfVariablesPerCategory()
    {
        return SimArchive.This().simContainer.numOptions;
    }

    public int GetMostRecentDataType()
    {
        return SimArchive.This().simContainer.designType;
    }

    public void FakeUpload()
    {
        DataContainer dc = new DataContainer();

        // add strings to datacontainer
        Debug.Log("Player Name: " + playerName);
        Debug.Log("Group Name: " + groupName);
        Debug.Log("Design Type: " + DesignTypeStr(designType));
        Debug.Log("Total Tests Done: " + totalTests);
        Debug.Log("Starting Money: " + SimArchive.This().archiveList[0].currentMoney);

        //foreach (SimArchive.ArchiveEntry entry in SimArchive.This().archiveList)
        //{
        //    DataContainer innerdc = new DataContainer();
        //    innerdc.addPair("Day", entry.day);
        //    innerdc.addPair("Location", CategorySetup.cats[0].options[entry.options[0]].databaseText);
        //    innerdc.addPair("Music", CategorySetup.cats[1].options[entry.options[1]].databaseText);
        //    innerdc.addPair("Price", CategorySetup.cats[2].options[entry.options[2]].databaseText);
        //    innerdc.addPair("Time of Day", CategorySetup.cats[3].options[entry.options[3]].databaseText);
        //    innerdc.addPair("Weather", CategorySetup.cats[4].options[entry.options[4]].databaseText);
        //    innerdc.addPair("Temperature", entry.temp);
        //    innerdc.addPair("Sales", entry.sales);
        //    innerdc.addPair("Gross Income", entry.grossIncome);
        //    innerdc.addPair("Costs", entry.costs);
        //    innerdc.addPair("Profit", entry.profit);
        //    innerdc.addPair("Current Money", entry.currentMoney);
        //    salesData.Add(innerdc);
        //}

        //dc.addPair("Sales Data", salesData);

        //DataUploader du = GetComponent<DataUploader>();
        //du.UploadData(dc);
    }
}