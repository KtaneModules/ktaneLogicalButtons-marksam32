using System.Collections.Generic;
using System.Linq;

public static class LabelLogic
{
    public static bool IsLabelLogicMet(IList<LogicalButton> buttons)
    {
        return buttons.All(x => x.Color != ButtonColor.Grey);
    }

    public static bool IsLabelColorMet(LogicalButton button)
    {
        return new[] { ButtonColor.Green, ButtonColor.Yellow, ButtonColor.Orange }.All(x => x != button.Color);
    }

    public static bool IsLabelLabelMet(LogicalButton button)
    {
        return button.Label.Length != 5;
    }

    public static bool IsLabelButtonMet(LogicalButton button)
    {
        return !button.Label.Equals(Constants.HmmmString) && !button.Label.Equals(Constants.NoString);
    }

    public static bool IsLabelWrongMet(LogicalButton currentButton, LogicalButton previousButton)
    {
        return currentButton.Color == previousButton.Color;
    }

    public static bool IsLabelBoomMet(LogicalButton button1, LogicalButton button2)
    {
        return button1.Color == button2.Color;
    }

    public static bool IsLabelNoMet(int currentIndex, IList<LogicalButton> buttons)
    {
        var button = buttons[currentIndex];
        var otherButtons = FilterOutIndex(currentIndex, buttons);
        switch (button.Color)
        {
            case ButtonColor.Red:
                return !ColorLogic.IsRedConditionMet(otherButtons.First(), otherButtons.Last());
            case ButtonColor.Blue:
                return !ColorLogic.IsBlueConditionMet(otherButtons.First(), otherButtons.Last());
            case ButtonColor.Green:
                return !ColorLogic.IsGreenConditionMet(otherButtons.Last());
            case ButtonColor.Cyan:
                return !ColorLogic.IsCyanConditionMet(button);
            case ButtonColor.Yellow:
                return !ColorLogic.IsYellowConditionMet(button);
            case ButtonColor.Purple:
                return !ColorLogic.IsPurpleConditionMet(otherButtons.First(), otherButtons.Last());
            case ButtonColor.White:
                return !ColorLogic.IsWhiteConditionMet(otherButtons.First(), otherButtons.Last());
            case ButtonColor.Orange:
                return !ColorLogic.IsOrangeConditionMet(buttons[0]);
            case ButtonColor.Grey:
                return !ColorLogic.IsGreyConditionMet(button, otherButtons);
            default:
                return false;
        }
    }

    public static bool IsLabelWaitMet(int stage)
    {
        return stage == 3;
    }

    public static bool IsLabelHmmmMet(IList<LogicalButton> buttons)
    {
        return !IsLabelNoMet(1, buttons);
    }

    private static IList<LogicalButton> FilterOutIndex(int currentIndex, IList<LogicalButton> buttons)
    {
        return buttons.Where(x => x.Index != currentIndex).OrderBy(x => x.Index).ToList();
    }
}
