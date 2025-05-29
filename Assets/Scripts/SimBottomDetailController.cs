using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimBottomDetailController : MonoBehaviour {
    
    public List<GameObject> backgroundObjectList;
    public List<GameObject> priceObjectList;

	// Use this for initialization
	void Start () {
        Reconfigure();
	}

    public void Reconfigure()
    {
        SetBackground();
        SetPrice();
    }

    private void SetBackground()
    {
        // lazy method
        // turning off all options first, then turning on the first one we have selected
        foreach (GameObject bgObj in backgroundObjectList) bgObj.SetActive(false);

        for (int i = SimSetup.This().cats[0].options.Count - 1; i >= 0; i--)
        {
            if (SimSetup.This().cats[0].options[i].active)
            {
                backgroundObjectList[i].SetActive(true);
                return;
            }
        }

        // got here? we dont have anything selected. this is fine i guess
    }

    public void SetPrice()
    {
        // lazy method
        // turning off all options first, then turning on the first one we have selected
        foreach (GameObject priceObj in priceObjectList) priceObj.SetActive(false);

        for (int i = SimSetup.This().cats[2].options.Count - 1; i >= 0; i--)
        {
            if (SimSetup.This().cats[2].options[i].active)
            {
                priceObjectList[i].SetActive(true);
                return;
            }
        }

        // got here? we dont have anything selected. this is fine i guess
    }

    //private Texture2D GetLocationTex()
    //{
    //    for (int i = 0; i < SimSetup.This().cats[0].options.Count; i++)
    //    {
    //        if (SimSetup.This().cats[0].options[i].active)
    //            return locationImgs[i];
    //    }

    //    // dont have one selected? butts
    //    return locationImgs[0];
    //}
}