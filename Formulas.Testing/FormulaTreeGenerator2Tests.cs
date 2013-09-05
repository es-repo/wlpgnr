using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MbUnit.Framework;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.Formulas.Testing
{
    [TestFixture]
    public class FormulaTreeGenerator2Tests
    {
        [Test]
        public void TestGenerate()
        {
            IEnumerable<Variable> variables = EnumerableExtensions.Repeat(i => "x" + i.ToString(CultureInfo.InvariantCulture), 3)
                .Select(n => new Variable(n));

            IEnumerable<Operator> operators = new Operator[]
            {
                OperatorsLibrary.Sum, OperatorsLibrary.Mul, OperatorsLibrary.Sin, OperatorsLibrary.Ln
            };
        }
    }
}
