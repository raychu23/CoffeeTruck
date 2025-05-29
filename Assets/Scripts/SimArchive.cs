using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimArchive : MonoBehaviour
{
    public class ArchiveEntry
    {
        public List<int> options;
        public int sales;
        public float costs;
        public float grossIncome;
        public float profit;
        public int temp;
        public int day;
        public float currentMoney;

        public ArchiveEntry(List<int> o, int s, float c, float g, float p, int t, int d, float cm)
        {
            options = o;
            sales = s;
            costs = c;
            grossIncome = g;
            profit = p;
            temp = t;
            day = d;
            currentMoney = cm;
        }
    }

    public class ArchiveAverage
    {
        //public string xLabel, yLabel;
        public int catNum, xOp, yOp;
        public float average, num;

        public ArchiveAverage(int x, int y, int cN)
        {
            //xLabel = x;
            //yLabel = y;
            xOp = x;
            yOp = y;
            catNum = cN;
            average = 0;
            num = 0;
        }

        public bool DoesThisAverageBelong(int x, int y, int cN)
        {
            //if (xLabel == x && yLabel == y)
            if (xOp == x && yOp == y && catNum == cN)
                return true;

            return false;
        }

        public void AddAverage(int sales)
        {
            average = ((average * num) + sales) / (num + 1f);
            num++;
        }
    }

    public List<ArchiveEntry> archiveList;
    public List<ArchiveAverage> archiveAverages;
    public float startingMoney;
    public float curMoney;
    public bool didWeFinish, overworked;
    public SimulationContainer simContainer;


    private static SimArchive obj;
    public static SimArchive This()
    {
        return obj;
    }

    void Start()
    {
        obj = this;
        Restart();
    }

    public void Restart()
    {
        archiveList = new List<ArchiveEntry>();
        startingMoney = 20000;
        curMoney = startingMoney;
        simContainer = null;
    }

    public void RunThroughSims(SimulationContainer _simContainer, int testsPerDay)
    {
        simContainer = _simContainer;
        startingMoney = curMoney;
        int day = 1;
        int counter = 0;
        //overworked = ow;

        foreach (Simulation sim in simContainer.simulations)
        {
            // add up money to current money
            curMoney += sim.profit;
            // store our sim in an archive
            archiveList.Add(new ArchiveEntry(sim.options, sim.sales, sim.costs, sim.grossIncome, sim.profit, sim.temp, day, curMoney));

            // if we are out of money, end it early
            if (curMoney <= 0)
            {
                Bankrupt();
                break;
            }
            
            // otherwise, continue to the next day
            counter++;
            if (counter == testsPerDay)
            {
                counter = 0;
                day++;
            }
        }
    }

    public void Bankrupt()
    {
        curMoney = 0;
        didWeFinish = false;
    }

    public void GenerateAverages(int catNum)
    {
        archiveAverages = new List<ArchiveAverage>();

        // setup our average class first
        for (int x = 0; x < CategorySetup.cats[catNum].options.Count; x++)
        {
            for (int y = 0; y < CategorySetup.cats[3].options.Count; y++)
            {
                archiveAverages.Add(new ArchiveAverage(x, y, catNum));
            }
        }

        // add each entry to its appropriate place
        foreach (ArchiveEntry entry in archiveList)
        {
            //Debug.Log(entry.sales + " - " + entry.options[catNum]);

            foreach (ArchiveAverage av in archiveAverages)
            {
                // if the entry's selected option in the category we are looking at (catNum)
                // is the same as the averageEntry's num, add it
                if (entry.options[catNum] == av.xOp && entry.options[3] == av.yOp)
                {
                    //Debug.Log(entry.options[catNum] + "  -  " + av.xOp);
                    av.AddAverage(entry.sales);
                    break;
                }
            }
        }
    }

    public string GetAverage(int x, int y, int cNum)
    {
        if (archiveAverages == null)
        {
            Debug.LogError("list not made");
            return "";
        }

        foreach (ArchiveAverage avg in archiveAverages)
        {
            if (avg.DoesThisAverageBelong(x, y, cNum))
            {
                if (avg.average == 0)
                    return "";

                return avg.average.ToString();
            }
        }

        return "";
    }
}