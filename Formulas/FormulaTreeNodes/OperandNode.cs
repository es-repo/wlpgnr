namespace WallpaperGenerator.Formulas.FormulaTreeNodes
{
    public class OperandNode : FormulaTreeNode
    {
        public Operand Operand { get; private set; }

        public OperandNode(Operand operand)
        {
            Operand = operand;
        }
    }
}
