
using System.Collections.Generic;

public static class Constants
{
    //Words

    public const string LogicString = "Logic";

    public const string ColorString = "Color";

    public const string LabelString = "Label";

    public const string ButtonString = "Button";

    public const string WrongString = "Wrong";

    public const string NoString = "No";

    public const string WaitString = "Wait";

    public const string HmmmString = "Hmmm";

    public const string BoomString = "Boom";

    //Operators

    public const string AndOperatorString = "AND";

    public const string OrOperatorString = "OR";

    public const string XorOperatorString = "XOR";

    public const string NandOperatorString = "NAND";

    public const string NorOperatorString = "NOR";

    public const string XnorOperatorString = "XNOR";

    //Other:

    public static readonly string[] GateStrings =
    {
        Constants.AndOperatorString,
        Constants.OrOperatorString,
        Constants.XorOperatorString,
        Constants.NandOperatorString,
        Constants.NorOperatorString,
        Constants.XnorOperatorString
    };

    public static readonly string[] WordStrings =
    {  Constants.LogicString,
        Constants.ColorString,
        Constants.LabelString,
        Constants.ButtonString,
        Constants.WrongString,
        Constants.BoomString,
        Constants.NoString,
        Constants.WaitString,
        Constants.HmmmString
    };

    public static readonly IList<ButtonColor> ButtonColors = new List<ButtonColor>
    {
        ButtonColor.Red,
        ButtonColor.Blue,
        ButtonColor.Green,
        ButtonColor.Yellow,
        ButtonColor.Purple,
        ButtonColor.White,
        ButtonColor.Orange,
        ButtonColor.Cyan,
        ButtonColor.Grey
    };

}
