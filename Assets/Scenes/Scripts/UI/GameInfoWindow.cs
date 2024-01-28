using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInfoWindow : MonoBehaviour
{
    [SerializeField] InputField NameInput;
    [SerializeField] Dropdown TimeControlDropDown;
    [SerializeField] ColorSelector ColorSelector;

    [SerializeField] Text ErrorText;
    private void Awake()
    {
        NameInput.text = Settings.Load().UserName;
        TimeControlDropDown.options = new List<Dropdown.OptionData>();
        foreach(TimeContol time in TimeControlSelector.time_controls)
        {
            TimeControlDropDown.options.Add(new Dropdown.OptionData(time.ToString()));
        }
    }

    public GameInfo GetGameInfo()
    {
        string Name = NameInput.text;
        if(!CheckName()) return null;

        TimeContol timeContol = TimeControlSelector.time_controls[TimeControlDropDown.value];
        ColorChoice color = ColorSelector.Color;

        return new GameInfo { ID = 0, Color = color, Name = Name, TimeContol = timeContol };
    }

    public void NameInputChanged()
    {
        CheckName();
    }

    private bool CheckName()
    {
        string error_mes;
        bool valid = Settings.CheckName(NameInput.text, out error_mes);
        ErrorText.text = error_mes;
        return valid;
    }
}
