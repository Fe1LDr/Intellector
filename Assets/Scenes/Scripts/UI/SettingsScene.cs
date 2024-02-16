using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsScene : MonoBehaviour
{
    private Settings settings;
    [SerializeField] private InputField NameInput;
    [SerializeField] private Text ErrorText;
    [SerializeField] private Text MaterialName;

    
    void Start()
    {
        ShowCurrentSettings();
    }
    void ShowCurrentSettings()
    {
        settings = Settings.Load();
        NameInput.text = settings.UserName;
        MaterialName.text = MaterialSelector.MaterialName(settings.Material);
    }

    public void InputChanged() => CheckName();
    private bool CheckName()
    {
        string error_mes;
        bool valid = Settings.CheckName(NameInput.text, out error_mes);
        ErrorText.text = error_mes;
        return valid;
    }
    public void SaveButtonClick()
    {
        if (CheckName())
        {
            settings.UserName = NameInput.text;
            settings.Save();
            Exit();
        }
    }
    public void CanselButtonClick()
    {
        ShowCurrentSettings();
        ErrorText.text = String.Empty;
    }
    public void SwitchMaterial(int direction)
    {
        int new_materials_number = ((int)settings.Material + direction);
        int max_number = Enum.GetNames(typeof(AvaibleMaterials)).Length;
        if (new_materials_number >= max_number) new_materials_number = 0;
        if (new_materials_number < 0) new_materials_number = max_number - 1;
        settings.Material = (AvaibleMaterials)(new_materials_number);
        MaterialName.text = MaterialSelector.MaterialName(settings.Material);
    }
    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
