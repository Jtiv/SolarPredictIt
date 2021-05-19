using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;



public class ApiCaller : UnityEngine.Object
{
    private ApiCaller _instance;

    private readonly string _basePredictItURL = 
    "https://www.predictit.org/api/marketdata/markets/";

    public ApiCaller()
    {
        _instance = this;
    }
   


    public IEnumerator CallPredictitAPI (int ID, Action<JSONNode> onComplete)
    {
        string url = _basePredictItURL + ID.ToString();

        using (UnityWebRequest PiApiRequest = UnityWebRequest.Get(url))
        {

            PiApiRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return PiApiRequest.SendWebRequest();
            
            if (PiApiRequest.isNetworkError || PiApiRequest.isHttpError)
            {
                Debug.LogError(PiApiRequest.error);
                yield break;
            }
            
            JSONNode MarketDataJSONNode = JSON.Parse(PiApiRequest.downloadHandler.text);

            if (MarketDataJSONNode.IsNull)
            {
                yield break;
            }

            onComplete(MarketDataJSONNode);
        }
    }
    
    public void Destroy()
    {
        Destroy(_instance);
    }


}
