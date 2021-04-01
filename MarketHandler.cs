using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SimpleJSON;
using TMPro;

public class MarketHandler : MonoBehaviour
{
    //singleton
    public static MarketHandler instance;

    //callbacks
    public delegate void HandledCallback(Dictionary<int, MarketData> marketslist);
    public event HandledCallback OnMarketHandled;

    //functionality
    private Timer timer;
    public List<int> _inputFieldList;
    public Dictionary<int, MarketData> MarketsList { get; private set; }


    
    private void Awake()
    {
        //singleton
        instance = this;

        MarketsList = new Dictionary<int, MarketData>();

        timer = FindObjectOfType<Timer>();
        timer.OnTimerHitInterval += OnButtonOrSomething;
        
    }
    
    public void OnButtonOrSomething()
    {
        foreach (int ID in _inputFieldList)
        {
            ApiCaller apiCaller = new ApiCaller();
            StartCoroutine(apiCaller.CallPredictitAPI(ID, HandleMarketData));
            apiCaller.Destroy();
        }
    }

    //Handles Data from the Api Caller and Arranges it within its proper markets and contracts
    private void HandleMarketData(JSONNode MarketDataNode)
    {
        //checked if passed JSON Node exists
        Debug.Log(MarketDataNode);
        
        //pull the name and ID, store dem badbois
        string Name = MarketDataNode["shortName"];
        int ID = MarketDataNode["id"];

        //Write names to console
        Debug.Log($"Name is {Name} + ID is {ID}");

        //create market object
        MarketData market = new MarketData(ID, Name);

        //Pull contracts from JSON Node
        JSONNode MarketContracts = MarketDataNode["contracts"];

        //Output to Console
        Debug.Log($"contract list made {MarketContracts.Count}");
        
        //As many times as there are contracts in MarketContracts...
        for (int i = 0; i < MarketContracts.Count; i++)
        {   //...pull these elements of contract-pertaining data
            string[] BuySellPrices = new string[6] 
            {
                MarketContracts[i]["lastTradePrice"],
                MarketContracts[i]["bestBuyYesCost"],
                MarketContracts[i]["bestBuyNoCost"],
                MarketContracts[i]["bestSellYesCost"],
                MarketContracts[i]["bestSellNoCost"],
                MarketContracts[i]["lastClosePrice"]
            };
            
            //...create a <Contract> to contain it
            Contract marketContractObject = new Contract
                (   MarketContracts[i]["shortName"], 
                    MarketContracts[i]["status"], 
                    BuySellPrices );
            //...call the market function to add the contract to it (is this the best way to do this? Jurys still out)
            market.AddContractToMarket(marketContractObject);
        }
        

        //now add the Market itself to a DICTIONARY of markets
        //adding ID as KEY so that calling the same market OVERRIDES instead of ADDs to DICTIONARY
        MarketsList[market._ID] = market;

        //Visualize the contents of the DICTIONARY
        if (_inputFieldList.Count <= MarketsList.Count)
        {
            OnMarketHandled?.Invoke(MarketsList);
        }

    }

    
    //private void OnDestroy()
    //{
    //    timer.OnTimerHitInterval -= OnButtonOrSomething;
    //}


}
