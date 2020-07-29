
public static class LogicalGates
{
    public static bool And(bool op1, bool op2)
    {
        return op1 && op2;
    }

    public static bool Or(bool op1, bool op2)
    {
        return op1 || op2;
    }

    public static bool Xor(bool op1, bool op2)
    {
        return ((op1 && !op2) || (!op1 && op2));
    }

    public static bool Nand(bool op1, bool op2)
    {
        return !And(op1, op2);
    }

    public static bool Nor(bool op1, bool op2)
    {
        return !Or(op1, op2);
    }

    public static bool Xnor(bool op1, bool op2)
    {
        return !Xor(op1, op2);
    }

}
