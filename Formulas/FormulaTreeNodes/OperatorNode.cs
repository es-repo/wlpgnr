namespace WallpaperGenerator.Formulas.FormulaTreeNodes
{
    public class OperatorNode : FormulaTreeNode
    {
        public Operator Operator { get; private set; }

        public OperatorNode(Operator @operator)
        {
            Operator = @operator;
        }
    }
}
