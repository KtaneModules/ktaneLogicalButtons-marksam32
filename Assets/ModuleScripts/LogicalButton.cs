
public class LogicalButton
{
    public LogicalButton(int index, ButtonColor color, string label)
    {
        this.Index = index;
        this.Color = color;
        this.Label = label;    
    }

    public int Index
    {
        get; private set;
    }
    public string Label
    {
        get;
        private set;
    }

    public ButtonColor Color
    {
        get;
        private set;
    }

    public bool IsPressed
    {
        get; set;
    }
}
