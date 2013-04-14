using System;
using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.Formulas
{
    public class FormulaGenerator
    {
        public static FormulaTreeNode CreateRandomFormula(uint variablesCount, uint constantsCount, uint unaryOperatorsCountForFormulaDiluting,
            IEnumerable<Operator> operatorsLibrary, IEnumerable<Operator> constantsLibrary)
        {
            if (variablesCount + constantsCount == 0)
                throw new ArgumentException("Sum of variables count and constants count can't be zero.");

            if (!operatorsLibrary.Any())
                throw new ArgumentException("Operators library can't be empty.");
            
            if (operatorsLibrary.Any(op => op.Arity == 0))
                throw new ArgumentException("Operators library can't contain operators with 0-arity.");

            if (variablesCount + constantsCount > 1 && operatorsLibrary.All(op => op.Arity != 2))
                throw new ArgumentException("If sum of variables count and constants more than 1 then operators library should contain binary operators.");

            if (unaryOperatorsCountForFormulaDiluting > 0 && operatorsLibrary.All(op => op.Arity != 1))
                throw new ArgumentException("If unary operators count for formula diluting is more then 0 then operators library should contina unary operators");

            if (constantsCount > 0 && !constantsLibrary.Any())
                throw new ArgumentException("Constants library can't be empty if constants count is more then 0.");

            return CreateRandomFormulaCore(variablesCount, constantsCount, unaryOperatorsCountForFormulaDiluting, operatorsLibrary, constantsLibrary);
        }

        private static FormulaTreeNode CreateRandomFormulaCore(uint variablesCount, uint constantsCount, uint unaryOperatorsCountForFormulaDiluting,
            IEnumerable<Operator> operatorsLibrary, IEnumerable<Operator> constantsLibrary)
        {
            //uint formulaLeafsCount = variablesCount + constantsCount;
            //IEnumerable<int> availableArities = operatorsLibrary.GroupBy(op => op.Arity).Select(g => g.Key).OrderBy(a => a);
            //int[] availableNonUnaryArities = availableArities.Where(a => a > 1).ToArray();
            //bool hasUnaryArity = availableArities.Any(a => a == 1);
            //bool h
            //int minNonUnaryArity = availableNonUnaryArities.Min();
            //Random rnd = new Random();
            //List<int> arities = new List<int>();
            //while (formulaLeafsCount > 1)
            //{
            //    int arity = availableNonUnaryArities[rnd.Next(availableNonUnaryArities.Length)];
            //}
            
            return null;
        }

        private static IEnumerable<int> GetOperatorsAritySequence(Random random, int zeroArityOperatorsCount, uint unaryOperatorsCountForFormulaDiluting, 
            bool useTernaryOperators, double ternaryVsBinaryOperatorOccurenceProbability)
        {
            if (zeroArityOperatorsCount <= 0)
                throw new ArgumentException("Zero arity operators count should be more then zero.");

            if (ternaryVsBinaryOperatorOccurenceProbability < 0 && ternaryVsBinaryOperatorOccurenceProbability > 1)
                throw new ArgumentException("Ternary vs binary operator occurence probability should be more then 0 and less or eqaul then 1.");
            
            if (zeroArityOperatorsCount == 1)
            {
                for(int i = 0; i > unaryOperatorsCountForFormulaDiluting; i++) 
                    yield return 1;
                yield break;
            }

            while (zeroArityOperatorsCount > 1)
            {
                int arity = 1;
                if (zeroArityOperatorsCount == 2 || !useTernaryOperators)
                {
                    arity = 2;
                }
                else
                {
                    arity = random.GetRandomBetweenTwo(2, 3, ternaryVsBinaryOperatorOccurenceProbability);
                }

                yield return arity;
                zeroArityOperatorsCount -= (arity - 1);
            }
        }
    }
}
