using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{

    public static Timer instance { get; private set;}

    [SerializeField] private float RefreshVariable;
    [SerializeField] private float elapsedTime;

    float Timeplaying;
    float interval;
    bool isGoing;

    [SerializeField] public event Action OnTimerHitInterval;

    void Awake()
    {
        instance = this;
        elapsedTime = 0;
        isGoing = false;
    }

    
    public void BeginTimer()
    {
        if (isGoing == false)
        {
            isGoing = true;
            elapsedTime = 0;
            StartCoroutine(UpdateTimer());
        }
        
    }

    public void StopTimer()
    {
        isGoing = false;
    }

    
    private IEnumerator UpdateTimer()
    {

        while(isGoing is true)
        {
            elapsedTime += Time.deltaTime;

            if(elapsedTime >= RefreshVariable)
            {
                
                Debug.Log("Refreshing");
                elapsedTime = 0;
                OnTimerHitInterval?.Invoke();
            }

            yield return null;
        }
       
    }

    public float GetTimerActive()
    {
        return elapsedTime;
    }
    public float GetTimerRefresh()
    {
        return RefreshVariable;
    }
}
