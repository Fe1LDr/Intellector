using System;
using UnityEngine;

[Serializable()]
public enum ColorChoice
{
    white = 0,
    black = 1,
    random = 2
}

public class ColorSelector : MonoBehaviour
{
    [SerializeField] GameObject WhiteSelection;
    [SerializeField] GameObject BlackSelection;
    [SerializeField] GameObject RandomSelection;

    public ColorChoice Color { get; private set; }

    private void Start()
    {
        RandomClick();
    }

    public void WhiteClick()
    {
        Color = ColorChoice.white;   
        WhiteSelection.SetActive(true); 
        BlackSelection.SetActive(false);
        RandomSelection.SetActive(false);
    }

    public void BlackClick()
    {
        Color = ColorChoice.black;
        BlackSelection.SetActive(true);
        RandomSelection.SetActive(false);
        WhiteSelection.SetActive(false); 
    }

    public void RandomClick()
    {
        Color = ColorChoice.random;
        RandomSelection.SetActive(true);
        WhiteSelection.SetActive(false);
        BlackSelection.SetActive(false);
    }
}
