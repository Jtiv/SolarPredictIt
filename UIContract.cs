using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIContract : MonoBehaviour
{
    private bool isActive;
    private Contract contractObject;
    public TextMeshProUGUI _shortName;
    public Text[] _buySellPrices;

    private void Update()
    {
        if (isActive == true)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

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

    public void ToggleActive()
    {
        if (isActive == false)
        {
            isActive = true;
        }
        else
        {
            isActive = false;
        }
        
    }

    private void OnDestroy()
    {
        Debug.Log("urk gaa gablaah keehh bleeeh *coughs blood* im ded");
    }
}
