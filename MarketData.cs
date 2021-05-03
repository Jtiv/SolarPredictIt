using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketData 
{
    public int _ID { get; private set; }
    public string _shortName { get; private set; }
    public Dictionary<int, Contract> ContractList { get; private set; }

    public MarketData(int ID, string Name)
    {
        _shortName = Name;
        _ID = ID;
        ContractList = new Dictionary<int, Contract>();
    }

    public void AddContractToMarket(Contract Contract)
    {
        ContractList[Contract._ID] = Contract;
    }

    public Dictionary<int, Contract> GetMarketContractList()
    {
        return ContractList;
    }

}
