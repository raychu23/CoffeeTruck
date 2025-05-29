using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class DataRowPrefabHolder : MonoBehaviour
{
    [System.Serializable]
    public class DataGroup
    {
        public Text groupText;
        public GameObject groupObj;

        public void SetGroup(string value)
        {
            if (value == null)
            {
                groupObj.SetActive(false);
            }

            else
            {
                groupText.text = value;
                groupObj.SetActive(true);
            }
        }

        public void SetAlpha(float value)
        {
            groupText.color = new Color(groupText.color.r, groupText.color.g, groupText.color.b, value);
        }

        public void IncreaseAlpha(float increaseValue)
        {
            groupText.color = new Color(groupText.color.r, groupText.color.g, groupText.color.b, groupText.color.a + increaseValue);
        }
    }

    public List<DataGroup> dataGroups;
    public DataGroup saleGroup;

    public void SetText(string day, string location, string music, string price, string time, string sales)
    {
        // SET THIS IN EDITOR
        dataGroups[0].SetGroup(day);
        dataGroups[1].SetGroup(location);
        dataGroups[2].SetGroup(music);
        dataGroups[3].SetGroup(price);
        dataGroups[4].SetGroup(time);

        saleGroup.SetGroup(sales);
    }

    public List<Color> colorOptions;

    public void SetBarColor(string value, int catNum)
    {
        for (int i = 0; i < CategorySetup.cats[catNum].options.Count; i++)
        {
            if (value == CategorySetup.cats[catNum].options[i].databaseText)
            {
                SetBarColor(colorOptions[i]);
                return;
            }
        }
    }

    public void SetBarColor(Color barColor)
    {
        // stop turning off our color reset. optimization.
        StopCoroutine("PauseBeforeReset");

        foreach (DataGroup dGroup in dataGroups)
        {
            dGroup.groupObj.GetComponent<Image>().color = barColor;
        }

        saleGroup.groupObj.GetComponent<Image>().color = barColor;
    }

    public void ResetBarColor()
    {
        StartCoroutine("PauseBeforeReset");
    }

    private IEnumerator PauseBeforeReset()
    {
        yield return new WaitForSeconds(.5f);

        DisplayResults.This().ColorOurRows(-1);
    }

    public void FadeInSetupInfo(float fadeSpeed)
    {
        StartCoroutine("FadeOurInfoIn", fadeSpeed);
    }

    private IEnumerator FadeOurInfoIn(float fadeSpeed)
    {
        // set out info to transparent
        foreach (DataGroup data in dataGroups)
        {
            data.SetAlpha(0);
        }
        
        // fade our text in with the speed included
        while (dataGroups[0].groupText.color.a < 1)
        {
            foreach (DataGroup data in dataGroups)
            {
                data.IncreaseAlpha(Time.deltaTime * fadeSpeed);
            }

            yield return false;
        }

        // set the text to fully opaque once done;
        foreach (DataGroup data in dataGroups)
        {
            data.SetAlpha(1);
        }
    }

    public void FadeInSales(float delay, float speed)
    {
        List<float> parameters = new List<float>() {delay, speed};
        StartCoroutine("FadeOurSalesIn", parameters);
    }

    private IEnumerator FadeOurSalesIn(List<float> parameters)
    {
        // set our sales text to transparent
        saleGroup.SetAlpha(0);

        // wait for our delay
        yield return new WaitForSeconds(parameters[0]);

        // fade our sales text in with the speed included
        while (saleGroup.groupText.color.a < 1)
        {
            saleGroup.IncreaseAlpha(Time.deltaTime * parameters[1]);
            yield return false;
        }

        // set the sales to fully opaque once done;
        saleGroup.SetAlpha(1);
    }

    public void HideSales()
    {
        saleGroup.groupText.color = new Color(saleGroup.groupText.color.r, saleGroup.groupText.color.g, saleGroup.groupText.color.b, 0);
    }

    public void ColorOurBars(int dataNumber)
    {
        DisplayResults.This().ColorOurRows(dataNumber);
    }
}