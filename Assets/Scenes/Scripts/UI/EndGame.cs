using Assets.Scenes.Scripts.Server;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour, IServerListenerExitObserver, IServerListenerRematchObserver
{
    [SerializeField] GameObject EndGameWindow;
    [SerializeField] GameObject Rematch;
    [SerializeField] NetworkManager networkManager;
    private Text low_text;
    private Text top_text;
    
    private void Awake()
    {
        Text[] text = EndGameWindow.GetComponentsInChildren<Text>();
        low_text = text[0];
        top_text = text[1];
    }
    public void OnExitReceived()
    {
        RematchSetActive(false);
    }
    public void OnRematchReceived()
    {
        DisplayRematchRequest();
    }

    public void DisplayResult(bool network, bool winner, bool team, bool by_exit)
    {
        EndGameWindow.SetActive(true);
        low_text.text = "хцпю нйнмвемю";
        if (network) top_text.text = (winner == team) ? "бш бшхцпюкх" : "бш опнхцпюкх";
        else top_text.text = (winner) ? "онаедхкх в╗пмше" : "онаедхкх аекше";
        if (by_exit) DisplayExit();

    }
    public void Hide()
    {
        EndGameWindow.SetActive(false);
    }

    private void DisplayExit()
    {
        low_text.text = "опнрхбмхй бшьек";
    }
    private void DisplayRematchRequest()
    {
        low_text.text = "опнрхбмхй опедкюцюер пебюмь";
    }
    private void RematchSetActive(bool active)
    {
        Rematch.SetActive(active);
    }
}
