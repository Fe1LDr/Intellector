using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LogWriter 
{
    const string LogFilePath = "log.txt";

    public static void WriteLog(string messeage)
    {
        try
        {
            using (StreamWriter LogStream = new StreamWriter(LogFilePath, true))
            {
                LogStream.WriteLine(messeage);
            }
        }
        catch (Exception) { }
    }
}
