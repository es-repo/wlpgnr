using System;
using System.Linq;
using System.Globalization;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.FormulaRendering
{
    public class ColorChannelTransformation
    {
        private readonly double _polinomCoefficcientA;
        private readonly double _polinomCoefficcientB;
        private readonly double _polinomCoefficcientC;
        
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

        public static ColorChannelTransformation CreatePolynomialChannelTransformation(double a, double b, double c, double dispersionCoefficient)
        {
            return new ColorChannelTransformation(a, b, c, dispersionCoefficient);
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
            double dispersionCoefficient = Math.Round(random.NextDouble(), 2);
            return new ColorChannelTransformation(a, b, c, dispersionCoefficient);
        }

        public override string ToString()
        {
            double[] coefficcients = new [] {_polinomCoefficcientA, _polinomCoefficcientB, _polinomCoefficcientC, DispersionCoefficient};
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
