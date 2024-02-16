using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TimeControlView : MonoBehaviour
{
    [SerializeField] GameObject GameObject;
    [SerializeField] Text OpponentTime;
    [SerializeField] Text MyTime;
    [SerializeField] GameObject OpponentPanel;
    [SerializeField] GameObject MyPanel;

    public delegate void DisplayTime(int time);

    public DisplayTime DisplayWhiteTime;
    public DisplayTime DisplayBlackTime;
    public Action DisplayWhiteTurn;
    public Action DisplayBlackTurn;

    private int my_time;
    private int opponent_time;
    private bool turn;
    bool time_run;

    Thread my_time_runner;
    Thread opponent_time_runner;

    public bool Team 
    {
        set
        {
            turn = !value;
            if (value)
            {
                DisplayWhiteTime = DisplayOpponentTime;
                DisplayBlackTime = DisplayMyTime;

                DisplayWhiteTurn = DisplayOpponentTurn;
                DisplayBlackTurn = DisplayMyTurn;
            }
            else
            {
                DisplayWhiteTime = DisplayMyTime;
                DisplayBlackTime = DisplayOpponentTime;

                DisplayWhiteTurn = DisplayMyTurn;
                DisplayBlackTurn = DisplayOpponentTurn;
            }
        }
    }

    public void Activate()
    {
        GameObject.SetActive(true);
    }
    public void StartRunTime()
    {
        time_run = true;
        DisplayWhiteTurn();
        my_time_runner = new Thread(() => ShowRunningTime(DisplayMyTime, ref my_time, true));
        opponent_time_runner = new Thread(() => ShowRunningTime(DisplayOpponentTime, ref opponent_time, false));
        my_time_runner.Start();
        opponent_time_runner.Start();
    }
    public void Stop()
    {
        time_run = false;
        my_time_runner.Join();
        opponent_time_runner.Join();
    }
    private void DisplayOpponentTime(int time)
    {
        opponent_time = time;
        MainTasks.AddTask(() => { if (OpponentTime != null) OpponentTime.text = TimeToString(time); });
    }
    private void DisplayMyTime(int time)
    {
        my_time = time;
        MainTasks.AddTask(() => { if (MyTime != null) MyTime.text = TimeToString(time); });
    } 
    private void DisplayOpponentTurn()
    {
        turn = false;
        
        OpponentPanel.SetActive(true);
        MyPanel.SetActive(false);
        
    }
    private void DisplayMyTurn()
    {
        turn = true;
        
        MyPanel.SetActive(true);
        OpponentPanel.SetActive(false);
        
    }
    private string TimeToString(int milliseconds)
    {
        int total_seconds = milliseconds / 1000;
        int minutes = total_seconds / 60;
        int seconds = total_seconds % 60;
        return $"{minutes:D2}:{seconds:D2}";
    }
    private void ShowRunningTime(DisplayTime display_time, ref int time, bool needed_turn)
    {
        const int sleep_time = 100;
        while (time_run)
        {
            if (turn != needed_turn)
            {
                Thread.Sleep(sleep_time);
                continue;
            }
            DateTime begin_time = DateTime.Now;
            Thread.Sleep(sleep_time);
            DateTime end_time = DateTime.Now;
            int elapsed_time = (int)(end_time - begin_time).TotalMilliseconds;
            display_time(time - elapsed_time);
        }
    }
}
