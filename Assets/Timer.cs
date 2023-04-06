using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class Timer : MonoBehaviour
{
    public static bool timerActivate;
    [SerializeField] private Timer instanceRef;
    private float currentTime;
    [SerializeField] private int startMinutes;
    [SerializeField] private TextMeshProUGUI currentTimeText;
    // Start is called before the first frame update

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        currentTime = startMinutes * 60;
        StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q)) StartTimer();
        if (Input.GetKeyUp(KeyCode.E)) StopTimer();

        if (timerActivate) currentTime += Time.deltaTime;

        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString() + ":" + GetMiliseconds(time);
    }


    string GetMiliseconds(TimeSpan time)
    {
        string milSec1 = time.Milliseconds.ToString()[0].ToString();
        string milSec2 = time.Milliseconds.ToString().Length > 1 ? time.Milliseconds.ToString()[1].ToString() : string.Empty;
        return milSec1 + milSec2;
    }

    public void StartTimer() => timerActivate = true;

    public static void StopTimer() => timerActivate = false;
}
