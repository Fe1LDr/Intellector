using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AvaibleMaterials
{
    Standard = 0,
    Mramor = 1
}

public class MaterialSelector : MonoBehaviour
{
    [SerializeField] private Material[] WhiteMaterials;
    [SerializeField] private Material[] BlackMaterials;

    private static Dictionary<AvaibleMaterials, string> MaterialNames = new Dictionary<AvaibleMaterials, string> {
        { AvaibleMaterials.Standard, "Стандарт" } ,
        { AvaibleMaterials.Mramor, "Мрамор"}
    };
    public static string MaterialName(AvaibleMaterials material) => MaterialNames[material];
    public (Material, Material) GetCurrentMaterials(AvaibleMaterials materials)
    {
        return (WhiteMaterials[(int)materials], BlackMaterials[(int)materials]);
    }
}
