using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MarketDataObject 
{
    public int _ID { get; private set; }
    public string shortName { get; private set; }
    public List<MarketContractObject> marketContractsList { get; private set; }

    public MarketDataObject(int ID, string Name)
    {
        shortName = Name;
        _ID = ID;
        marketContractsList = new List<MarketContractObject>();
    }

    public void AddContractToMarket(MarketContractObject marketContractObject)
    {
        marketContractsList.Add(marketContractObject);
    }

    public List<MarketContractObject> GetMarketContractList()
    {
        return marketContractsList;
    }

}
