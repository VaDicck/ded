public abstract class Culture
{
    public abstract CultureType Type { get; }
        
    public int Count { get; protected set; }
}