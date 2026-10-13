using UnityEngine;

[CreateAssetMenu(fileName = "CarrotCulture", menuName = "Configs/Cultures/Carrot")]
public class CarrotCulture : Culture
{
    public override CultureType Type => CultureType.Carrot;

    private int a;
}