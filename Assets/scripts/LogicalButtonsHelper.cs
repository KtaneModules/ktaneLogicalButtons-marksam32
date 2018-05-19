

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LogicalButtonsHelper 
{

    private int[] idx = { 0, 1, 2 };

    private static readonly IList<int>[,] pressOrders = new IList<int>[,]
    {
        { new List<int> { 1, 2, 3 }, new List<int> { 2, 1, 3 }, new List<int> { 3, 2, 1 } },
        { new List<int> { 3, 1, 2 }, new List<int> { 2, 3, 1 }, new List<int> { 1, 3, 2 } }
    };

    private LogicalButton[] buttons;

    private ILogicalGateOperator gateOperator;
    
    public LogicalButtonsHelper(LogicalButton[] buttons, ILogicalGateOperator gateOperator)
    {
        this.buttons = buttons;
        this.gateOperator = gateOperator;
    }

    public IList<int> SolveOrder(int stage)
    {
        var pressOrder = this.PressOrder(this.gateOperator.Group, stage);
        var press = this.Solve(stage);
        var result = pressOrder.Where(x => press[x - 1]).ToList();

        //Debug.LogFormat("Solution for stage {0} => PressOrder: {1}; ShouldPress: {2}; Result: {3};",
        //   stage,
        //   string.Join(",", pressOrder.Select(x => x.ToString()).ToArray()),
        //   string.Join(",", press.Select(x => x.ToString()).ToArray()),
        //   string.Join(",", result.Select(x => x.ToString()).ToArray())
        //);

        return result;      
    }

    public string DebugStringButtons(int index, int stage)
    {
        var button = this.buttons[index];
        var colorSolve = this.ColorSolve(index);
        var labelSolve = this.LabelSolve(index, stage);
        return string.Format("{0}({1}) {4} {2}({3}) == {5}",
            button.Color.ToString(),
            colorSolve,
            button.Label,
            labelSolve,
            this.gateOperator.Name,
            this.gateOperator.Operator(colorSolve, labelSolve));   
    }

    public string DebugOrderString(int stage)
    {
        return string.Format("{0}", string.Join(",", this.PressOrder(this.gateOperator.Group, stage).Select(x => x.ToString()).ToArray()));
    }

    private IList<bool> Solve(int stage)
    {
        var solution = new List<bool>();
        for (int i = 0; i < 3; ++i)
        {
            solution.Add(this.Solve(i, stage));
        }

        return solution;
    }

    private bool Solve(int index, int stage)
    {
        var colorSolve = this.ColorSolve(index);
        var labelSolve = this.LabelSolve(index, stage);
        //Debug.LogFormat("Solve for stage {0} and index {1} => Button {2}: Color {3} {4} Label {5} == {6}",
        //    stage, index,
        //    index + 1, colorSolve, this.gateOperator.Name, labelSolve, 
        //    this.gateOperator.Operator(this.ColorSolve(index), this.LabelSolve(index, stage)));
        return this.gateOperator.Operator(this.ColorSolve(index), this.LabelSolve(index, stage));
    }

    private IList<int> PressOrder(int group, int stage)
    {
        return pressOrders[group - 1, stage - 1];
    }

    private bool ColorSolve(int index)
    {
        var button = this.buttons[index];
        var otherIndexes = this.OtherIndex(index);
        switch (button.Color)
        {
            case ButtonColor.Red:
                return ColorLogic.IsRedConditionMet(this.buttons[otherIndexes[0]], this.buttons[otherIndexes[1]]);
            case ButtonColor.Blue:
                return ColorLogic.IsBlueConditionMet(this.buttons[otherIndexes[0]], this.buttons[otherIndexes[1]]);
            case ButtonColor.Green:
                var nextIndex = this.NextIndexInClocwiseOrder(index);
                return ColorLogic.IsGreenConditionMet(this.buttons[nextIndex]);
            case ButtonColor.Yellow:
                return ColorLogic.IsYellowConditionMet(button);
            case ButtonColor.Purple:
                return ColorLogic.IsPurpleConditionMet(this.buttons[otherIndexes[0]], this.buttons[otherIndexes[1]]);
            case ButtonColor.White:
                return ColorLogic.IsWhiteConditionMet(this.buttons[otherIndexes[0]], this.buttons[otherIndexes[1]]);
            case ButtonColor.Orange:
                return ColorLogic.IsOrangeConditionMet(this.buttons[0]);
            case ButtonColor.Cyan:
                return ColorLogic.IsCyanConditionMet(button);
            case ButtonColor.Grey:
                return ColorLogic.IsGreyConditionMet(button, new[] { this.buttons[otherIndexes[0]], this.buttons[otherIndexes[1]] });
            default:
                throw new InvalidOperationException();
        }
    }

    private bool LabelSolve(int index, int stage)
    {
        var button = this.buttons[index];
        var otherIndexes = this.OtherIndex(index);
        switch (button.Label)
        {
            case Constants.LogicString:
                return LabelLogic.IsLabelLogicMet(this.buttons);
            case Constants.ColorString:
                return LabelLogic.IsLabelColorMet(button);
            case Constants.LabelString:
                return LabelLogic.IsLabelLabelMet(this.buttons[0]);
            case Constants.ButtonString:
                return LabelLogic.IsLabelButtonMet(this.buttons[NextIndexInClocwiseOrder(index)]);
            case Constants.WrongString:
                return LabelLogic.IsLabelWrongMet(button, this.buttons[NextIndexInCounterClocwiseOrder(index)]);
            case Constants.NoString:
                return LabelLogic.IsLabelNoMet(index, this.buttons);
            case Constants.WaitString:
                return LabelLogic.IsLabelWaitMet(stage);
            case Constants.HmmmString:
                return LabelLogic.IsLabelHmmmMet(this.buttons);
            case Constants.BoomString:
                return LabelLogic.IsLabelBoomMet(this.buttons[otherIndexes[0]], this.buttons[otherIndexes[1]]);
            default:
                throw new InvalidOperationException();
        }
    }

    private int[] OtherIndex(int index)
    {
        return this.idx.Where(x => x != index).OrderBy(x => x).ToArray();
    }

    // Buttons are numbered in 'reading order' so clockwise means subtracting.
    private int NextIndexInClocwiseOrder(int index)
    {
        var nextIndex = index - 1;
        return (nextIndex < 0) ? 2 : nextIndex;
    }

    private int NextIndexInCounterClocwiseOrder(int index)
    {
        var nextIndex = index + 1;
        return (nextIndex > 2) ? 0 : nextIndex;
    }



}
