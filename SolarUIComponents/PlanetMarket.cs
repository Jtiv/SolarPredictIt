using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlanetMarket : MonoBehaviour
{
    [SerializeField]
    private Transform Cam;
    [SerializeField]
    private Canvas Canvas;
    [SerializeField]
    private TextMeshProUGUI TextMesh;
    private MarketData MarketData;

    public void SetMarketData (MarketData md)
    {
        MarketData = md;
        TextMesh.text = MarketData._shortName;
        Cam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        Canvas.transform.LookAt(Canvas.transform.position + Cam.forward);
    }


}
