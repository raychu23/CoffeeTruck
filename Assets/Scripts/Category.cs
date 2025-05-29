using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Category
{
    public class Option
    {
        public bool active;
        public string text, databaseText;
        public int num;

        public Option(bool a, string t, string niceT, int n)
        {
            active = a;
            text = t;
            num = n;
            databaseText = niceT;
        }

        public void Select()
        {
            active = !active;
        }

        public void SetValue(bool val)
        {
            active = val;
        }
    }

    public List<Option> options;
    public string name;

    public Category(string n, List<string> ops, List<string> niceOps)
    {
        name = n;
        options = new List<Option>();

        int num = 0;
        foreach (string op in ops)
        {
            options.Add(new Option(num == 0 ? true : false, op, niceOps[num], num));
            num++;
        }
    }

    public int NumSelected()
    {
        int numSel = 0;

        foreach (Option op in options)
        {
            if (op.active) numSel++;
        }

        return numSel;
    }

    public List<int> SelectedCategories()
    {
        List<int> selectedCats = new List<int>();

        int num = 0;
        foreach (Option op in options)
        {
            if (op.active)
                selectedCats.Add(num);

            num++;
        }

        return selectedCats;
    }
}