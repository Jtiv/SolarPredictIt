
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Contract
{
    private Contract _instance;

    public string _shortName { get; private set; }
    public string _status { get; private set;}
    public string[] _buySellPrices { get; private set; }

    public Contract(string Shortname, string Status, string[] BuySellPrices)
    {
        _shortName = Shortname;
        _status = Status;
        _buySellPrices = BuySellPrices;
        _instance = this;
    }

    public void AddTo(List<Contract> ContractList)
    {
        ContractList.Add(_instance);
    }

    public void Destroy()
    {
        _instance = null;
    }

}
