using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MainTasks : MonoBehaviour
{
    private static Queue<Action> tasks  = new Queue<Action>();

    void Update()
    {
        if(tasks.Count != 0)
        {
            Action task = tasks.Dequeue();
            task();
        }
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
