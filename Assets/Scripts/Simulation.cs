using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Simulation
{
    public List<int> options;
    public int sales;
    public float costs;
    public float grossIncome;
    public float profit;
    public int temp;

    public Simulation(List<int> ops, int numTestsPerDay)
    {
        // SIMULATION STUFF HAPPENS HERE. this is where the magic happens
        options = ops;

        float fSales = CalcSales(ops[1], ops[3], ops[0]); // calculate original sales
        fSales = ModifyBasedOnPrice(fSales, ops[2]); // adjust based on price

        // figure out daily temp and how it affects sales
        temp = Mathf.RoundToInt((float)CalcUtil.GetNormal(60, 5));
        fSales -= ((temp - 65) / 2f);

        // this part uses the weather. USE THE WEATHER!
        // note: weather = ops[4]

        // lastly, add some randomness
        //float randomness = (float)CalcUtil.GetNormal(0, 10);
        fSales = (float)CalcUtil.GetNormal(fSales, 10); //+= randomness;
        
        // modification to lower sales
        fSales -= 20;

        // if sales are lower than zero, cap it out at zero
        fSales = Mathf.Max(fSales, 0);
        
        sales = Mathf.RoundToInt(fSales);
        costs = (300 / numTestsPerDay) + sales * .5f;
        grossIncome = sales * (ops[2] + 2);
        profit = grossIncome - costs;
    }

    private float CalcSales(int music, int time, int location)
    {
        if (music == 0) // no music
        {
            if (time == 0) // morning
            {
                if (location == 0) return 100; // business
                else if (location == 1) return 40; // zoo
                else if (location == 2) return 80; // park
                else return 60; // hall
            }
            else if (time == 1) // lunch
            {
                if (location == 0) return 100; // business
                else if (location == 1) return 90; // zoo
                else if (location == 2) return 90; // park
                else return 110; // hall
            }
            else if (time == 2) // afternoon
            {
                if (location == 0) return 100; // business
                else if (location == 1) return 90; // zoo
                else if (location == 2) return 110; // park
                else return 90; // hall
            }
            else // evening
            {
                if (location == 0) return 50; // business
                else if (location == 1) return 70; // zoo
                else if (location == 2) return 90; // park
                else return 70; // hall
            }
        }

        else if (music == 1) // hiphop
        {
            if (time == 0) // morning
            {
                if (location == 0) return 90; // business
                else if (location == 1) return 40; // zoo
                else if (location == 2) return 90; // park
                else return 60; // hall
            }
            else if (time == 1) // lunch
            {
                if (location == 0) return 90; // business
                else if (location == 1) return 90; // zoo
                else if (location == 2) return 100; // park
                else return 110; // hall
            }
            else if (time == 2) // afternoon
            {
                if (location == 0) return 90; // business
                else if (location == 1) return 90; // zoo
                else if (location == 2) return 120; // park
                else return 90; // hall
            }
            else // evening
            {
                if (location == 0) return 40; // business
                else if (location == 1) return 70; // zoo
                else if (location == 2) return 100; // park
                else return 70; // hall
            }
        }

        else // alternative
        {
            if (time == 0) // morning
            {
                if (location == 0) return 85; // business
                else if (location == 1) return 45; // zoo
                else if (location == 2) return 95; // park
                else return 60; // hall
            }
            else if (time == 1) // lunch
            {
                if (location == 0) return 85; // business
                else if (location == 1) return 95; // zoo
                else if (location == 2) return 105; // park
                else return 110; // hall
            }
            else if (time == 2) // afternoon
            {
                if (location == 0) return 85; // business
                else if (location == 1) return 95; // zoo
                else if (location == 2) return 125; // park
                else return 90; // hall
            }
            else // evening
            {
                if (location == 0) return 35; // business
                else if (location == 1) return 75; // zoo
                else if (location == 2) return 105; // park
                else return 70; // hall
            }
        }
    }

    private float ModifyBasedOnPrice(float sales, int price)
    {
        if (price == 0)
            return sales * 1.4f;

        else if (price == 1)
            return sales;

        else if (price == 2)
            return sales / 1.4f;

        else
            return sales / Mathf.Pow(1.4f, 2);
    }
}

public class SimulationContainer
{
    public List<Simulation> simulations;
    public List<Category> categories;
    public int numOfSimulations;
    public int designType;
    public bool overworked;
    public List<int> numOptions;

    public SimulationContainer(List<Simulation> sims)
    {
        simulations = sims;
    }

    public SimulationContainer(List<int> option1, List<int> option2)
    {
        simulations = new List<Simulation>();

        for (int i = 0; i < 65; i++)
        {
            simulations.Add(new Simulation(option1, 2));
            simulations.Add(new Simulation(option2, 2));
        }

        // other variables we dont need too much of
        overworked = false;
        categories = null;
        numOfSimulations = 0;
        designType = 0;
    }

    public SimulationContainer(List<Category> cats, int _numOfSimulations, int _designType, bool _overworked)
    {
        numOfSimulations = _numOfSimulations;
        designType = _designType;
        overworked = _overworked;

        simulations = new List<Simulation>();

        // first, make a list of what is actually selected in each category
        // being lazy, so doing it hardcoded for now
        List<int> locationList = cats[0].SelectedCategories();
        List<int> musicList = cats[1].SelectedCategories();
        List<int> priceList = cats[2].SelectedCategories();
        List<int> timeList = cats[3].SelectedCategories();
        List<int> weatherList = cats[4].SelectedCategories();

        // figure out how many options per category are activated
        numOptions = new List<int> {
            locationList.Count, musicList.Count, priceList.Count, timeList.Count, weatherList.Count
        };

        // now that we know that, multiply them together to figure out how many runs we need per repetition
        int totalVariety = 1;
        foreach (int num in numOptions)
        {
            totalVariety *= num;
        }

        // if random, each day should still happen. everything else is random
        if (designType == 2)
        {
            for (int i = 0; i < numOfSimulations; i++)
            {
                foreach (int timeSlot in timeList)
                {
                    simulations.Add(new Simulation(new List<int>() {
                        locationList[Random.Range(0, locationList.Count)],
                        musicList[Random.Range(0, musicList.Count)],
                        priceList[Random.Range(0, priceList.Count)],
                        timeSlot,
                        weatherList[Random.Range(0, weatherList.Count)] },
                        timeList.Count));
                }
            }
        }

        else if (designType == 1)
        {
            // balanced sequential
            for (int i = 0; i < numOfSimulations; i++)
            {
                foreach (int location in locationList)
                {
                    foreach (int music in musicList)
                    {
                        foreach (int price in priceList)
                        {
                            foreach (int weather in weatherList)
                            {
                                foreach (int time in timeList)
                                {
                                    simulations.Add(new Simulation(new List<int>() {
                                            location, music, price, time, weather },
                                        timeList.Count));
                                }
                            }
                        }
                    }
                }
            }
        }

        else // balanced random
        {
            // first, figure out our possible options (minus time)
            List<List<int>> tempList = new List<List<int>>();

            // balanced sequential
            for (int i = 0; i < numOfSimulations; i++)
            {
                foreach (int location in locationList)
                {
                    foreach (int music in musicList)
                    {
                        foreach (int price in priceList)
                        {
                            foreach (int weather in weatherList)
                            {
                                tempList.Add(new List<int> { location, music, price, weather });
                            }
                        }
                    }
                }
            }

            List<List<Simulation>> simShifts = new List<List<Simulation>>();
            foreach (int time in timeList)
            {
                simShifts.Add(CalcShiftsPerTime(tempList, time, timeList.Count));
            }

            // theoretically, each list<sim> in simshifts should be the same length, right?

            for (int i = 0; i < simShifts[0].Count; i++)
            {
                for (int j = 0; j < timeList.Count; j++)
                {
                    simulations.Add(simShifts[j][i]);
                }
            }
        }

        //foreach (Simulation sim in finalSimList)
        //{
        //    Debug.Log(sim.options[0] + " - " + sim.options[1] + " - " + sim.options[2] + " - " + sim.options[3]);
        //}
    }

    private List<Simulation> CalcShiftsPerTime(List<List<int>> combos, int time, int shiftsPerDay)
    {
        // in this time,
        // include time with each combination
        List<Simulation> shifts = new List<Simulation>();
        foreach (List<int> options in combos)
        {
            shifts.Add(new Simulation(new List<int>() {options[0], options[1], options[2], time, options[3]}, shiftsPerDay));
        }

        // mix the times around
        shifts = CalcUtil.Reshuffle(shifts);

        return shifts;
    }
}