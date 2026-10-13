using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "CarrotCulture", menuName = "Configs/Cultures/Carrot")]
public class CarrotCulture : Culture
{
    public CarrotCulture(PlayerCultures playerCultures, DayCycleService dayCycleService) : base(playerCultures, dayCycleService) { }
    
    public class Factory : PlaceholderFactory<CarrotCulture> {}
    
    public override CultureType Type => CultureType.Carrot;
}