using UnityEngine;

[CreateAssetMenu(fileName = "Quota", menuName = "Configs/Quotas/Quota")]
public class Quota : ScriptableObject
{
    [field: SerializeField] public QuotaElement Main { get; private set; } = new(Element.Main);
    
    [field: SerializeField] public QuotaElement Sub1 { get; private set; } = new(Element.Sub1);
    
    [field: SerializeField] public QuotaElement Sub2 { get; private set; } = new(Element.Sub2);

    public enum Element
    {
        Main,
        Sub1,
        Sub2,
    }

    public bool TryGet(Element element, out QuotaElement quota)
    {
        quota = null;
        
        switch (element)
        {
            case Element.Main:
                quota = Main;
                break;
            
            case Element.Sub1:
                quota = Sub1;
                break;
            
            case Element.Sub2:
                quota = Sub2;
                break;
        }

        return quota is { IsEmpty: false };
    }
    
    public override string ToString()
    {
        return $"Main: {Main},\nSub1: {Sub1},\nSub2: {Sub2}";
    }
}