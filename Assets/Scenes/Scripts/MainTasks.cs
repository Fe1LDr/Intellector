using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MainTasks : MonoBehaviour
{
    private static Queue<Action> tasks  = new Queue<Action>();

    private void Update()
    {
        try
        {
            if (tasks.Count != 0)
            {
                Action task = tasks.Dequeue();
                task();
            }
        }
        catch (Exception e) { LogWriter.WriteLog(e.ToString() + e.Source + e.StackTrace); }
    }

    
    public static void ExecuteTask(Action task)
    {
        AddTask(task);
        WaitTask();
    }

    public static void AddTask(Action task)
    {
        tasks.Enqueue(task);
    }

    private static async void WaitTask()
    {
        await Task.Run(() => { while (tasks.Count > 0) { } });
    }
}
