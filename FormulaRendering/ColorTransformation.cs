using System;
using System.Linq;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.FormulaRendering
{
    public class ColorTransformation
    {
        public ColorChannelTransformation RedChannelTransformation { get; private set; }

        public ColorChannelTransformation GreenChannelTransformation { get; private set; }

        public ColorChannelTransformation BlueChannelTransformation { get; private set; }

        public ColorTransformation(ColorChannelTransformation redChannelTransformation,
            ColorChannelTransformation greenChannelTransformation,
            ColorChannelTransformation blueChannelTransformation)
        {
            RedChannelTransformation = redChannelTransformation;
            GreenChannelTransformation = greenChannelTransformation;
            BlueChannelTransformation = blueChannelTransformation;
        }

        public override string ToString()
        {
            ColorChannelTransformation[] transformations =
            {
                RedChannelTransformation,
                GreenChannelTransformation,
                BlueChannelTransformation
            };

            return string.Join(";", transformations.Select(t => t.ToString()).ToArray());
        }

        public static ColorTransformation FromString(string value)
        {
            string[] channelTransformations = value.Split(new [] {';'}, StringSplitOptions.RemoveEmptyEntries);
            ColorChannelTransformation redChannelTransformation = ColorChannelTransformation.FromString(channelTransformations[0]);
            ColorChannelTransformation greenChannelTransformation = ColorChannelTransformation.FromString(channelTransformations[1]);
            ColorChannelTransformation blueChannelTransformation = ColorChannelTransformation.FromString(channelTransformations[2]);
            return new ColorTransformation(redChannelTransformation, greenChannelTransformation, blueChannelTransformation);
        }

        public static ColorTransformation CreateRandomPolynomialColorTransformation(Random random, Bounds coefficientBounds, double zeroChannelProbabilty)
        {
            ColorChannelTransformation redChannelTransofrmation = ColorChannelTransformation.CreateRandomPolinomialChannelTransformation(random, coefficientBounds, zeroChannelProbabilty);
            ColorChannelTransformation greenChannelTransofrmation = ColorChannelTransformation.CreateRandomPolinomialChannelTransformation(random, coefficientBounds, zeroChannelProbabilty);
            if (redChannelTransofrmation.IsZero && greenChannelTransofrmation.IsZero)
            {
                zeroChannelProbabilty = 0;
            }

            ColorChannelTransformation blueChannelTransofrmation = ColorChannelTransformation.CreateRandomPolinomialChannelTransformation(random, coefficientBounds, zeroChannelProbabilty);
            return new ColorTransformation(redChannelTransofrmation, greenChannelTransofrmation, blueChannelTransofrmation);
        }
    }
}
