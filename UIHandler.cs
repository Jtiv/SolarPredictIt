using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    private MarketHandler marketHandler;
    private Transform contractSlotTemplate;
    private Transform marketSlotTemplate;

    private void Awake()
    {
        contractSlotTemplate = transform.Find("ContractSlotTemplate");
        marketSlotTemplate = contractSlotTemplate.Find("marketSlotTemplate");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
