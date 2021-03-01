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
        Debug.Log("created");

        _instance = this;
    }
   


    public IEnumerator CallPredictitAPI (int ID, Action<JSONNode> onComplete)
    {
        string url = _basePredictItURL + ID.ToString();

        using (UnityWebRequest PiApiRequest = UnityWebRequest.Get(url))
        {

            PiApiRequest.downloadHandler = new DownloadHandlerBuffer();
            Debug.Log($"making a request at{url}");


            yield return PiApiRequest.SendWebRequest();
            Debug.Log("Request Sent and Returned");


            if (PiApiRequest.isNetworkError || PiApiRequest.isHttpError)
            {
                Debug.LogError(PiApiRequest.error);
                yield break;
            }

            Debug.Log(PiApiRequest.downloadHandler.text);

            JSONNode MarketDataJSONNode = JSON.Parse(PiApiRequest.downloadHandler.text);

            if (MarketDataJSONNode.IsNull)
            {
                Debug.Log("shits null");
                yield break;
            }
            else
            {
                Debug.Log("not null?");
            }

            Debug.Log("node made me thinks");
            onComplete(MarketDataJSONNode);
        }
    }
    
    public void Destroy()
    {
        Destroy(_instance);
    }


}
