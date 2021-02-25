using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using TMPro;

public class MarketHandler : MonoBehaviour
{
    private Timer timer;
    private float spacingVar;
    private float lineBreakVar;

    [SerializeField]
    private GameObject UI_CanvasPanel;
    private RectTransform UiCanvasTransform;

    [SerializeField]
    private GameObject UI_Contract;

    //[SerializeField] private TMP_InputField[] _inputFieldArray;

    [SerializeField]
    private int[] _inputFieldArray;

    public List<MarketDataObject> MarketsList { get; private set; }

    private void Awake()
    {
        timer = FindObjectOfType<Timer>();

        timer.OnTimerHitInterval += OnButtonOrSomething;

        UiCanvasTransform = UI_CanvasPanel.GetComponent<RectTransform>();

        spacingVar = UI_Contract.GetComponent<RectTransform>().rect.width * .52f;

        lineBreakVar = UiCanvasTransform.rect.width / spacingVar;

        // _inputFieldArray = FindObjectsOfType<TMP_InputField>();
    }
    
    public void OnButtonOrSomething()
    {
        //check if there is an existant list and if it has items -- Empty it's contents to refresh
        if (MarketsList != null)
        {
            //empty the contents of the MarketsList
            foreach (MarketDataObject mdo in MarketsList)
            {
                foreach  (MarketContractObject mco in mdo.marketContractsList)
                {

                    mco.Destroy();
                    
                }

                //clear contract lists of each 
                mdo.marketContractsList.Clear();

            }

            //then clear the MarketList
            MarketsList = null;
        }
        
        MarketsList = new List<MarketDataObject>();

        foreach (int ID in _inputFieldArray)
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
        
        //pull the name, and ID store dem badbois
        string Name = MarketDataNode["shortName"];
        int ID = MarketDataNode["id"];

        //Write names to console
        Debug.Log($"Name is {Name} + ID is {ID}");

        //create market object
        MarketDataObject market = new MarketDataObject(ID, Name);

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
            
            //...create a MarketContractObject to contain it
            MarketContractObject marketContractObject = new MarketContractObject
                (   MarketContracts[i]["shortName"], 
                    MarketContracts[i]["status"], 
                    BuySellPrices );
            //...call the market function to add the contract to it (is this the best way to do this? Jurys still out)
            market.AddContractToMarket(marketContractObject);
        }
        //now add the Market itself to a list of markets
        MarketsList.Add(market);

        //now reorder the list of markets to keep the UI consistant <lowest ID to highest>
        //UI is spawned based on timing of api call, meaning not always in the same order

        


        //take in ID of market
        //sort IDs
        //return new list in new order


        //
        int x = 0;
        int y = 0;

         

        foreach (MarketDataObject MarketData in MarketsList)
        {
            x = 0;
            y++;

            foreach (MarketContractObject MarketContractData in MarketData.marketContractsList)
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


        //send alert to update all Contracts and Markets in UI

    }

    private List<MarketDataObject> SortList(List<MarketDataObject> input)
    {
        Debug.Log("head of list reads" + input[0]);
        int c = input.Count;

        for (int i = 0; i < c - 1; i++)
        {
            int j = i;
            while (j > 0 && input[j - 1]._ID > input[j]._ID)
            {
                Swap(input[j], input[j - 1]);
                j = j - 1;
            }

        }

        Debug.Log("new head of list reads" + input[0]);

        return input;

    }


    private void Swap(object a, object b)
    {
        var temp = a;
        a = b;
        b = temp;
    }

    //private void OnDestroy()
    //{
    //    timer.OnTimerHitInterval -= OnButtonOrSomething;
    //}


}
