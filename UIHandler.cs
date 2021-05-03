using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;

    //private MarketHandler marketHandler;

    private float spacingVar;
    private float lineBreakVar;

    [SerializeField]
    private GameObject UI_CanvasPanel;
    private RectTransform UiCanvasTransform;

    [SerializeField]
    private GameObject UI_Contract, Octahedron_Contract;

    private void Start()
    {
        instance = this;

        //marketHandler = FindObjectOfType<MarketHandler>();
        MarketHandler.instance.OnMarketHandled += HandleUI;

        UiCanvasTransform = UI_CanvasPanel.GetComponent<RectTransform>();
        spacingVar = UI_Contract.GetComponent<RectTransform>().rect.width * .52f;
        lineBreakVar = UiCanvasTransform.rect.width / spacingVar;
    }

    
    public void HandleUI(Dictionary<int, MarketData> MarketsList)
    {

        ClearUIObjects();

        float x;
        float y = 0f;

        foreach (KeyValuePair<int, MarketData> entry in MarketsList)
        {
            //spawn a market list UI object to house all of the contract UI objects
            //...For now just create a new line effectively making a new row
            x = .7f;
            y++;

            foreach (KeyValuePair<int, Contract> MarketContractData in entry.Value.ContractList)
            {
                //instantiate UI object for contract

                UIContract uicontract = Instantiate(UI_Contract, UI_CanvasPanel.transform).GetComponent<UIContract>();
                uicontract.SetContract(MarketContractData.Value);
                RectTransform dataTransform = uicontract.GetComponent<RectTransform>();
                dataTransform.anchoredPosition = new Vector2(x * spacingVar, y * -spacingVar);

                //instantiate octohedron UI elements
                OctahedronContract octahedronContract = Instantiate(Octahedron_Contract, dataTransform).GetComponent<OctahedronContract>();
                octahedronContract.SetContract(MarketContractData.Value);
                x++;

                // (x > lineBreakVar){ x = 0; y++;}

                if (MarketContractData.Value._status == "Open")
                {
                    Debug.Log($"{MarketContractData.Value._shortName}" +
                            $"{MarketContractData.Value._status}" +
                            $"Last Trader Price  > {MarketContractData.Value._buySellPrices[0]}" +
                            $"Best Buy Yes Cost  > {MarketContractData.Value._buySellPrices[1]}" +
                            $"Best Buy No Cost   > {MarketContractData.Value._buySellPrices[2]}" +
                            $"Best Sell Yes Cost > {MarketContractData.Value._buySellPrices[3]}" +
                            $"Best Sell No Cost  > {MarketContractData.Value._buySellPrices[4]}" +
                            $"Last Close Price   > {MarketContractData.Value._buySellPrices[5]}");
                }


                Debug.Log("______________________________________________________________");
            }
        }

    }

    public void ClearUIObjects()
    {
        UIContract[] toDelete = FindObjectsOfType<UIContract>();
        foreach (var thing in toDelete)
        {
            Destroy(thing);
        }

        OctahedronContract[] OctsToDelete = FindObjectsOfType<OctahedronContract>();
        foreach (var octItem in OctsToDelete)
        {
            Destroy(octItem);
        }
    }
}
