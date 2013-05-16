using System;

namespace WallpaperGenerator.FormulaRendering
{
    public class ColorChannelTransformation
    {
        public Func<double, double> TransformationFunction { get; private set; }

        public double DispersionCoefficient { get; set; }

        public ColorChannelTransformation(Func<double, double> transformationFunction, double dispersionCoefficient)
        {
            TransformationFunction = transformationFunction;
            DispersionCoefficient = dispersionCoefficient;
        }

        public static ColorChannelTransformation CreatePolynomialChannelTransformingFunction(double a, double b, double c, double dispersionCoefficient)
        {
            return new ColorChannelTransformation(v => v * v * v * a + v * v * b + v * c, dispersionCoefficient);
        }
    }
}
