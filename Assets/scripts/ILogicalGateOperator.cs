using System;

public interface ILogicalGateOperator {
    bool Operator(bool op1, bool op2);

    string Name { get; }

    int Group { get; } 
}

public class AndGate : ILogicalGateOperator
{
    public bool Operator(bool op1, bool op2)
    {
        return LogicalGates.And(op1, op2);
    }

    public string Name
    {
        get
        {
            return Constants.AndOperatorString;
        }
    }

    public int Group
    {
        get
        {
            return 1;
        }
    }
}

public class OrGate : ILogicalGateOperator
{
    public bool Operator(bool op1, bool op2)
    {
        return LogicalGates.Or(op1, op2);
    }

    public string Name
    {
        get
        {
            return Constants.OrOperatorString;
        }
    }

    public int Group
    {
        get
        {
            return 1;
        }
    }
}

public class XorGate : ILogicalGateOperator
{
    public bool Operator(bool op1, bool op2)
    {
        return LogicalGates.Xor(op1, op2);
    }

    public string Name
    {
        get
        {
            return Constants.XorOperatorString;
        }
    }

    public int Group
    {
        get
        {
            return 1;
        }
    }
}

public class NandGate : ILogicalGateOperator
{
    public bool Operator(bool op1, bool op2)
    {
        return LogicalGates.Nand(op1, op2);
    }

    public string Name
    {
        get
        {
            return Constants.NandOperatorString;
        }
    }

    public int Group
    {
        get
        {
            return 2;
        }
    }
}

public class NorGate : ILogicalGateOperator
{
    public bool Operator(bool op1, bool op2)
    {
        return LogicalGates.Nor(op1, op2);
    }

    public string Name
    {
        get
        {
            return Constants.NorOperatorString;
        }
    }

    public int Group
    {
        get
        {
            return 2;
        }
    }
}

public class XnorGate : ILogicalGateOperator
{
    public bool Operator(bool op1, bool op2)
    {
        return LogicalGates.Xnor(op1, op2);
    }

    public string Name
    {
        get
        {
            return Constants.XnorOperatorString;
        }
    }

    public int Group
    {
        get
        {
            return 2;
        }
    }
}

public static class LogicalGateOperatorFactory
{
    public static ILogicalGateOperator Create(string operatorName)
    {
        switch (operatorName.ToUpperInvariant())
        {
            case Constants.AndOperatorString:
                return new AndGate();
            case Constants.OrOperatorString:
                return new OrGate();
            case Constants.XorOperatorString:
                return new XorGate();
            case Constants.NandOperatorString:
                return new NandGate();
            case Constants.NorOperatorString:
                return new NorGate();
            case Constants.XnorOperatorString:
                return new XnorGate();
            default:
                throw new InvalidOperationException();
        }
    }
}
