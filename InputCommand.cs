using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lim.InGameConsole;

public class InputCommand
{
    [ConsoleCommand]
    public static void AddID (int ID)
    {
        if (!MarketHandler.instance._inputFieldArray.Contains(ID))
        {
            MarketHandler.instance._inputFieldArray.Add(ID);
            Debug.Log("added");
        }
        else
        {
            Debug.Log("List contains ID already -- AKA already queued to lookup");
        }
    }

    [ConsoleCommand]
    public static void ClearIDs()
    {
        MarketHandler.instance._inputFieldArray.Clear();
    }

    [ConsoleCommand]
    public static void ClearUI()
    {
        MarketHandler.instance.ClearUIObjects();
    }

    [ConsoleCommand]
    public static void SpawnUI()
    {
        MarketHandler.instance.HandleUI();
    }

    [ConsoleCommand]
    public static void CallPredictIt()
    {
        MarketHandler.instance.OnButtonOrSomething();
    }

    [ConsoleCommand]
    public static void StopTimer()
    {
        Timer.instance.StopTimer();
    }

    [ConsoleCommand]
    public static void BeginTimer()
    {
        Timer.instance.BeginTimer();
    }

   

}
