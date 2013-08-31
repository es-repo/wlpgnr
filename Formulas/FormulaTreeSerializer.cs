using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace WallpaperGenerator.Formulas
{
    public static class FormulaTreeSerializer
    {
        public static string Serialize(FormulaTree formulaTree)
        {
            return TreeSerializer.Serialize(formulaTree.Root, op => op.Name);
        }

        public static FormulaTree Deserialize(string serialized)
        {
            List<Variable> availableVariables = new List<Variable>();
            TreeNode<Operator> root = TreeSerializer.Deserialize(serialized, s => OperatorFromString(s, availableVariables), op => op.Arity);
            return new FormulaTree(root);
        }

        private static Operator OperatorFromString(string str, ICollection<Variable> availableVariables)
        {
            Operator knownOperator = GetKnownOperator(str);
            if (knownOperator != null)
                return knownOperator;

            double constant;
            if (double.TryParse(str, NumberStyles.Number, CultureInfo.InvariantCulture, out constant))
            {
                return new Constant(constant);
            }

            Variable availableVariable = availableVariables.FirstOrDefault(op => op.Name == str);
            if (availableVariable == null)
            {
                availableVariable = new Variable(str);
                availableVariables.Add(availableVariable);
            }
            return availableVariable;
        }

        private static Operator GetKnownOperator(string str)
        {
            return OperatorsLibrary.All.FirstOrDefault(op => op.Name.Equals(str, StringComparison.OrdinalIgnoreCase));
        }
    }
}
