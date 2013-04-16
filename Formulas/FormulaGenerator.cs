using System;
using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Utilities;
using System.Globalization;
using WallpaperGenerator.Formulas.Operators;

namespace WallpaperGenerator.Formulas
{
    public class FormulaGenerator
    {
        public static FormulaTreeNode CreateRandomFormula(int variablesCount, int constantsCount, int unaryOperatorsCountForFormulaDiluting,
            Operator[] operatorsLibrary, Operator[] constantsLibrary)
        {
            if (variablesCount < 0)
                throw new ArgumentException("Variables count can't be less then 0.");

            if (constantsCount < 0)
                throw new ArgumentException("Constants count can't be less then 0.");

            if (unaryOperatorsCountForFormulaDiluting < 0)
                throw new ArgumentException("Unary operators count can't be less then 0.");

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

        private static FormulaTreeNode CreateRandomFormulaCore(int variablesCount, int constantsCount, int unaryOperatorsCountForFormulaDiluting,
            Operator[] operatorsLibrary, Operator[] constantsLibrary)
        {                        
            Dictionary<int, Operator[]> availableOperatorsByArityMap = new Dictionary<int, Operator[]>();
            for (int i = 1; i < 4; i++)
            {
                availableOperatorsByArityMap.Add(i, operatorsLibrary.Where(op => op.Arity == i).ToArray());
            }

            int zeroOperatorsCount = variablesCount + constantsCount;
            double ternaryVsBinaryOperatorOccurenceProbability = (double)availableOperatorsByArityMap[3].Length /
                (availableOperatorsByArityMap[2].Length + availableOperatorsByArityMap[3].Length);

            Random random = new Random();
            int[] operatorsAritySequence = GetNonZeroOperatorsAritySequence(random, zeroOperatorsCount,
                unaryOperatorsCountForFormulaDiluting, ternaryVsBinaryOperatorOccurenceProbability).ToArray();
            IEnumerable<Operator> nonZeroOperators = operatorsAritySequence.Select(a => availableOperatorsByArityMap[a].TakeRandom(random));

            IEnumerable<string> variableNames = EnumerableExtensions.Repeat(i => "x" + i.ToString(CultureInfo.InvariantCulture), variablesCount);
            IEnumerable<Operator> variables = variableNames.Select(n => new Variable(n)).Cast<Operator>();
            IEnumerable<Operator> constants = EnumerableExtensions.Repeat(i => constantsLibrary[random.Next(constantsLibrary.Length)], constantsCount);
            IEnumerable<Operator> zeroArityOperators = variables.Concat(constants).Randomize(random); 

            //
            
            return null;
        }

        public static IEnumerable<int> GetNonZeroOperatorsAritySequence(Random random, int zeroArityOperatorsCount, 
            int unaryOperatorsCount, double ternaryVsBinaryOperatorOccurenceProbability)
        {
            if (zeroArityOperatorsCount < 1)
                throw new ArgumentException("Zero arity operators can't be less then 1.");

            if (unaryOperatorsCount < 0)
                throw new ArgumentException("Zero arity operators can't be less then 0.");

            if (ternaryVsBinaryOperatorOccurenceProbability > 0.99)
                throw new ArgumentException("Ternary VS binary operator occurence probability can't be more then 0.99.");

            while (zeroArityOperatorsCount > 1)
            {
                double unaryOperatorOccurenceProbability = (double)unaryOperatorsCount / (zeroArityOperatorsCount + unaryOperatorsCount);
                int arity = random.GetRandomBetweenTwo(2, 1, unaryOperatorOccurenceProbability);
                if (arity == 2 && zeroArityOperatorsCount > 2)
                {
                    arity = random.GetRandomBetweenTwo(2, 3, ternaryVsBinaryOperatorOccurenceProbability);
                }
                   
                yield return arity;
                zeroArityOperatorsCount -= (arity - 1);
                if (arity == 1)
                    unaryOperatorsCount--;
            }

            foreach (int i in Enumerable.Repeat(1, unaryOperatorsCount))
                yield return i;
        }
    }
}
