using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIContract : MonoBehaviour
{   
    private MarketContractObject marketContractObject;
    public TextMeshProUGUI _shortName;
    public Text[] _buySellPrices;

    public void Update()
    {
        if (marketContractObject == null)
        {
            Destroy(this);
            Debug.Log("destroyed");
        }


    }

    public UIContract(MarketContractObject MarketContractObject)
    {
        SetContract(MarketContractObject);
    }

    public void SetContract(MarketContractObject Contract)
    {
        marketContractObject = Contract;
        RefreshUI();
    }
 

    public void RefreshUI()
    {
        _shortName.text = marketContractObject._shortName;
        //_status.text = marketContractObject._status;

        for (int i = 0; i < marketContractObject._buySellPrices.Length; i++)
        {
            _buySellPrices[i].text = marketContractObject._buySellPrices[i];
        }
    }

}
