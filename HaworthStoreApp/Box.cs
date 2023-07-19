public class Components
{
    public string? storeName { get; set; }
    public int count { get; set; }
 
}




public class Box
{
    public string? boxName { get; set; }
    public IList<Components>? screws { get; set; }
    public IList<Components>? wheels { get; set; }
    public IList<Components>? arambars { get; set; }
    public IList<Components>? nuts { get; set; }
}
