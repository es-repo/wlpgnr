using System;
using System.Linq;
using System.Globalization;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.FormulaRendering
{
    public class ColorChannelTransformation
    {
        public double _polinomCoefficcientA;
        public double _polinomCoefficcientB;
        public double _polinomCoefficcientC;
        
        public Func<double, double> TransformationFunction { get; private set; }

        public double DispersionCoefficient { get; set; }

        public bool IsZero
        {
            get { return _polinomCoefficcientA.Equals(0) && _polinomCoefficcientB.Equals(0) && _polinomCoefficcientC.Equals(0); }
        }

        public ColorChannelTransformation(Func<double, double> transformationFunction, double dispersionCoefficient)
        {
            TransformationFunction = transformationFunction;
            DispersionCoefficient = dispersionCoefficient;
        }

        private ColorChannelTransformation(double a, double b, double c, double dispersionCoefficient)
        {
            _polinomCoefficcientA = a;
            _polinomCoefficcientB = b;
            _polinomCoefficcientC = c;
            TransformationFunction = v => v * v * v * a + v * v * b + v * c;
            DispersionCoefficient = dispersionCoefficient;
        }

        public static ColorChannelTransformation CreateRandomPolinomialChannelTransformation(Random random, 
            int coefficientLowBound, int coefficientHighBound, double zeroChannelProbabilty)
        {
            double zeroChannel = random.NextDouble();
            if (zeroChannel < zeroChannelProbabilty)
                return new ColorChannelTransformation(0, 0, 0, 0);

            random.RandomlyShrinkBounds(ref coefficientLowBound, ref coefficientHighBound);

            double a = Math.Round(random.NextDouble() * random.Next(coefficientLowBound, coefficientHighBound), 2);
            double b = Math.Round(random.NextDouble() * random.Next(coefficientLowBound, coefficientHighBound), 2);
            double c = Math.Round(random.NextDouble() * random.Next(coefficientLowBound, coefficientHighBound), 2);

            if (a.Equals(0) && b.Equals(0) && c.Equals(0))
            {
                a = b = c = (coefficientHighBound - coefficientLowBound)/2.0;
            }

            double dispersionCoefficient = Math.Round(random.NextDouble(), 2);
            return new ColorChannelTransformation(a, b, c, dispersionCoefficient);
        }

        public override string ToString()
        {
            double[] coefficcients = {_polinomCoefficcientA, _polinomCoefficcientB, _polinomCoefficcientC, DispersionCoefficient};
            return string.Join(",", coefficcients.Select(c => c.ToString(CultureInfo.InvariantCulture)).ToArray());
        }

        public static ColorChannelTransformation FromString(string value)
        {
            string[] coeffficients = value.Split(new []{','}, StringSplitOptions.RemoveEmptyEntries);
            double a = double.Parse(coeffficients[0]);
            double b = double.Parse(coeffficients[1]);
            double c = double.Parse(coeffficients[2]);
            double d = double.Parse(coeffficients[3]);
            return new ColorChannelTransformation(a, b, c, d);
        }
    }
}
