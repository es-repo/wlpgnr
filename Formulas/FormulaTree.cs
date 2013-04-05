namespace WallpaperGenerator.Formulas
{
    public class FormulaTree 
    {
        //private readonly FormulaTreeNode _root;

        //public FormulaTree(FormulaTreeNode root)
        //{
        //    _root = root;
        //} 

        //public IEnumerable<Variable> FindVariables()
        //{
        //    return FindVariables(_root);
        //}

        //private static IEnumerable<Variable> FindVariables(FormulaTreeNode node)
        //{            
        //    IEnumerable<FormulaTreeNode> parentAndChildren = Enumerable.Repeat(node, 1).Concat(node.Children);
        //    IEnumerable<Variable> variables = Enumerable.Empty<Variable>();
        //    foreach (FormulaTreeNode n in parentAndChildren)
        //    {
        //        Variable variable = ReturnVariableIfContain(n);
        //        if (variable != null)
        //            variables
        //    }
            
        //    if (node is OperandNode)
        //    {
        //        OperandNode operandNode = (OperandNode) node;
        //        if (operandNode.Operand is Variable)
        //            variables = variables.Concat(Enumerable.Repeat((Variable)operandNode.Operand, 1));
        //    }

        //    variables = variables.Concat();
        //    return variables;
        //}

        //private static Variable ReturnVariableIfContain(FormulaTreeNode node)
        //{
        //    if (node is OperandNode)
        //    {
        //        OperandNode operandNode = (OperandNode)node;
        //        if (operandNode.Operand is Variable)
        //            return (Variable)operandNode.Operand;
        //    }

        //    return null;
        //}
    }
}
