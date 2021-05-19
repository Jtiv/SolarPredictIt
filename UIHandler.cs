using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;

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
            //spawn a marketList UI object to house all of the contract UI objects
            PlanetMarket planetMarket = Instantiate(PlanetMarket, PlanetMarketPos.Dequeue(), Quaternion.identity).GetComponent<PlanetMarket>();
            planetMarket.SetMarketData(entry.Value);


            x  = .7f;
            y += .7f;

            foreach (KeyValuePair<int, Contract> MarketContractData in entry.Value.ContractList)
            {
                //instantiate UI object for contract
                //Monobehaviors and Components... this needs to be instantiated with gameobject of sorts then can be accessed...

                RectTransform uiContractTransform = new GameObject().AddComponent<RectTransform>();
                uiContractTransform.SetParent(UI_CanvasPanel.transform);
                uiContractTransform.anchoredPosition = new Vector2(x * spacingVar, y * -spacingVar);
                Vector3 ParentPosition = uiContractTransform.position;

                //instantiate octohedron UI elements
                OctahedronContract octahedronContract = Instantiate(Octahedron_Contract, ParentPosition, Quaternion.identity).GetComponent<OctahedronContract>();
                octahedronContract.SetContract(MarketContractData.Value);
                octahedronContract.SetGravPoint(planetMarket.gameObject.transform);
                x++;
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

    public void SpawnSingleUIContract(Contract Contract)
    {
        UIContract uiContract = Instantiate(UI_Contract, UI_CanvasPanel.transform).GetComponent<UIContract>();
        uiContract.SetContract(Contract);
        RectTransform uiContractTransform = uiContract.GetComponent<RectTransform>();
        uiContractTransform.anchoredPosition = new Vector2(.7f * spacingVar, 2.7f * -spacingVar);
        uiContract.ToggleActive();
        
    }
    
}
