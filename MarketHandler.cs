using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using TMPro;

public class MarketHandler : MonoBehaviour
{
    //singleton
    public static MarketHandler instance;
    
    
    private Timer timer;
    private float spacingVar;
    private float lineBreakVar;

    [SerializeField]
    private GameObject UI_CanvasPanel;
    private RectTransform UiCanvasTransform;

    [SerializeField]
    private GameObject UI_Contract;
    
    public List<int> _inputFieldArray;

    public Dictionary<int, MarketData> MarketsList { get; private set; }

    private void Awake()
    {
        //singleton
        instance = this;

        MarketsList = new Dictionary<int, MarketData>();

        timer = FindObjectOfType<Timer>();
        timer.OnTimerHitInterval += OnButtonOrSomething;

        UiCanvasTransform = UI_CanvasPanel.GetComponent<RectTransform>();
        spacingVar = UI_Contract.GetComponent<RectTransform>().rect.width * .52f;
        lineBreakVar = UiCanvasTransform.rect.width / spacingVar;

    }
    
    public void OnButtonOrSomething()
    {
        ClearUIObjects();
         
        foreach (int ID in _inputFieldArray)
        {
            //vvv spawn new MARKET_UI to house markets 
            
            //^^^ spawn new MARKET_UI to house markets

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
        if (_inputFieldArray.Count <= MarketsList.Count)
        {
            HandleUI();
        }

    }

    public void ClearUIObjects()
    {
        UIContract[] toDelete = FindObjectsOfType<UIContract>();
        foreach (var thing in toDelete)
        {
            Destroy(thing);
        }
    }
    

    public void HandleUI()
    {
        

        int x = 0;
        int y = 0;

        foreach (KeyValuePair<int, MarketData> entry in MarketsList)
        {
            //spawn a market list UI object to house all of the contract UI objects
            //...For now just create a new line effectively making a new row
            x = 0;
            y++;
            
            foreach (Contract MarketContractData in entry.Value.ContractList)
            {
                //instantiate UI object for contract

                UIContract uicontract = Instantiate(UI_Contract, UI_CanvasPanel.transform).GetComponent<UIContract>();
                uicontract.SetContract(MarketContractData);
                RectTransform dataTransform = uicontract.GetComponent<RectTransform>();
                dataTransform.anchoredPosition = new Vector2(x * spacingVar, y * -spacingVar);
                x++;

                // (x > lineBreakVar){ x = 0; y++;}

                if (MarketContractData._status == "Open")
                {
                    Debug.Log($"{MarketContractData._shortName}" +
                            $"{MarketContractData._status}" +
                            $"Last Trader Price  > {MarketContractData._buySellPrices[0]}" +
                            $"Best Buy Yes Cost  > {MarketContractData._buySellPrices[1]}" +
                            $"Best Buy No Cost   > {MarketContractData._buySellPrices[2]}" +
                            $"Best Sell Yes Cost > {MarketContractData._buySellPrices[3]}" +
                            $"Best Sell No Cost  > {MarketContractData._buySellPrices[4]}" +
                            $"Last Close Price   > {MarketContractData._buySellPrices[5]}");
                }


                Debug.Log("______________________________________________________________");
            }
        }
        
    }

    
    //private void OnDestroy()
    //{
    //    timer.OnTimerHitInterval -= OnButtonOrSomething;
    //}


}
