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
    private GameObject UI_CanvasPanel, OctoContractSpawnPoint;
    private RectTransform UiCanvasTransform;

    [SerializeField]
    private GameObject UI_Contract, Octahedron_Contract, PlanetMarket;

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
        Queue<Vector3> PlanetMarketPos = RandomishPlanetMarketTransform(1000, MarketsList.Count);

        float x;
        float y = 0f;

        foreach (KeyValuePair<int, MarketData> entry in MarketsList)
        {
            //spawn a market list UI object to house all of the contract UI objects
            //...For now just create a new line effectively making a new row
            PlanetMarket planetMarket = Instantiate(PlanetMarket, PlanetMarketPos.Dequeue(), Quaternion.identity).GetComponent<PlanetMarket>();


            x  = .7f;
            y += .7f;

            foreach (KeyValuePair<int, Contract> MarketContractData in entry.Value.ContractList)
            {
                //instantiate UI object for contract
                
                UIContract uicontract = Instantiate(UI_Contract, UI_CanvasPanel.transform).GetComponent<UIContract>();
                uicontract.ToggleActive();
                RectTransform uiContractTransform = uicontract.GetComponent<RectTransform>();
                uiContractTransform.anchoredPosition = new Vector2(x * spacingVar, y * -spacingVar);
                uicontract.SetContract(MarketContractData.Value);
                Vector3 ParentPosition = uiContractTransform.position;

                //instantiate octohedron UI elements
                OctahedronContract octahedronContract = Instantiate(Octahedron_Contract, ParentPosition, Quaternion.identity).GetComponent<OctahedronContract>();
                octahedronContract.SetContract(MarketContractData.Value);
                octahedronContract.SetGravPoint(planetMarket.gameObject.transform);

                uicontract.ToggleActive();
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
            Destroy(thing.gameObject);
        }

        OctahedronContract[] OctsToDelete = FindObjectsOfType<OctahedronContract>();
        foreach (var octItem in OctsToDelete)
        {
            Destroy(octItem.gameObject);
        }

        PlanetMarket[] PlanetsToDestroy = FindObjectsOfType<PlanetMarket>();
        foreach (var planet in PlanetsToDestroy)
        {
            Destroy(planet.gameObject);
        }


    }

    private Queue<Vector3> RandomishPlanetMarketTransform(int inputDistance, int NumOfMarkets)
    {
        Queue<Vector3> locations = new Queue<Vector3>();

        int leap = inputDistance / NumOfMarkets++;
        int land = (-inputDistance / 2) + leap;

        for (int i = 1; i < NumOfMarkets; i++)
        {
            Vector3 position = new Vector3(land, Random.Range(-50f, 50f), Random.Range(175f, 350f));
            locations.Enqueue(position);
            land += leap;
        }

        return locations;
    }

    
}
