using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketData 
{
    public int _ID { get; private set; }
    public string _shortName { get; private set; }
    public List<Contract> ContractList { get; private set; }

    public MarketData(int ID, string Name)
    {
        _shortName = Name;
        _ID = ID;
        ContractList = new List<Contract>();
    }

    public void AddContractToMarket(Contract Contract)
    {
        ContractList.Add(Contract);
    }

    public List<Contract> GetMarketContractList()
    {
        return ContractList;
    }

}
