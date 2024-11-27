using UnityEngine;

public class UserConfig
{
    public string UserName { get; set; }
    public PieceMaterials Material { get; set; }

    public void Save()
    {
        PlayerPrefs.SetString(nameof(UserName), UserName);
        PlayerPrefs.SetInt(nameof(Material), (int)Material);
    }

    public static UserConfig Load()
    {
        return new UserConfig
        {
            UserName = PlayerPrefs.GetString(nameof(UserName)),
            Material = (PieceMaterials)PlayerPrefs.GetInt(nameof(Material)),
        };
    }
}

