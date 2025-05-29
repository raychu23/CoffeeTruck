using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FinalResultsGraphController : MonoBehaviour
{
    public static FinalResultsGraphController instance;

    private List<Vector2> graphData;
    private float minMoney, maxMoney;

    public RectTransform graphAreaRect;

    public GameObject dataPointPrefab;

    public Text startingFundsTxt, finalFundsTxt;
    
    void Start()
    {
        instance = this;

        SetupGraph(SimArchive.This().archiveList);

        StartCoroutine("GenerateGraph");

        DrawMoneyText();
        DrawYLabel();
    }

    private void DrawMoneyText()
    {
        startingFundsTxt.text = FormatMoney(SimArchive.This().startingMoney);
        finalFundsTxt.text = FormatMoney(SimArchive.This().curMoney);
    }

    public List<Text> yLabelTxtList;

    private void DrawYLabel()
    {
        for (int i = 0; i < 7; i++)
        {
            yLabelTxtList[i].text = FormatMoneyNoCents(minMoney + (maxMoney - minMoney) * i / 6f);
        }
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
    }

    public IEnumerator GenerateGraph()
    {
        // delay to let loading screen to by
        yield return new WaitForSeconds(3f);

        // get our vector2 of data
        //graphData

        // figure out our graph start x, start y, width, and height
        float graphX = graphAreaRect.anchoredPosition.x - (graphAreaRect.sizeDelta.x / 2f);
        float graphY = graphAreaRect.anchoredPosition.y - (graphAreaRect.sizeDelta.y / 2f);
        float graphWidth = graphAreaRect.sizeDelta.x;
        float graphHeight = graphAreaRect.sizeDelta.y;

        // vector2 probably has all data between 0 and 1

        Vector2 dataPointPos;

        int count = graphData.Count;

        // for each piece of data
        foreach (Vector2 dataPoint in graphData)
        {
            // figure out the x and y position to place comparing graph dimensions to data
            dataPointPos = new Vector3(graphX + dataPoint.x * graphWidth + Screen.width / 2f, graphY + dataPoint.y * graphHeight + Screen.height / 2f, 0);

            // instantiate data point icon and add to list
            GameObject dataPointObj = GameObject.Instantiate(dataPointPrefab);

            // place correctly
            dataPointObj.transform.localPosition = dataPointPos;

            // set transform parent
            dataPointObj.transform.SetParent(graphAreaRect);

            yield return new WaitForSeconds(.05f);
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

    public void LoadGameData()
    {
        // load our game data url
        CalcUtil.OpenGameData();
    }

    public void RestartGame()
    {
        // load the first scene (story scene, not the "first" scene");
        //GameControlMaster.This().RestartGame();
        SceneManager.LoadScene("StartScene");
    }
}
