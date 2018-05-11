
using System.Collections.Generic;
using System.Linq;

public static class ColorLogic
{
    public static bool IsRedConditionMet(LogicalButton button1, LogicalButton button2)
    {
        return button1.Color != ButtonColor.Blue && button2.Color != ButtonColor.Blue;
    }

    public static bool IsBlueConditionMet(LogicalButton button1, LogicalButton button2)
    {
        return button1.Color == ButtonColor.Blue || button2.Color == ButtonColor.Blue;
    }

    public static bool IsGreenConditionMet(LogicalButton button)
    {
        return button.Color == ButtonColor.Purple || button.Color == ButtonColor.White;
    }

    public static bool IsYellowConditionMet(LogicalButton button)
    {
        return !button.Label.Equals(Constants.WrongString) && !button.Label.Equals(Constants.LogicString);
    }

    public static bool IsPurpleConditionMet(LogicalButton button1, LogicalButton button2)
    {
        return !IsPrimaryColor(button1.Color) && !IsPrimaryColor(button2.Color);
    }

    public static bool IsWhiteConditionMet(LogicalButton button1, LogicalButton button2)
    {
        return IsPrimaryColor(button1.Color) || IsPrimaryColor(button2.Color);
    }

    public static bool IsOrangeConditionMet(LogicalButton button)
    {
        return button.Color != ButtonColor.Orange;
    }

    public static bool IsCyanConditionMet(LogicalButton button)
    {
        return button.Label.Length == 5;
    }

    public static bool IsGreyConditionMet(LogicalButton button, IList<LogicalButton> otherButtons)
    {
        return otherButtons.Any(x => x.Label.Equals(button.Label));
    }

    private static bool IsPrimaryColor(ButtonColor color)
    {
        return color == ButtonColor.Red || color == ButtonColor.Blue || color == ButtonColor.Yellow;
    }
}
