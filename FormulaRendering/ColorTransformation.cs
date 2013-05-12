using System;

namespace WallpaperGenerator.FormulaRendering
{
    public class ColorTransformation
    {
        public Func<double, double> TransformRedChannel { get; private set; }

        public Func<double, double> TransformGreenChannel { get; private set; }

        public Func<double, double> TransformBlueChannel { get; private set; }

        public ColorTransformation(Func<double, double> transformRedChannel, Func<double, double> transformGreenChannel, 
            Func<double, double> transformBlueChannel)
        {
            TransformRedChannel = transformRedChannel;
            TransformGreenChannel = transformGreenChannel;
            TransformBlueChannel = transformBlueChannel;
        }

        public static ColorTransformation CreatePolynomialColorTransformation(double ra, double rb, double rc,
            double ga, double gb, double gc,
            double ba, double bb, double bc
            )
        {
            Func<double, double> transformRedChannel = CreatePolynomialChannelTransformingFunction(ra, rb, rc);
            Func<double, double> transformGreenChannel = CreatePolynomialChannelTransformingFunction(ga, gb, gc);
            Func<double, double> transformBlueChannel = CreatePolynomialChannelTransformingFunction(ba, bb, bc);
            return new ColorTransformation(transformRedChannel, transformGreenChannel, transformBlueChannel);
        }

        private static Func<double, double> CreatePolynomialChannelTransformingFunction(double a, double b, double c)
        {
            return v => v * v * v * a + v * v * b + v * c;
        }
    }
}
