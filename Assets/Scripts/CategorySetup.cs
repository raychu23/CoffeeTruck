using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CategorySetup : MonoBehaviour
{
    public static List<Category> cats = new List<Category>() {
        new Category("Location", new List<string>(new string[] {
            "Business District",
            "Near the Zoo",
            "City Park",
            "City Hall" }), new List<string>(new string[] {
                "Business", "Zoo", "Park", "CityHall" })),

        new Category("Music", new List<string>(new string[] {
            "No Music",
            "Hip Hop",
            "Alternative" }), new List<string>(new string[] {
                "none", "HipHop", "Alternative"})),

        new Category("Price", new List<string>(new string[] {
            "$2 per cup",
            "$3 per cup",
            "$4 per cup",
            "$5 per cup" }), new List<string>(new string[] {
                "$2", "$3", "$4", "$5" })),

        //new Category("Time", new List<string>(new string[] {
        //    "Morning: 6am - 10am",
        //    "Lunch: 10am - 2pm",
        //    "Afternoon: 2pm - 6pm",
        //    "Evening: 6pm - 10pm" }), new List<string>(new string[] {
        //        "Morning", "Lunch", "Afternoon", "Evening" })),

                new Category("Time", new List<string>(new string[] {
            "Morning",
            "Lunch",
            "Afternoon",
            "Evening" }), new List<string>(new string[] {
                "Morning", "Lunch", "Afternoon", "Evening" })),

        new Category("Weather", new List<string>(new string[] {
            "Relatively constant (recommended)",
            "Variable weather patterns" }), new List<string>(new string[] {
                "Constant", "Variable"})),
    };
}