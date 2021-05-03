using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIClock : MonoBehaviour
{
    private Timer timer;
    [SerializeField]
    private TextMeshProUGUI ClockTime;

    private void Start()
    {
        timer = Timer.instance;
    }

    void Update()
    {
        float[] array = new float[2]{ Mathf.Round(timer.GetTimerActive()), timer.GetTimerRefresh() };
        ClockTime.text = string.Join(":", array);
    }
}
