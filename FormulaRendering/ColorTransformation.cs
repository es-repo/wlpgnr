using System;
using System.Linq;  

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
            ColorChannelTransformation[] transformations = new []
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

        public static ColorTransformation CreateRandomPolynomialColorTransformation(Random random, int coefficientLowBound, int coefficientHighBound, double zeroChannelProbabilty)
        {
            ColorChannelTransformation redChannelTransofrmation = ColorChannelTransformation.CreateRandomPolinomialChannelTransformation(random, coefficientLowBound, coefficientHighBound, zeroChannelProbabilty);
            ColorChannelTransformation greenChannelTransofrmation = ColorChannelTransformation.CreateRandomPolinomialChannelTransformation(random, coefficientLowBound, coefficientHighBound, zeroChannelProbabilty);
            if (redChannelTransofrmation.IsZero && greenChannelTransofrmation.IsZero)
            {
                zeroChannelProbabilty = 0;
            }
            
            ColorChannelTransformation blueChannelTransofrmation = ColorChannelTransformation.CreateRandomPolinomialChannelTransformation(random, coefficientLowBound, coefficientHighBound, zeroChannelProbabilty);
            return new ColorTransformation(redChannelTransofrmation, greenChannelTransofrmation, blueChannelTransofrmation);
        }
    }
}
