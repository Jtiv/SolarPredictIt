using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lim.InGameConsole;

public class InputCommand
{
    [ConsoleCommand]
    public static void AddID (int ID)
    {
        if (!MarketHandler.instance._inputFieldList.Contains(ID))
        {
            MarketHandler.instance._inputFieldList.Add(ID);
        }
        else
        {
            Debug.Log("List contains ID already -- AKA already queued to lookup");
        }
    }

    [ConsoleCommand]
    public static void ClearIDs()
    {
        MarketHandler.instance._inputFieldList.Clear();
    }

    [ConsoleCommand]
    public static void ClearUI()
    {
        UIHandler.instance.ClearUIObjects();
    }

    [ConsoleCommand]
    public static void SpawnUI()
    {
        UIHandler.instance.HandleUI(MarketHandler.instance.MarketsList);
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
