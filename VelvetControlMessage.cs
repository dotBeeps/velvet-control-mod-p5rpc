namespace VelvetControl;

public class VelvetControlMessage
{
    public string command { get; set; }
    public VelvetParams parameters { get; set; }
}

public class VelvetParams : Dictionary<string, int>
{
    
}