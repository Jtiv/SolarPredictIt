using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIContract : MonoBehaviour
{   
    private Contract contractObject;
    public TextMeshProUGUI _shortName;
    public Text[] _buySellPrices;
    
    public UIContract(Contract MarketContractObject)
    {
        SetContract(MarketContractObject);
    }

    public void SetContract(Contract Contract)
    {
        contractObject = Contract;
        RefreshUI();
    }
 

    public void RefreshUI()
    {
        _shortName.text = contractObject._shortName;
        //_status.text = marketContractObject._status;

        for (int i = 0; i < contractObject._buySellPrices.Length; i++)
        {
            _buySellPrices[i].text = contractObject._buySellPrices[i];
        }
    }

    private void OnDestroy()
    {
        Debug.Log("urk gaa gablaah keehh bleeeh *coughs blood* im ded");
    }
}
