using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CalcUtil
{
    // apparently i never call this function?
    public static int FigureOutCategory(List<int> options, int numSim)//, bool randomized)
    {
        // if we only have one thing selected, FANTASTIC
        if (options.Count == 1)
        {
            return options[0];
        }

        // if nothing was active... uhh.. not sure yet
        else if (options.Count == 0)
        {
            // NOTHING WAS SELECTED? PANIC PANIC PANIC PANIC
            // this shouldn't happen anymore because we check this ahead of time.
            //GameControlMaster.This().ErrorWindowOpen("You didn't select an option in each category");
            return -1;
        }

        // we have a few things selected. lets deal with it and decide which one
        else
        {
            //if (randomized)
            //    return options[UnityEngine.Random.Range(0, options.Count)];

            //else
                return options[numSim % options.Count];
        }
    }

    public static bool AtleastOneThingSelected(List<Category> cats)
    {
        foreach (Category cat in cats)
        {
            if (cat.NumSelected() == 0)
                return false;
        }

        return true;
    }

    public static bool NoMoreThanThreeShift(List<Category> cats)
    {
        if(cats[3].NumSelected() > 3)
        {
            return false;
        }
        return true;
    }

    public static List<Simulation> Reshuffle(List<Simulation> intList)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < intList.Count; t++)
        {
            Simulation tmp = intList[t];
            int r = UnityEngine.Random.Range(t, intList.Count);
            intList[t] = intList[r];
            intList[r] = tmp;
        }

        return intList;
    }

    public static List<int> Reshuffle(List<int> intList)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < intList.Count; t++)
        {
            int tmp = intList[t];
            int r = UnityEngine.Random.Range(t, intList.Count);
            intList[t] = intList[r];
            intList[r] = tmp;
        }

        return intList;
    }

    public static List<int> ArrayToList(int[] array)
    {
        List<int> temp = new List<int>();
        foreach (int i in array)
        {
            temp.Add(i);
        }
        return temp;
    }

    public static List<int[]> Reshuffle(List<int[]> intList)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < intList.Count; t++)
        {
            int[] tmp = intList[t];
            int r = UnityEngine.Random.Range(t, intList.Count);
            intList[t] = intList[r];
            intList[r] = tmp;
        }

        return intList;
    }

    // Get normal (Gaussian) random sample with mean 0 and standard deviation 1
    public static double GetNormal()
    {
        //// Use Box-Muller algorithm
        double u1 = UnityEngine.Random.value;
        double u2 = UnityEngine.Random.value;
        double r = Math.Sqrt(-2.0 * Math.Log(u1));
        double theta = 2.0 * Math.PI * u2;
        return r * Math.Sin(theta);
    }

    public static double GetNormal(double mean, double standardDeviation)
    {
        if (standardDeviation <= 0.0)
        {
            string msg = string.Format("Shape must be positive. Received {0}.", standardDeviation);
            throw new ArgumentOutOfRangeException(msg);
        }
        return mean + standardDeviation * GetNormal();
    }

    private static void OpenTab(string url)
    {
        //Application.ExternalCall("OpenWindow", url, "Main Camera");
        Application.OpenURL(url);
    }

    public static void OpenGameData()
    {
        //GameControlMaster.This().loading = true;
        OpenTab("https://www.stat2games.sites.grinnell.edu/data/coffeetruck/coffeetruck.php");
    }
}