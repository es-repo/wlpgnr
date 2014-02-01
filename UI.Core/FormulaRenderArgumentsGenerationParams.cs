using System.Collections.Generic;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.UI.Core
{
    public class FormulaRenderArgumentsGenerationParams
    {
        public Bounds<int> DimensionCountBounds = new Bounds<int>(4, 15);
        public Bounds<int> MinimalDepthBounds = new Bounds<int>(2, 2);
        public Bounds ConstantBounds = new Bounds(-10, 10);
        public Bounds ConstantProbabilityBounds = new Bounds(0, 1);
        public Bounds LeafProbabilityBounds = new Bounds(0, 0.3);
        public Bounds RangeBounds = new Bounds(-40, 40);
        public Bounds ColorChannelPolinomialTransformationCoefficientBounds = new Bounds(-10, 10);
        public double ColorChannelZeroProbabilty = 0.2;
        public Bounds UnaryVsBinaryOperatorsProbabilityBounds = new Bounds(0.4, 0.70);

        public IDictionary<Operator, Bounds> OperatorAndMaxProbabilityBoundsMap = new Dictionary<Operator, Bounds>
        {
            {OperatorsLibrary.Sum, new Bounds(0.25, 1)},
            {OperatorsLibrary.Sub, new Bounds(0.25, 1)},
            {OperatorsLibrary.Mul, new Bounds(0, 0.25)},
            //{OperatorsLibrary.Div, new Bounds(0, 0.5)},
            {OperatorsLibrary.Max, new Bounds(0, 0.15)},
            {OperatorsLibrary.Pow, new Bounds(0, 0.5)},

            {OperatorsLibrary.Abs, new Bounds(0, 0.15)},
            {OperatorsLibrary.Sin, new Bounds(0.15, 1)},
            {OperatorsLibrary.Cos, new Bounds(0, 1)},
            {OperatorsLibrary.Atan, new Bounds(0, 0.25)},
            {OperatorsLibrary.Ln, new Bounds(0, 0.5)},
            {OperatorsLibrary.Sqrt, new Bounds(0, 1)},
            {OperatorsLibrary.Cbrt, new Bounds(0, 1)},
            //{OperatorsLibrary.Pow2, new Bounds(0, 0.1)},
            //{OperatorsLibrary.Pow3, new Bounds(0, 0.1)},
        };

        public Operator[] ObligatoryOperators = {OperatorsLibrary.Sin, OperatorsLibrary.Sum};

        public bool PredefinedFormulaRenderingArgumentsEnabled = true;
        public int FirstPredefinedFormulaRenderingArgumentsCount = 3;
        public int RepeatPredefinedFormulaRenderingArgumentsAfterEvery = 5;
        public string[] PredefinedFormulaRenderingFormulaRenderArgumentStrings =
        {
@"-0.61,1.32;1.32,1.54;-5.75,0.06;-6.71,-3.6;-1.85,-0.57;5.91,8.52;7.51,7.73;-2.26,6.17
0.5,0.14,-0.77,0.68;3.47,-0.57,-6.58,0.33;-0.03,-1.01,-1.3,0.14
Sin Sin Sin Cbrt Mul Sum Mul Sin Mul Sin Sum Cbrt x1 Sin x4 Cbrt Sin Sin x2 Cbrt Mul Sin Sin Mul x5 x7 Sin Sum Sin x4 Sin x5 Sum Sin Sum Sin Sum Sum x4 x2 Sin x1 Sin Mul Sin x2 Mul x1 x3 Sum Sin Sin Sin Sin x3 Sin Sin Sum Sin x7 Sin x4 Sin Sum Sum Sin Sum Sin Sin x2 Mul Cbrt x4 Cbrt x6 Sin Sum Mul Sum -6.91 x0 Sin x4 Sum Sin x6 Sin x4 Sum Sin Mul Sin Mul x4 x7 Sum Sum x6 x6 Cbrt x6 Mul Sin Sum Sin x1 Sin x5 Sin Mul Sum x1 x7 Mul x0 x7",

@"-1.54,1.23;-14.78,-8.19;-1.64,4.31;-3.31,0.31
-3.08,-1.92,-0.2,0.13;-1.99,-0.96,-2.41,0.11;-2.3,0.15,-0.63,0.01
Sum Sqrt Sin Sum Sum Sin Sum Sin Sum x3 x2 Sum Sum x1 x0 Sum x1 x2 Sum Sin Sin Mul x2 x3 Sum Max Max x1 x3 Sum x3 x3 Sin Sin x3 Sum Sin Sum Sum Max x3 x2 Sum x1 x3 Sqrt Sum x0 x0 Max Sin Sin Sum x1 x1 Sum Sum Sum x0 x2 Sum x2 x1 Sqrt Sin x2 Sin Sin Sin Sqrt Sum Sin Sin Max x0 x0 Sin Sum Sin x2 Sum x1 x3",

@"1.12,1.37;0,3.8;-1.29,-0.05;-0.67,-0.18;-4.11,2;-0.75,0.81;-0.06,0.52;-1.56,3.08;-1.96,-0.41
-0.24,0.51,0.07,0.31;2.25,0.36,0.42,0.43;0,0,0,0
Sub Sum Cbrt Cos Sub Sum Sum Sum Cbrt Atan Pow x5 x5 Sum Sum x6 x2 Sqrt x3 Atan Atan Pow2 x6 Cbrt Cbrt Sum Atan Pow2 x2 Sqrt x1 Sub Atan Pow x4 Sum Atan x4 Atan Pow x1 x2 Atan Sqrt Cbrt Atan Pow3 x0 Atan Pow Sum Sub Cbrt Cbrt Atan Pow3 x3 Sum Atan Pow2 Atan Pow x0 x0 Sub Atan Pow x1 x3 Atan Pow -3.5 x8 Sum Sub Atan Ln Cbrt x4 Atan Pow x5 Atan x5 Atan Pow2 Sum Atan Pow2 x4 Sum x7 x7 x7 Atan Ln Max Sum Sum Atan Pow2 Cos Sub x2 x5 Cos Atan Pow x8 Sqrt x3 Sum Sqrt Max Atan Ln x1 Sum Sum x3 x8 Sub x2 x7 Cbrt Cos Max Sum x4 x3 Sub x2 x6 Sub Sum Atan Pow Sub Cbrt x5 Cos x3 -9.9 Sub Cbrt Cbrt Cos x8 Atan Pow Sqrt x4 x7 Sub Cbrt Sum Atan Cos x8 Sub Cos x4 Cbrt x7 Atan Ln Sqrt Atan Pow2 x2",                                                                                                                                                                                                                                                                                                                                                                                                                                       

@"-9.27,2.72;-7.07,-3;-4.5,-3;2.55,10.18;-7.28,10.09;-10.93,-3.73;-0.26,12.38;-4.06,-1.65;1.46,9.21;-6.95,12.64;-0.29,10.4
0.42,-1.83,-7.08,0.43;-0.35,-2.19,0.19,0.02;-1.62,-3.07,3.7,0.01
Sin Sum Sqrt Sum Sin Sqrt Sum Sqrt Sum x3 x5 Sum Sum x12 x10 2.31 Sqrt Sqrt Sin Sum Sqrt x11 Sum x13 x7 Max Sqrt Sin Sum Sqrt Atan Pow -6.05 x1 x0 Sin Sqrt Sin Sum Sum x12 Sum x6 x11 Sum Sin x9 Sin x10",

@"-6.37,4.96;-1.99,2.45;3.27,4.03;-19.4,-0.83;-10.45,2.98;-10.12,-8.42;-3.37,7.27
0,0,0,0;0.05,-0.21,0.7,0.09;0.97,0.38,1.19,0.7
Atan Sub Sin Cbrt Sub Cbrt Sub Atan Sub Sum x4 x6 Sub x1 x1 Cbrt Sum Mul x1 x3 Sub x0 x6 Sub Sin Sub Sin Sub x0 x4 Cbrt Sub x1 x3 Sub Cbrt Sub Sub x3 x4 Sin x1 Cbrt Sub Sub x2 x4 Cbrt x3 Cbrt Sub Mul Sub Cbrt Sum Cbrt Sub x6 x4 Sub Cbrt x5 Cbrt x3 Sin Sum Sin Sin x3 Mul Sin x5 Sin x0 Sub Sin Sub Mul Sub x0 x4 Sum x5 x1 Sub Sub x1 x6 Cbrt x4 Sub Sin Cbrt Cbrt x5 Cbrt Sub Sub x1 x5 Cbrt x0 Cbrt Sin Mul Cbrt Sum Cbrt x0 Sin x0 Sub Cbrt Sum x0 x4 Sub Sub x3 x0 Sub x3 x2",

@"-7.76,1.86;-6.12,10.89;-0.75,2.46;-4.96,15.44;-4.42,-2.94;-2.14,0.84;-12.86,-7.97;-17.97,-3.81;1.23,3.72
-0.02,1.23,4.45,0.23;0.49,-2.42,-1.75,0.8;-1.54,0,-0.03,0.74
Sum Cbrt Sin Atan Pow3 Sum Mul Atan Cbrt Sum Cbrt Sum x3 x1 -0.48 Atan Cbrt Cbrt Sum Atan Pow3 x7 4.49 x3 Sum Atan Sum Atan Mul Cbrt Cbrt Sum Cbrt Sum Cbrt x6 9.59 Mul Cbrt Atan Pow3 x8 Sum Sum x0 -8.62 Sum x4 x1 Sin Sum Sum Sum Sin Mul x6 x8 Sum Sum x7 x8 Mul x2 x5 Sin Sum Sin x8 Mul x6 x4 x3 Cbrt Cbrt Mul Sum Cbrt Atan Sum Mul x8 x8 Atan Pow3 x3 5.3 x2 Sum Sin Sum Cbrt Cbrt Atan Pow3 Cbrt Sum Atan x0 Sin x7 Cbrt Sum Sum Sum Atan Pow3 Sum x2 x1 Cbrt Sin Sin x6 Atan Sin Cbrt Sum x3 x4 Sum Mul Atan Pow3 Sum x3 x7 Sum Atan Pow3 x7 Cbrt Sum x1 x5 Sum 2.04 Mul Sum Atan x0 Atan x8 Sum Cbrt x0 Cbrt x5 Atan Sum Sum Atan Sum Mul Sum Sum Sum x8 x6 x6 Sum Sum x6 x1 Atan Pow3 x7 Atan Pow3 Cbrt x2 Cbrt Sum x8 Sum 1.07 Sum x3 x1 Cbrt Mul Cbrt Mul Cbrt Cbrt x0 Mul Sum x1 x4 Sin x5 Cbrt Cbrt Mul Cbrt x1 Sin x1 Cbrt Sum Sum Cbrt Sum Cbrt Atan Pow3 x7 Sin Atan Pow3 x2 Sum Sum Cbrt Sin x2 Sum Sum x1 x8 Cbrt x8 Sin Atan Pow3 x6 Sum Cbrt Atan Pow3 Sin x2 Cbrt Mul Cbrt Sum x5 x0 Cbrt Atan x3",

@"-3.08,-2.19;-5.43,-1.58;-6.76,0.2;0.99,4.71;-0.04,1.49;-1.49,-0.34;-4.64,0.01;-4.65,6.26;-4.16,-2.05
-2.2,-3.22,0.14,0.21;4.34,-4.43,-1.05,0.13;0,0,0,0
Cbrt Mul Sub Sqrt Sub Atan Ln Sub Pow -6.9 Atan x1 Sub x2 x4 Atan Sum Sub Sqrt x3 Sin x2 Sum Mul x7 x0 Sum x2 x8 Atan Ln Atan Ln Atan Sum x6 x1 Sin Sub Sub Sqrt Sqrt Sub Sub x5 x5 Sin x7 Sin Atan Sum Atan Pow x7 7.85 Sub x2 x5 Atan Pow2 Sub x3 Sin Sqrt x0",

@"-0.24,12.65;-4.91,-1.21;-0.56,19.56;-2.07,1.41;0.44,1.4;-14.44,0.66;-12.22,3.92;-2.15,3.36;-16.72,-15.65;-4.12,-0.98;-2.99,16.71
-0.4,-0.48,-0.23,0.33;0.13,0.33,-0.38,0.07;1.7,0.44,0.78,0.48
Sin Sum Sum Sin Sum Sin Atan Ln Sum Atan Pow2 Sum Sin x6 Atan x9 Sin Sum Abs Sum x6 x1 Sum Atan x5 Atan Pow2 x4 Sum Sum Sum Mul Atan Sin Atan Pow2 x3 Cos Mul 5.87 Sum Sum x8 x7 Sin x0 Cos Sum x3 Atan Sum Atan Ln x9 Sum x0 x8 Sum Sum Sum Sum Atan Ln x6 6.38 Sin Sum Sum x10 x10 Sum 7.25 x0 Sum Sum Cos Sum x9 x10 Atan Pow2 x9 Sum Sum Sum x5 x9 Sum -6.74 x7 Atan Pow2 x10 Sin Sin Atan Pow2 Sin x8 Atan Pow2 Sin Atan Sum Atan Ln x4 1.89 Cos Sum Sum Sum Sum Sin Atan Ln Sum 0.84 Sin x6 Sum Atan Ln Sum Sin x1 -7.69 Atan Ln Atan Pow2 x1 Cos Sum Atan Pow2 Sin Atan Ln x5 Sin Atan Sum Cos x10 Atan Ln x6 Atan Pow2 Atan Pow2 Sin Cos Sin x0 Atan Pow2 Atan Ln Sum Atan Pow2 Sum x10 x10 Atan Sin Atan x2 Atan Pow2 Sum Cos Sin Mul Mul Sum Atan Ln Sum x1 x5 Atan Ln Sum x2 x0 Sin Sum Sum Atan Pow2 x5 Atan Ln x4 Sum Sum x4 x9 Atan Pow2 x3 Mul Sum Sum Sin Atan x10 Sum Sum x10 x8 -4.28 Atan Pow2 Atan Pow2 x5 Atan Atan Pow2 Sin x7 Sum Sum Sum 7.07 Cos Atan Atan Ln Sin x9 Mul Atan Atan Sin Sum Sum x9 x6 Sum -7.88 x3 Sum Atan Sum Cos Atan Ln x0 Cos Atan Ln x4 Atan Pow2 Atan Ln x6 Sum Sum Atan Ln Sum 9.52 Sum Sum x8 x7 Sum x5 x10 -6.5 Sum Sin Sum Sum Sin Sum x9 x2 Sin Sum x10 x9 -3.33 Sum Cos Cos Cos Sum x0 x9 Sum Sin Atan Sin x3 Sum Sum Sum x4 x3 6.13 Atan Sum x7 x0",

@"-0.92,2.65;-4.8,-2.64;9.67,23.29;13.75,32.32;-4.97,4.78;0.12,0.38;-23.54,10.18
-0.74,6.73,-2.97,0.16;-0.01,1.38,0.51,0.12;-0.28,1.37,0.24,0.18
Sub Sum Sub Sin Sin Cbrt Sub Sub Sum Sin Cbrt Atan Pow2 x2 Sum Sub Cos Sqrt x0 x1 Atan Pow3 Sub x5 x6 Atan Sqrt Sqrt Cos Sqrt x5 Cbrt Mul Sqrt Sub Sub Sqrt x3 Cbrt x1 Mul Sub x0 x2 Sub x1 x2 Cbrt Sqrt Atan Ln x3 Sub Sub Sin Sqrt Sin Sub Sin Sum Sub Sum x6 x6 Sin x3 Sqrt Cos x0 Sqrt Sub Sin Atan Ln x2 Sin Cbrt x0 Sub Sqrt Mul Atan Pow3 Atan Sub Sub x0 x6 Sub x4 x6 Sin Cbrt Atan Pow2 Sin x6 Sin Sub Mul Sub Sub Sub Sqrt x4 Sub x2 x0 Abs Sub x1 x5 Sub Cbrt Sub x1 x1 Sub Atan Pow2 x6 Sub x1 x0 Sin Cbrt Sub Atan Pow2 x1 Mul x4 x3 Sub Cbrt Sum Sin Sub x3 x6 Sin Sub x1 x6 Sqrt Abs Sqrt Atan Ln x6 Sub Sub Sub Sub Cos Sin Abs Atan Pow2 x2 Sin Sqrt Sub Mul Abs x0 Cbrt x1 Sub Cbrt x2 Sin x5 Sin Cos Cbrt Sub Abs Sub x0 x2 Sub Sub x6 x1 Sqrt x3 Atan Pow3 Sub Sub Sin Mul Mul x3 x0 Atan Pow3 x6 Sub Sqrt Sub x4 x5 Sqrt Sqrt x0 Sub Cbrt Sub Cos x6 Sub x1 x1 Sqrt Sqrt Atan Pow3 x0 Atan Atan Pow3 Sub Mul x5 Atan Atan Ln x2 Sin Sub Sub Atan Pow3 x2 Cbrt x5 Sin Sum x4 x4 Sin Sub Abs Sin Sub Sub Sub Atan Pow2 Atan Pow2 x2 Atan Sub Sub Sub x1 x1 Atan Pow3 x5 Sqrt Cbrt x5 Sqrt Sqrt Sub Cos Mul x2 x3 Cos Atan Pow2 x0 Cos Sub Abs Sub x5 Sin Atan Pow2 x5 Mul Sub Sub Mul x4 x4 Cbrt x4 Mul Sum x2 x1 Mul x1 x2 Sqrt Sub Sin x0 Cos x3 Cbrt Sub Sin Sin Sub Sin Atan Ln Mul x1 x5 Sub Sum Sub Sub x2 x4 Sub x1 x2 x3 Sub Sub Sqrt x2 Sub x5 x2 Sin Sin x0 Sub Sin Mul Sub Sub Cos Sub x5 x3 Sub Sum x4 x0 Cbrt x1 Atan Pow3 Atan Pow2 x4 Sub Sub Sub Sqrt x4 Abs x6 Sub Cbrt x5 Sqrt x5 Sin Sin Sub x5 x1 Sub Sub Mul Sub Sub Cbrt x3 Mul x6 x5 Sum Sqrt x0 Atan Pow2 x4 Sqrt Sub Atan Pow3 x4 Sqrt x6 Sub Sqrt Atan Pow3 x5 Mul Sub Sub x0 x4 Atan Pow3 x6 Cos Sub x3 x4 Sub Atan Mul Cos Sub x3 x1 Sub Sqrt x6 Cbrt x3 Sum Cbrt Sin Sub x2 x6 Sin Mul Sum x5 x2 Sub x1 x3 Atan Pow3 Sub x6 Cos Sin Sin Sqrt Atan Pow2 Cbrt Mul Sin x4 Atan Pow3 x4",


@"-2.04,1.11;-6.78,-1.01;-0.5,-0.15;3.07,4.5;-3.63,-2.85;-6.24,1.32;-1.25,1.51;-2.99,5.39
-0.55,4.7,-0.48,0.8;-0.26,-0.01,-0.16,0.19;-0.13,-0.19,-0.23,0
Sub Sin Sin Sub Sum Atan Ln Sub Sub Sin Sub x7 x1 Sub Max x0 x6 x2 Sqrt Sub Sqrt x6 Sub x5 x6 Sum Atan Ln Max Sub Sqrt x1 Sub x4 x3 Sqrt Sub x3 x0 Sin Sin Sub -8.11 Sub Sub x4 x0 Atan Pow2 x7 Sum Sub Sub Sub Sub Sin Sub x4 x4 Sub Sin x0 -3 Sub Sum Atan Ln x7 Sin x7 Sqrt Sub x4 x3 Sin Atan Ln Sin x3 Sum Sin Max Sub Sub x6 x3 Sin x5 Sub Sin x5 Sin x6 Sub Sub Sum Sqrt x5 Sin x7 -4.79 Sub Sin Sub x5 x5 Sub Sum x6 x1 Sin x3 Sub Atan Ln Sub Sub Sum x2 -7.66 Atan Pow2 x4 Sin Sub x2 x3 Sin Sin Sub Atan Pow2 x3 Atan Pow2 x1 Sub Sqrt Sin Sin Sub Sub Sub Sub Sum Max x6 x4 Sub 2.48 x4 Sub Sub x0 x6 Sqrt x6 Sqrt Sub Sin x6 Sub x1 x4 Sqrt Atan Ln Sin x7 Sub Sub Sqrt Sub Sub x4 x6 Atan Pow2 x0 Sub Sin Atan Ln x7 Sum Sin x2 Sub x2 x2 Sub Sub Sub Sub x3 x6 Sin x2 Sub Sub x2 x6 -9.39 Sub Sub x6 Sub x3 x2 Sub Sin x3 Sum x6 x1 Sub Sin Sin Atan Ln Atan Ln Sin Sqrt x6 Sub Atan Ln Sub Sin Sub Sin Sub x7 x7 Sub Sub x7 x1 x0 Sqrt Sub Atan Pow2 x7 Sub Atan Pow2 x2 5.67 Sin Sum Sin Atan Pow2 Sqrt Sqrt x4 Sum Sub Sub Sub Atan Ln x6 Sub x6 x0 Sum Sub x5 x7 Sub x2 x7 Atan Pow2 Sum x2 x5 Sub Sub Sqrt Sub x3 x1 Sin Sqrt x5 Sin Sin Sub x0 x4",


@"-9.33,2.22;-11.22,0.8;-2.91,0.44;-3.69,-2.28
0,0,0,0;0.02,0.08,-0.13,0.04;0.09,1.31,-0.17,0.73
Sub Sin Sin Atan Pow2 Cbrt Sub Atan Pow2 Sub Sin Sum Cbrt x3 Sub x1 x2 Sub Sub Sub x1 x3 Atan Pow3 x1 Atan Pow3 x1 Sin Sub Sub Sub Cbrt Sum x2 x3 Sub Sub x3 x3 Sub x1 x0 Sub Sub Sub x2 x3 Sum x2 x0 Sum Atan Ln x0 Atan Ln x1 Cbrt Sin Atan Pow3 x3 Sub Sin Atan Pow3 Sub Atan Ln Sin Sub Sub Sum Atan Ln x1 Cbrt x1 Atan Pow2 x3 Atan Pow2 Atan Pow3 x3 Sub Sub Sum Atan Pow3 Sum Sub x1 x2 Sub x1 x3 Sub Sub Sub Sub x3 x1 Sin x2 Sub Sin x0 Sub x0 x1 Sub Sub Sum x3 x3 Sub x2 x0 Sub Sum x2 x2 Atan Pow3 x2 Sub Cbrt Sum Cbrt Sub x2 x2 Sqrt Atan Pow3 x2 Sum Sub Atan Pow3 x1 Sub Sum x0 x1 Atan x1 Atan Pow3 Sum x1 x0 Sum Sub Sum Sub Sum Atan Ln x1 Sub x2 x1 Sin Sub x2 x3 Sum Atan Pow2 x2 Atan Sum x2 x0 Sin Atan Pow2 Sin x1 Sub Sin Sum Sub Sum x2 x1 Sin x3 Atan Pow3 x3 Sum Atan Pow2 Sum x3 x0 Sum Sin Atan x1 Sub Sum x2 x1 Sub x3 x2 Atan Pow2 Sum Sqrt Sub Sin Sin Atan Pow2 Sub Sub x1 x3 Sum x2 x0 Sum Sub Sin Sub Cbrt Sub x2 x1 Sub Sub x3 x0 Sub x3 x2 Sub Sub Sub Sum x0 x3 Atan Pow3 x0 Atan Pow2 x1 Sub Sub Sum x0 x1 Sum x0 x1 Sub Sub x2 x2 Sum x1 x1 Sum Sub Sum Sub Sub x2 x0 Sum x0 x0 Sin Sum x3 x2 Sub Sum Atan Ln x0 Sin x2 Sub Atan Pow3 x1 Cbrt x3 Atan Pow3 Atan Ln x3 Sub Cbrt Sub Sum Cbrt Sin Cbrt Sub x1 x2 Cbrt Sub Cbrt Sub x2 x3 Sum Sub x2 x2 Sub x2 x1 Sub Atan Ln Cbrt Sub x3 x1 Sum Sqrt Sum Atan Ln x2 Atan Pow3 x1 Sin Sub Sub x0 x0 Sin x1 Atan Ln Sum Sub Atan Pow2 Atan Pow2 x3 Cbrt Sub Sin x2 Sub x1 x3 Sub Sin Atan Pow2 x2 Atan Pow2 Sin x2",

@"-4.34,-3.04;-1.57,7.3;-6.52,3.75;3.33,8.63
0.03,1.22,-0.51,0.45;0.29,4.18,3.66,0.16;-0.05,1.4,1.25,0.14
Sum Cos Sub Sqrt Cbrt Atan Pow3 Sub Sqrt Sub Mul Sum Sum x2 x0 Sin x1 Atan Ln x1 Sqrt Mul Sub x1 x1 Sum x0 x1 Sqrt Sum Mul Cos Cos x3 Atan Pow3 x2 Cos Atan Pow3 x3 9.7 Sub Sqrt Sqrt Sin Atan Pow3 Sqrt Sub Atan Pow3 Sum Sqrt x2 Atan Pow3 x2 Sqrt Atan Pow3 Atan Pow2 x3 Cos Sum Sum Sum Sqrt Sum Sub Sum Atan Pow3 Atan Ln x2 Sqrt Sin Cos x2 Sum Atan Pow3 Atan Pow3 x2 Sin Cos Sin x1 Cos Sub Sum Sqrt Sqrt x2 Cos Sub x3 x1 Sqrt Cos Sub 1.86 x2 Cos Sum Sub Cos Cos Sqrt Mul x1 x3 Atan Ln Atan Ln x1 Cos Sub Sin Sqrt Sub x2 x0 Atan Pow3 Mul x2 x2 Cos Sqrt Atan Pow3 Sin Atan Pow2 Sub x1 x1 Atan Pow3 Sum Sum Sqrt Sub Atan Pow3 Sum x3 x1 Sub Sub Atan Pow3 x3 Mul x1 x2 Atan Pow2 x2 Atan Pow3 Sqrt Sqrt Mul x3 x0 Mul 3.77 Atan Pow3 Atan Ln Cos x1",

@"-6.88,7.67;2.29,11.81;0.01,9.58;-5.21,1.68;0.55,0.98;0.02,7.2;-10.68,-9.11;-9.71,9.9;0.15,2.66;-3.82,-0.12;-0.83,0.1;-7.88,4.84;-5.82,-1.89
-1.99,-3,-2.13,0.46;-0.71,-0.15,-0.28,0.74;0.16,-0.36,-0.69,0.04
Sqrt Sub Sub Sub Atan Pow2 Sqrt Sub Sqrt Sub x5 x2 Sum Sub x8 x3 Sin x8 Sqrt Sqrt Sub Sqrt Sqrt Sub x6 x6 Sin Sqrt Sum x0 x10 Sub Sub Sub Sqrt Sub Sin Sqrt x6 Sin Sub x7 x10 Sin Sqrt Sin Sqrt x9 0.05 -1.04 Sqrt Sqrt Sub Sin Sqrt Sqrt Sqrt Sqrt x4 Sum Sub Sin Sqrt Sub x6 x8 Sub Sin Sqrt x6 Sqrt Sub x11 x4 Atan Pow2 Sub Sqrt x1 Sqrt x12",

@"-2.38,-0.01;-1.85,3.57;0.11,2.6;-4.52,1.92;-0.98,0.12;-4.21,2.51;-2.48,6.78;-1.19,3.32;-2.13,4.22;-2.76,-0.63;-4.37,2;-0.4,4.93;-3.41,0.24
1.5,7.23,-2.17,0.28;-0.6,2.44,-4.34,0.05;-0.47,-0.65,-0.12,0.44
Sqrt Sub Sub Sub Atan Pow2 Sqrt Sub Sqrt Sub x5 x2 Sum Sub x8 x3 Sin x8 Sqrt Sqrt Sub Sqrt Sqrt Sub x6 x6 Sin Sqrt Sum x0 x10 Sub Sub Sub Sqrt Sub Sin Sqrt x6 Sin Sub x7 x10 Sin Sqrt Sin Sqrt x9 0.05 -1.04 Sqrt Sqrt Sub Sin Sqrt Sqrt Sqrt Sqrt x4 Sum Sub Sin Sqrt Sub x6 x8 Sub Sin Sqrt x6 Sqrt Sub x11 x4 Atan Pow2 Sub Sqrt x1 Sqrt x12",

@"-2.23,-0.06;-3.8,1.93;-5.48,6.55;-1.77,0.51;2.09,4.83;-0.23,-0.03;-2.17,0.9;-1.68,-0.45;0.36,1.08;1.22,2.47;6.53,7.02;-0.64,1.79;-2.75,2.07
1.06,-0.11,3.57,0.04;0,0,0,0;-0.59,2.83,3.42,0.56
Sqrt Sub Sub Sub Atan Pow2 Sqrt Sub Sqrt Sub x5 x2 Sum Sub x8 x3 Sin x8 Sqrt Sqrt Sub Sqrt Sqrt Sub x6 x6 Sin Sqrt Sum x0 x10 Sub Sub Sub Sqrt Sub Sin Sqrt x6 Sin Sub x7 x10 Sin Sqrt Sin Sqrt x9 0.05 -1.04 Sqrt Sqrt Sub Sin Sqrt Sqrt Sqrt Sqrt x4 Sum Sub Sin Sqrt Sub x6 x8 Sub Sin Sqrt x6 Sqrt Sub x11 x4 Atan Pow2 Sub Sqrt x1 Sqrt x12",

@"-0.92,-0.39;0.23,0.74;-5.12,0.38;0.85,2.89
0.02,-0.04,-0.09,0.07;0,0,0,0;-0.82,0.14,0.02,0.14
Cos Sub Sub Atan Ln Cos Atan Ln Sum x2 Cos Sum Cos x1 Mul x2 x3 Atan Mul Sum Cos Max Sum Sub Sum Sub x2 x3 Sum x0 x1 Cos Atan Ln x2 Atan Pow x2 Atan Ln x2 Cos Sin Cos Sub x3 x3 8.62 Atan Ln Sub Cos Sub Cos Cos x1 Sub Sub x1 x0 Sum x0 x1 Atan Sub Max Mul x3 x3 Max x1 x1 Cos Sub x0 x1 Cos Sum Mul Sub Cos Sum -3.35 Atan Cos Sub Cos x1 Atan Ln x3 Mul Sub Sum Sub Atan Ln x0 Mul Atan x1 Sum -4.38 x0 Sub Sub Mul x1 -2.92 Cos x3 Atan Sin x3 Cos Atan Atan Sum x1 x2 Cos Atan Ln Sum Cos x2 Sum x0 x2 Atan Atan Sum Atan Ln Sum Sub x3 1.54 Cos x3 Mul Cos Cos Sin x2 Cos Cos Sub x0 x2 Sub Sum Mul Sub Mul Cos Cos Sub x2 x0 Sin Cos Cos x0 x2 Cos Cos Cos Sub Cos x1 Atan Pow x2 x1 Sub Sub Cos Atan Atan Ln x2 Atan Ln Cos Atan x3 Sin Cos Cos Sub Atan x0 Mul x0 x0 Mul Cos Sub Sin Cos Sin Sub x1 x1 3.83 Mul Atan Cos Sub Atan Ln x0 Mul Cos x1 Cos x2 Atan Ln Sub Cos Cos x0 Sin Sum x0 -9.4",

@"0.74,8.77;-6.2,10.21;2.69,12.4
2.51,-2.49,2.65,0.28;-0.83,1.38,1.22,0.19;0.62,-0.6,-0.12,0.35
Cbrt Sum Sin Atan Pow Cos Cos Atan Pow2 Sum Cbrt Sum x2 x2 Atan Pow2 x2 1.81 Atan Ln Sum Sum Sin Sum 9.14 Sum Sum Cbrt Sum x0 x1 Cos Sum x1 x0 Cos Sin Sin x0 Sin Sin Atan Ln Mul Sum x0 x1 Atan Pow2 x0 Atan Pow2 Atan Sin Sum Cos Sum x2 x1 Cos Sin x1",

@"-1.37,9.04;-8.94,-2.52;-7.65,0.42;-3.03,3.88
0.83,0.38,-1.38,0.36;0,0,0,0;0.74,-0.45,0.59,0.85
Cbrt Cos Abs Sum Sum Atan Pow2 Sum Abs Sin Sin Atan Pow3 x1 Cos Cos Sum Sum x2 x2 Sin x1 Cos Sum Sum Atan Pow2 Cos Atan Pow2 x0 Cbrt Sum Cbrt Sum x2 x2 Sum Sum x3 x3 Atan Pow2 x0 Cos Sum Sum Sum Sin x3 Sum x0 x1 Sin Sum x0 x2 Sum Atan Pow2 x3 Sum Sum x3 x3 Abs x1 Atan Pow3 Cbrt Sum x1 Cos Atan Pow2 Sum x0 x1",

@"-3.44,0.69;-0.37,10.03;-0.11,0.84;-0.94,3.61
-3.61,-5.69,3.12,0.11;-0.04,0.33,-0.92,0.22;-0.38,-0.19,3.43,0.11
Sub Sub Sub Sub Sub Sub Sub -3.26 Sub Atan Ln x1 Sub Atan Pow3 x3 Atan Ln x1 Cbrt Sin Atan Pow3 x1 Sub Cbrt Sum Atan Pow3 x2 Sub Sub x0 x3 Sub x0 x0 Sub Cbrt Sub Atan x2 Sub x0 x3 Cbrt Sum Sub x3 x1 Cbrt x0 Sub Sin Atan Sin Cbrt Cbrt x1 Sub Atan Cbrt Cbrt Sub x0 x2 -9.12 Sub Sub Mul Cbrt Atan Ln Sub x0 x1 -0.85 -4.86 Atan Pow3 Mul Atan Pow3 Abs x1 Cbrt Cbrt Cbrt x1 Atan Pow3 Atan Pow3 Atan Atan Pow 5.45 Atan x0 Atan Mul Sub Cbrt Atan Ln Sub Sum Cbrt x2 Cbrt x0 Atan Cbrt x3 Sub Sub Sum Sub Cbrt Sub x0 x2 Sub Sub x2 x1 Sub x2 x2 -4.97 Sub Sub Atan Pow3 x2 Cbrt Cbrt x0 Atan Ln Sub x2 x2 Mul x3 Sub Cbrt Atan Ln x2 Cbrt Sub Mul x0 x0 Sub x2 -2.23 Cbrt Sub Atan Ln Sub Cbrt Sub x2 x1 6.09 Cbrt Sub Sub Pow x1 Atan x0 Atan Ln x0 Sub Atan Pow3 x1 Atan Pow x0 -4.35",

@"-1.14,1.3;-1.44,0.45;-2.04,0.44;-1.92,-1.87;-1.22,0.31
0.67,-2.6,-0.22,0.2;0,0,0,0;0,0,0,0
Cbrt Atan Pow3 Sum Mul Cos Atan Pow2 Sum Atan Pow3 x6 Abs Cbrt x7 Atan Pow3 Atan Ln Cos Sqrt x7 Cbrt Sqrt Sub Cbrt Sum Mul Cos x10 Sum x0 x6 Sin Cbrt x5 Cbrt Atan Pow3 Sum x7 x5",

@"-4.89,0.16;-0.47,9.66;-0.16,3.96;-1.8,3.12;-0.87,3.64
0.26,0.11,0.41,0.07;-3.39,1.49,5.63,0.36;-2.76,6.5,1.63,0.06
Cbrt Atan Pow3 Sum Mul Cos Atan Pow2 Sum Atan Pow3 x6 Abs Cbrt x7 Atan Pow3 Atan Ln Cos Sqrt x7 Cbrt Sqrt Sub Cbrt Sum Mul Cos x10 Sum x0 x6 Sin Cbrt x5 Cbrt Atan Pow3 Sum x7 x5",

@"-10.61,-1.23;-26.32,-12.78;-30.4,-23.39;-8.99,11.67;1.12,22.12;-7.74,0.74;-0.67,3.77;-18.74,-2.76
0,0,0,0;0.69,-0.24,-1.12,0.34;-0.37,-0.61,-0.78,0.4
Sub Sqrt Sqrt Sub Sqrt Sub Sum Cos Atan Pow -1.25 Cos x0 Sub Max Mul Sub x7 x5 Atan Ln x5 Sum Sqrt x6 Sum x3 x6 Sqrt Sum Mul x5 x3 Sqrt x8 Sub Sin Atan Ln Sub x8 x7 Sqrt Sqrt Mul Sum x5 x2 Atan Pow x0 8.1 Sub Atan Ln Sqrt Sub -5.77 Sub Cos x6 Max x7 x4 Mul Cos Sqrt Cos Sub 3.57 Sqrt x8 Cos Sub Sqrt Atan Ln x0 Mul Cos Sum x4 x5 Sub Cos x0 Atan Pow x7 3.7 Atan Pow -5.72 Sub Cos Cos Sqrt Cos Cos Atan Ln x3 0.96",

@"0.56,4.63;0.13,21.85;0.23,5.35;-17.69,6.58;-14.71,-13.89;-16.69,0.59
2.29,-0.24,2.91,0.35;0.02,0.46,0.02,0.48;-0.03,-0.59,2.32,0.27
Sum Sum Atan Ln Sin Cbrt Atan Ln Cbrt Atan Cos Cbrt x1 Cbrt Atan Ln Sum Cos Sum Cos Atan Ln Cbrt x6 Sum Cos Sum Sum x3 x4 Sum x0 x1 Cbrt Cbrt Cos x2 Mul Cos Cos Sum Cos Sum x1 x4 Cos Cos x4 Mul Cbrt Cos Sum Sum x6 x1 Atan Pow2 x2 Cbrt Sum Atan Cbrt x3 Mul Cbrt x0 Cos x2 Cbrt Cbrt Cbrt Sum Cos Cbrt Atan Pow2 Cos Cbrt Cbrt x2 Cbrt Sum Sum Cos Atan Pow2 Cbrt x0 x3 Sqrt Atan Pow2 Cbrt Cos x1",

@"-2.28,-1.91;0.65,17.79;-3.54,2.37;-6.29,6.04;-1.76,13.32;-4.85,-1.17
-0.18,-1.53,-0.99,0.38;0.08,0.36,0.16,0.38;1.92,0.78,0.08,0.03
Sum Sum Atan Ln Sin Cbrt Atan Ln Cbrt Atan Cos Cbrt x1 Cbrt Atan Ln Sum Cos Sum Cos Atan Ln Cbrt x6 Sum Cos Sum Sum x3 x4 Sum x0 x1 Cbrt Cbrt Cos x2 Mul Cos Cos Sum Cos Sum x1 x4 Cos Cos x4 Mul Cbrt Cos Sum Sum x6 x1 Atan Pow2 x2 Cbrt Sum Atan Cbrt x3 Mul Cbrt x0 Cos x2 Cbrt Cbrt Cbrt Sum Cos Cbrt Atan Pow2 Cos Cbrt Cbrt x2 Cbrt Sum Sum Cos Atan Pow2 Cbrt x0 x3 Sqrt Atan Pow2 Cbrt Cos x1",

@"-8.31,9.23;-3.74,-0.38;-25.36,-19.61;-3.6,2.99;-4.32,8.87;-11.32,6.86
0,0,0,0;0.55,-0.01,0.05,0.64;1.6,1.92,-0.43,0.36
Sum Sum Atan Ln Sin Cbrt Atan Ln Cbrt Atan Cos Cbrt x1 Cbrt Atan Ln Sum Cos Sum Cos Atan Ln Cbrt x6 Sum Cos Sum Sum x3 x4 Sum x0 x1 Cbrt Cbrt Cos x2 Mul Cos Cos Sum Cos Sum x1 x4 Cos Cos x4 Mul Cbrt Cos Sum Sum x6 x1 Atan Pow2 x2 Cbrt Sum Atan Cbrt x3 Mul Cbrt x0 Cos x2 Cbrt Cbrt Cbrt Sum Cos Cbrt Atan Pow2 Cos Cbrt Cbrt x2 Cbrt Sum Sum Cos Atan Pow2 Cbrt x0 x3 Sqrt Atan Pow2 Cbrt Cos x1",

@"-17.13,17.81;-10.02,-4.48;-9.6,-6.75;0.96,11.52;1.5,13.69;0.38,18.84;-5.5,7.65;-11.37,4.87;-1.13,-0.53;-0.15,8.32;-20.25,-0.99;-2.42,-0.51;-0.91,0.44;-7.72,18.64
-0.19,1.43,0.47,0.42;0.15,0.75,-0.04,0.21;0,0,0,0
Sum Sum Sum Sum Mul Atan Pow2 Atan Pow2 Atan Pow2 Atan Pow3 x1 Atan Pow2 Mul Sum Sum Sum Atan Pow3 x10 Sin x13 Sum Sum x2 x5 Atan Pow3 x12 Sum Sum Sin x8 Cos x10 Cos Sum x5 x2 Sin Sin Abs Sum x6 x3 Atan Pow2 Atan Pow2 Atan Pow2 Sin Cos x11 Atan Pow3 Mul Sum Atan Pow3 Mul Sin Atan Pow2 x5 Sum Sum x6 x3 Cos x2 Cos Sin Atan Pow2 Cos x1 Cos Sum Cos Sum Sum Sin x1 Sum x0 x12 Sum Cos x12 Sum x6 x9 Sum Mul Sum Sin x4 Cos x4 Mul Abs x4 Sum x7 x13 Atan Pow3 Sum x0 x5 Atan Pow2 Sum Sum Atan Pow2 Sum Cos Sum Mul x7 x3 Sum x10 x11 Sum Sin Sum x12 x7 Sum Sum x3 x9 Sum x9 x10 Atan Pow2 Sum Atan Pow2 Mul x6 x10 Sum Mul Sum x1 x8 Cos x6 5.63 Atan Pow3 Sin Sum Sum Mul Mul x10 x1 -1.98 Sum Sum x9 x5 Cos x10 Atan Pow3 Atan Pow2 x13 Atan Pow3 Sin Sum Sum Sin Mul Sum Mul Sum Cos x11 Sum x8 x3 Sum Sum x9 x0 Sum -8.45 x12 Atan Pow3 Sum x13 x2 Atan Pow2 Atan Pow2 x3 Cos Sum Sum Abs Sum Cos x13 Cos x10 Cos Sum Sum x0 1.79 Atan Pow3 x1 -0.53 Sin Abs Sum Sum Atan Pow3 Atan Pow2 x5 Atan Pow2 Atan Pow2 x8 Atan Pow2 Sum Sum x6 x11 Atan Pow2 x3",

@"-2.24,6.72;-10.51,14.92;6.56,9.88;-0.57,10.54;1.71,15.67;-11.35,24.91;-9.24,-6.16;0.97,4.95;-15.98,2.98;20.34,24.76;-17.27,-0.67
1.3,-0.93,-1.35,0.24;-0.72,0.78,-0.92,0.24;-2.27,-5.23,-3.95,0.87
Sin Sin Cos Max Cos Sin Sum Sin Cos Sum Atan Ln x7 Sum Sin x8 Sin x2 Sum Sin Sin Sin Cos x6 Sin Sin Sin Sum x10 x0 Mul Sin Sum Sin Sin Sum Sin Cos x9 Sum Sum x7 x3 Sin x3 Max Sum Abs Sum Sum x6 x9 Abs x10 Sum Sin Sum x0 x1 Atan Ln x4 Sum Sum Sin Sum x7 x4 Atan Pow3 x5 Sum Sum Cos x2 Sum x9 x2 Sin Sin x0 Sum Sum Sum Sum Sin Sum Sin x0 Sum x8 x3 Sin Sum 3.61 Sum x1 x10 Sum Mul Abs Sin x1 Sin Cos x3 Cos Max Sin x7 Cos x6 Atan Pow3 Sin Sum Atan Ln x8 Sum x1 x10 Sin Abs Cos Sum Sum Atan Ln x10 Sin x5 Mul Atan Ln x3 Abs x0",

@"-6.53,0.26;-1.54,2.55;-3.41,3.03;-14.2,1.29;-3.03,10.97
0,0,0,0;0.91,-0.4,0.52,0.42;-0.05,-0.06,0.2,0.82
Sum Sin Sum Sum x3 Sum Cos Sum Sin Sin Sin Atan Sum x4 x2 Sum Sum Sum Cos Sum x1 x2 Sin Atan x0 Sin Sum Sin x3 Sum x0 x1 Sin Sum Sum Sin x1 Sin x4 Sum Sin x4 Sin x1 Sum Sin Sum Sin Sin Sum Sum x3 x2 Sin x3 Sin Sin Sum Sum x2 x3 Atan x0 Sum Sum Sum Sin Sin Sum x1 x3 Sin Sum Sum x0 x0 Sum x3 x3 Sum Sin Atan Pow3 x4 Atan Sin Sum x1 x4 Sin Cos Sin Sum Sum x2 x0 Sin x3 Sum Sum Sin Sum Sin Sin Sin Sum Atan x2 Sum x3 x0 Sin Sum Sin Cos Sin x1 Sum Mul Sum x4 x4 Sum x0 x1 Sin Sum x0 x4 Atan Sin Sum Sin Sum Sin Sin x2 Atan Sin x4 Sin Sum Sin Sin x1 Sin Sum x2 x0 x1 Sin Sum Sin Sum Sin Sin Sum Sum Sin Sin Sum x4 x2 -3.46 Sum Sin Sin Sum x0 x2 Sin Sin Sum x0 x2 x3 Sin Sum Sum x2 Mul Atan Atan Sum Sum Sum x2 -1.47 Sum x1 x2 Sum Cos x4 Sum x2 x3 Sum Sum 5.02 Sin Sum Sin x1 Sum x2 x0 Sum Atan Sum Mul x2 x1 Sum x4 x0 Sum Sum Sum x4 x3 Sum x3 x3 Cos Sin x3 Sum Sin Atan Sin Sin Cos Sum x4 x3 Sin Sum Sum Sin Cos Sin x0 Sum Sum Atan x4 Sin x0 Sin Sum x1 x4 Sin Sin Atan Sin x1",

@"-7.33,26.2;-27.83,-8.06;-4.63,5.66;-16.8,10.36;3.66,8.04;-11.6,5.51;2.29,10.05;-14.44,12.77;-4.45,-1.86;2.99,11.74
0.77,2.51,-1.42,0.17;5.49,0.04,-6.17,0.32;3.25,-2.73,-0.88,0.91
Sin Sqrt Sqrt Sum Sub Sub Sqrt Sqrt Sub Sum x4 x8 Sqrt x2 Sqrt Sin Sub Sum x5 x2 Sqrt x11 Sqrt Sqrt Sum Sum Sum x7 x5 Sqrt x1 Sub Sqrt x9 Sqrt x3 Sqrt Sub Sum Sqrt Sqrt Sub x7 x9 Sub Sqrt Sum 2.47 x1 Sqrt Sum x3 x1 Sum Sqrt Sin Sqrt x10 Atan Pow Sqrt x2 x7",

@"-2.19,3.05;-5.58,9.46;-8.66,-8.04;-8.55,-7.49;1.07,10.64;-4.37,4.57
0.57,-0.35,-0.21,0.17;1.75,-0.35,1.45,0.05;0,0,0,0
Sum Sqrt Sum Sub Sum Sqrt Sin Sum Sqrt x4 Sum x0 x5 Cbrt Sub 0.31 Cos Sum x5 x1 1.78 1.39 Cos Sum Cos Sub Cbrt Cbrt Sqrt Cos x1 Cbrt Cos Sub Sqrt x0 Sqrt x2 Sqrt Cbrt Sum Sqrt Cos Sqrt x6 Sum Cos Sum x2 x0 Sqrt Sqrt x2",

@"-14.11,-4.82;-1.74,1.86;-0.47,1.26;-12.26,-4.47;-20.73,-6.03;-16.02,-5.9
2.69,0.66,-2.61,0.08;0.04,-0.43,2.51,0.04;2.64,-2.31,0.45,0.07
Sum Sqrt Sum Sub Sum Sqrt Sin Sum Sqrt x4 Sum x0 x5 Cbrt Sub 0.31 Cos Sum x5 x1 1.78 1.39 Cos Sum Cos Sub Cbrt Cbrt Sqrt Cos x1 Cbrt Cos Sub Sqrt x0 Sqrt x2 Sqrt Cbrt Sum Sqrt Cos Sqrt x6 Sum Cos Sum x2 x0 Sqrt Sqrt x2",

@"-9.16,-4.71;-1.33,9.42;-3.93,0.24;-0.98,0.59;-4.4,9.06;-10.25,-2.96
2.21,-0.04,-0.35,0.99;0,0,0,0;0.72,-0.9,-0.22,0.26
Sum Cbrt Sum Sub Mul Atan Ln Sin Sub Sum Sqrt x1 Atan Pow3 x4 Sin Cos x2 Cos Sub Sqrt Sub Cos Sin x0 Cbrt Cbrt x2 Atan Pow3 Mul Sum x2 x1 Sqrt x0 Sum Sqrt Sin Cbrt Sum Sum Atan Pow3 x0 Sqrt x5 Sqrt Sin x1 Sum Cbrt Sqrt Sub Atan Ln x4 Sin Sin x2 Sum Sqrt Cbrt Mul 1.65 Sqrt x5 Cos Sin Sub Sub x1 5.13 Cos x3 Sin Sub Cos Sub Sub Atan Pow3 Max x0 x3 Sin Cbrt Cos x1 Sum Sum Sin Cbrt x5 Sub Cos x1 Cbrt x2 Cos Sum Sin x2 Cbrt x3 Sum Sum Sqrt Cbrt Sin Cos x0 Cbrt Mul -2.78 Sum Cos x3 Sum x5 x4 Sin Sum Sub Atan Pow2 x2 Cbrt Max x4 x2 Mul Sin Sub x3 x2 Sin Cos x5 Sin Sum Sin Sqrt Sqrt Max Cbrt Cos Atan Pow3 x4 Sum 6.99 Mul Sin Cbrt x2 Atan Pow3 x2 4.75",

@"0.25,1.49;-5.51,0.41;-7.16,-1.78;-10.55,4.94
0,0,0,0;2.11,-0.12,-2.65,0.15;0,0,0,0
Mul Sum Sin Sqrt Sum Sin Atan Pow2 Atan Pow2 Sum Sqrt x0 Sqrt x1 Sqrt Sum Sin Sin Atan Pow2 Sum x0 x2 Atan Pow2 Cbrt Atan Pow2 x2 Cbrt Sum Sum Sqrt Sum Mul 9.91 Atan Pow2 Atan Pow2 x3 Sin Cbrt Sin Atan Pow2 x0 Cbrt Sqrt Sin Cbrt Sqrt Sqrt Sum x1 x0 Sum Sin Sin Sin Sum Sum Cbrt Max x3 x0 Sqrt Sum x3 x3 Sum Sum Cbrt x1 Sqrt x3 Sqrt Sqrt x2 Sin Sqrt Sum Sin Cbrt Atan Pow2 x2 Sin Sum Cbrt Sqrt x3 Sin Sin x1 Sum Sin Max Atan Pow2 Sum Sqrt Mul Sum x2 Sum Sqrt x2 Sqrt x1 Atan Pow2 Sin x2 Sin Sum Sin Sum Atan Pow2 x3 Sum x2 x1 Sum Mul Sqrt x3 Sqrt x0 Sin Sum x0 x1 Sum Sum Sqrt Sum Sum Sum Sqrt Mul x2 x3 Mul Sum x2 x0 Cbrt x2 Sum Sum Atan Pow2 x0 Sqrt x2 Sin Mul x0 x2 Sin Sum Sum Atan Pow2 x1 Sin x0 Sum Atan Pow2 x1 Sin x0 Sin Sqrt Cbrt Atan Pow2 Sin x0 Sqrt Cbrt Sin Sum Mul Sum Sum x0 x3 Atan Pow2 x3 Sin Cbrt x2 Sqrt Sum Atan Pow2 x1 Sum x1 x3 Sqrt Sum Atan Pow2 Atan Pow2 Sqrt Sum Sin Sin x3 Atan Pow2 x0 Sqrt Sum Sum Sqrt Sum Sqrt Atan Pow2 x2 Cbrt Sqrt Sum x2 x1 Sin Atan Pow2 Sin Sum x3 x1 Sum Sqrt Sum Sum Sqrt Cbrt x2 Sin Cbrt x0 Sum Cbrt Sum x2 x3 Sqrt Atan Pow2 x0 Sum Mul Sum Sum Sin x2 Sqrt x0 Sqrt Sqrt x3 Sum Cbrt Sin x0 Cbrt Atan Pow2 x0 Sum Atan Pow2 Sum x0 x2 Sqrt Sum Mul x2 x1 Sum x0 x3",

@"0.25,1.49;-5.51,0.41;-7.16,-1.78;-10.55,4.94
2.31,3.52,-1.42,0.01;1.4,-0.3,-0.08,0.46;0,0,0,0
Mul Sum Sin Sqrt Sum Sin Atan Pow2 Atan Pow2 Sum Sqrt x0 Sqrt x1 Sqrt Sum Sin Sin Atan Pow2 Sum x0 x2 Atan Pow2 Cbrt Atan Pow2 x2 Cbrt Sum Sum Sqrt Sum Mul 9.91 Atan Pow2 Atan Pow2 x3 Sin Cbrt Sin Atan Pow2 x0 Cbrt Sqrt Sin Cbrt Sqrt Sqrt Sum x1 x0 Sum Sin Sin Sin Sum Sum Cbrt Max x3 x0 Sqrt Sum x3 x3 Sum Sum Cbrt x1 Sqrt x3 Sqrt Sqrt x2 Sin Sqrt Sum Sin Cbrt Atan Pow2 x2 Sin Sum Cbrt Sqrt x3 Sin Sin x1 Sum Sin Max Atan Pow2 Sum Sqrt Mul Sum x2 Sum Sqrt x2 Sqrt x1 Atan Pow2 Sin x2 Sin Sum Sin Sum Atan Pow2 x3 Sum x2 x1 Sum Mul Sqrt x3 Sqrt x0 Sin Sum x0 x1 Sum Sum Sqrt Sum Sum Sum Sqrt Mul x2 x3 Mul Sum x2 x0 Cbrt x2 Sum Sum Atan Pow2 x0 Sqrt x2 Sin Mul x0 x2 Sin Sum Sum Atan Pow2 x1 Sin x0 Sum Atan Pow2 x1 Sin x0 Sin Sqrt Cbrt Atan Pow2 Sin x0 Sqrt Cbrt Sin Sum Mul Sum Sum x0 x3 Atan Pow2 x3 Sin Cbrt x2 Sqrt Sum Atan Pow2 x1 Sum x1 x3 Sqrt Sum Atan Pow2 Atan Pow2 Sqrt Sum Sin Sin x3 Atan Pow2 x0 Sqrt Sum Sum Sqrt Sum Sqrt Atan Pow2 x2 Cbrt Sqrt Sum x2 x1 Sin Atan Pow2 Sin Sum x3 x1 Sum Sqrt Sum Sum Sqrt Cbrt x2 Sin Cbrt x0 Sum Cbrt Sum x2 x3 Sqrt Atan Pow2 x0 Sum Mul Sum Sum Sin x2 Sqrt x0 Sqrt Sqrt x3 Sum Cbrt Sin x0 Cbrt Atan Pow2 x0 Sum Atan Pow2 Sum x0 x2 Sqrt Sum Mul x2 x1 Sum x0 x3",

@"-2.4,27.26;-12.2,-5.61;-5.62,19.95;-1.18,15.72;-6.5,22.45;-0.94,-0.37;-12.28,-6.81;-16.04,-5.04;-11.94,4.3;-15.49,-4.65;-21.7,13.03;-2.06,-0.04;-18.89,0.32;-3.76,-2.91
-2.28,0.1,-0.53,0.71;-0.79,-0.21,-1.47,0.23;1.31,-5.04,-6.23,0.89
Sum Sin Abs Sin Sum Mul Mul Sum Sin Sin Sqrt Sub x5 x2 Mul Sum Sin Sum x4 x3 Sum x2 Sum x4 x0 Sum Sin Sin x12 Pow x14 Atan x12 Mul Sin Sin Sin Sin x6 Sub Sub Sin Mul -5.18 x0 Sin Sin x8 Sin Sub -5.06 Sum x10 x12 Sin Sin Sin Atan Pow -5.08 Sin x5 Sin Sin Atan Pow 0.4 Sin Sin Sin x1 Sum Mul Mul Sin Atan Pow Sin Sum Sin Sum Sin x1 Mul x0 x2 Sqrt Sub Atan Pow x9 -9.87 Sub x12 x11 -8.62 Mul Sin Sin Sin Sub Sin Sin Sin x3 Sin Sin Sum x6 x4 Sin Sin Sum Sum Sum Sum Sum x7 x12 Sub x6 x10 Pow x10 Atan x3 Sum Mul Sin x8 Sum x9 x10 Atan Pow -4 x2 Sum Sin Sin Sum x5 -9.38 Sin Sin Sin x0 Atan Pow x1 Sub 4.52 Sin Sin Sin Sin Sin Sub x9 x9 Sin Sin Pow -8.56 Atan Sin Pow x14 Atan Mul x2 Sub Sub x7 x1 Sin x3",

@"-14.28,3.46;-17.58,-12.33;2.14,18.72;2.95,4.95;-2.26,15.66;-4.32,6.4;0.35,13.45;-4.48,-2.48;-4.01,8.35
-0.77,1.28,0.09,0.79;2.38,3.2,-1.29,0.35;1.13,-2.3,-1.58,0.08
Sum Cos Sub Sum Cbrt Sub Cbrt Atan Ln Sum x6 x3 Cos Cbrt Sum Sub x1 x8 Cos x6 Sum Sub Cos Cbrt Cbrt Abs x7 Sin Atan Ln Cbrt x6 Cbrt Atan Ln Sin Cbrt x3 Sub Cos Sum Sum Sum Sum Cbrt x4 Atan Ln x4 Atan Ln x5 Cos Sin Sub x2 x9 Sin Cbrt Cbrt Sub x3 x9 Sub Cbrt Sub Cbrt Cbrt Atan Ln x1 Cbrt Cbrt Cbrt x1 Atan Ln Atan Ln Sin x8 Cbrt Sum Sub Cos Sub Cbrt Sub x3 Sin Cos x5 Sum Cos Sub Cos x8 Cbrt x3 Sum Cbrt Cbrt x2 Cbrt Sum x4 x7 Cbrt Cbrt Abs Sum Sub Cos x8 Cbrt x7 Cos Cos x2 Cbrt Sub Sin Cbrt Cbrt Sin Cbrt x5 Cbrt Cbrt Atan Ln Sum x9 x6"                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
,

@"-5.5,0.19;-0.13,5.57;-5.24,-3.24;-0.4,3.57
1.61,-2.88,0.08,0.54;-0.6,-0.76,2.47,0.55;0,0,0,0
Atan Ln Mul Atan Ln Sub Mul Mul Sum Sum Sub Atan Ln x3 Atan Ln x3 Sin Atan Ln x0 Atan Ln Atan Ln x3 Sub Sin Sin Atan Ln x0 Atan Ln Sum x0 x2 Atan Ln Atan Ln Atan Ln x2 Sum Sin Sin Sin Sum Sin x0 Sin x2 Sin Atan Ln Sum Sin x0 Sin x3 Sin Sin Sin Sin Sin Sum Sin Sin Sin x1 Sum Atan Ln x0 Sum Sin x0 Sin x1",

@"-0.92,13.81;0.42,10.47;-1.76,5.61;1.31,29.54;4.3,6.3;-20.11,-5.58;2.17,4.17;-9.36,6.54;-7.11,17.94;-22.24,1.99
-0.39,-0.81,4.19,0.14;0.76,0.1,0.36,0.4;0,0,0,0
Cos Sum Sum Sub Cos Cos Sum Cos Cos Cos Cos Cos x3 Sum Cbrt Sub Sub Sub x1 x8 Cos x3 Sub Sum x6 x6 Cbrt x9 Sum Cos Cos Sum x9 x7 Cos Sub Sum x7 x0 Sub x7 x5 Sum Cos Cos Cos Sub Sum Cos Sum x6 -3.15 Cbrt Sum x6 x2 Cbrt Sum Cos x4 Sin x3 Sum Cos Sum Cos Sum Sum Cos x7 Cbrt x3 Cbrt Sub x0 x5 2.35 Sum Sub 8.75 Sum Sum Cos Sum x0 x5 Sub Cos x9 4.3 Sub Sum Cbrt x7 Cos x7 -5.55 Sub Sub Sin Sum Cos x5 Sum x4 x1 Cos Sum Sub x0 x2 Sub x1 x6 Sum Cos Cbrt Sum x6 x5 Sub Sub Sum x6 x7 Cos x9 Sum -9.5 Sub 6.63 x9 Sub Cos Cbrt Cos Sub Sum Sub Sum Sum x8 x8 Sum x9 x4 Sum Cos x0 Cos x3 Sum Cos Cos x5 Cbrt Sum x2 x3 Cos Cos Cos Sub x4 x1 Cbrt Cos Sub Sub Cos Cos Cbrt Cos x9 Cos Sum x6 Sub Sum x2 x2 Sum x4 x2 Sub Cos Cos Cos Cos x3 Sum Cbrt Cbrt Sum x6 x5 Cos Cbrt Cos x1 Cbrt Sum Cos Cos Cos Sum Cos Cos Sum Sub x9 x1 Sub x1 x6 Sum Sum -7.74 Sin Cbrt x3 Sum Cos Sum x0 x3 Sum Cos x3 Sum x6 x8 Sum -0.99 Sub Cos Sum Sum Cos Cos Cos x5 Sum Sum Cos x2 Sub x9 x2 Sub Cos x2 Sum x9 x8 -2.6 0.18",

@"1.17,2.17;-2.93,-1.93;0.53,1.53;-0.62,0.38;2.32,3.32;-0.52,3.61;-0.81,2.49;-2.77,0.27;1.93,2.93;-1.58,-0.58;-3.53,1.41;-1.71,0.46;-0.68,0.32;-1.62,1.13
-1.53,0.17,1.36,0.53;-0.05,0.22,2.19,0.66;-5.81,-4.94,-1.61,0.23
Sum Sub Sub Sqrt Sin Sum Sqrt Sum x13 Sqrt Sin Sqrt Sin x2 Sqrt Sub Sum Sub Mul Sqrt x13 x1 Sub Sqrt x9 Sub x5 x6 Sub Sqrt Sqrt x9 Sqrt Sum x0 x14 Sqrt Sum x5 Sub Sub x2 x5 Mul x3 x2 Sin Sqrt Sub -7.48 Sqrt Sum Sub Sub Sub Sqrt x2 Sub x8 x12 Sub Mul x8 x7 Sqrt x14 Sin Sqrt Sqrt x4 Mul Sin Sum Mul x3 x7 8.09 Sin Sqrt Sum x3 x5 Sum Sub 9.76 Sub Sqrt Sqrt Sqrt Sin Sqrt Sum Sub x4 x13 Sin x1 x13 Sqrt Sin Sum Sum Sqrt Sqrt Sqrt Sum Sub x7 x9 Sin x10 Sub Sum Sin Sqrt Sin x14 x6 Sin Sqrt Sub Sin x12 Sum x9 x9 Sub Sin Sum Sum Sqrt Sum x2 x14 Sqrt Sum 0.36 x14 x13 Sqrt Sqrt Sub Sqrt Sum x3 -3.86 Sub Sqrt x14 Sum x8 x10 x2",
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
@"-15.5,-13.16;-6.58,-0.24;-10.07,-0.92;2.65,10.47;5.22,7.92;1.57,11.77;-4.82,5.58;-4.57,0.79;-12.61,-7.16
0.69,2.21,0.51,0.88;0.35,0.07,0.05,0.19;-1.06,0.16,2.74,0.23
Sum Sqrt Abs Sin Sum Sum Sqrt Sqrt Pow x8 Atan x4 Sqrt Sqrt Sqrt x7 Sqrt Sum Sin Sin x7 Mul x8 Sum x9 x2 Atan Pow Mul Sqrt Sum Atan Pow x9 Abs x8 Sum Sum Sqrt x5 Sqrt x5 Sum Mul x6 x4 Sqrt x3 Abs Sum x9 Pow -5.41 Atan Abs x0 -1.03",
                                                                                                                                                                                                                                       
@"-10.91,-4.42;-16.07,5.82;-0.1,0.9;-20.31,7.03;0.68,3.27;-17.23,8.6;-0.23,20.4;-8.43,-4.76
0.28,0.29,-0.64,0.93;0.68,4.98,2.64,0.12;9.27,-0.29,-1.17,0.04
Sqrt Sub Mul Sub Cos Cos Cos Sin Atan Pow Sin x0 x7 Sqrt Mul Mul Sub Atan Pow 3.25 Sin x0 Cos Sin Mul x0 x6 Sin Cos Sqrt Sin x2 Sqrt Sqrt Sin Sqrt Sqrt x6 Sub Sub Sin Cos Cos Sqrt Sub Sqrt x5 Sqrt x3 Sin Sub Sub Cos Sqrt Sin x4 Cos Cos Sin x0 Cos Sub Sub Sin x4 Cos x4 Sub Max x5 x2 Sqrt x6 Sub Sin Sqrt Sub Sin Sin Cos x1 Sub Sum Sqrt x2 Sqrt x4 Sin Sub x7 x3 Sub Sqrt Sub Cos Sin Cos x6 Sin Sub Sqrt x2 Cos x6 Cos Sin Sub Sqrt Cos x7 Cos Sin x2 Sqrt Sub Sin Cos Sqrt Cos Cos Sin Mul x5 x1 Cos Sin Sin Cos Sqrt Sub Sqrt x4 Sqrt x1",
                                                                                                                                                                                                                                                                                                                                                                                                      
@"7.8,19.26;1.19,9.86;-2.22,4.17;-4.86,6.26;-2.59,0.79;4.7,8.73
-3.37,-0.74,3.59,0.03;0.99,-0.8,-0.98,0.09;1.37,-0.75,-1.12,0.52
Cos Sub Sin Sub Sin Abs Sin Sub Sub 8.13 Atan Pow x7 x9 Pow 4.83 Atan Atan Pow x10 2.81 2.05 Sum Sin Atan Pow 7.96 Atan Pow -6.73 Sin Sin Sqrt x8 Sqrt Cos Sum Sum Sin Atan Pow Sin x5 x8 Sqrt Cos Sum 8.21 Sum x7 x1 Cos Sum -6.57 Atan Pow Sub x7 x7 8.78",

@"-1.52,-0.52;-3.98,1.6;0.29,2.15;0.18,1.18;-4.73,-1.53
0.23,5.29,-2.22,0.42;-1.2,-0.02,-0.41,0;-0.41,1.61,1.16,0.1
Sum Sum Sin Sin Sum Sum Sum Sum Sin Sin Sin x1 Sin Max Sum x4 x3 Sum x1 x3 Sum Sin Sin Sum x1 x3 Sin Sum -2.56 Sin x0 Sum Sum Sum Sin Sum x1 x4 Sum Sin x4 Max x0 x4 Sin Sin Sum x2 x3 -7.98 Sum Sin Max x0 Sum Sum Sin x0 Sum x2 4.56 Sin Sum x1 x3 Sum Sum Sin Sin Sin x2 Sin Sum Sin x0 Sin x2 Sum Max Sin Sin x2 x0 Sum Sin Sin x1 Sum Sin x1 Sin x2 Sin Sin Sum Sin Sum Sum Sum Sin Sum x4 x2 Sum Max x3 x3 Sum x1 x0 Sin Sum Sum x3 x0 Sum x3 x1 Sin Sum Sum Max x4 x1 Sum x0 x2 Sin Sin x0 -2.21 Sum Sin Sin Sum Sin Sin Sin Max Sin Sum x2 x1 Sin Sin x3 Sum Sum Sin Sum Sum Sum x2 x2 Sin x4 Sum Sin x4 Sum x4 x4 1.11 -9.36 Sin Sin Sin Sin Sum 5.25 Sin Sum Sum 2.75 Sin x1 Sum Sum x0 x2 Sum x0 x3",

@"-0.78,3.69;2.41,3.67;-2.52,-0.99;-3.92,-1.4;-0.85,0.85;-3.44,3.49;-3.78,-0.54;-2.09,2.82;2.9,6.4;0.34,6.4;-3.3,0.42;-0.46,0.54
-0.13,1.35,-2.48,0.13;3.15,-6.19,-1.45,0.43;-0.33,0.67,-1.12,0.83
Sqrt Sqrt Sum Sqrt Sum Mul Sqrt Cbrt Sqrt Sqrt Sqrt Sum x10 x4 Sin Sin Mul Sqrt Sin Sin x11 Sin Cbrt Cbrt x7 Sqrt Sin Abs Sqrt Mul Sin Sqrt x8 Mul Sin x0 Cbrt x9 Sum Cbrt Sqrt Sin Mul Sqrt Cbrt Sqrt Abs x12 Mul Sqrt Cbrt Mul x10 x1 Mul Mul Sin x1 Cbrt x6 Sin Sum x7 x1 Cbrt Mul Mul Mul Sin Sum Sin Mul x10 x0 Mul Sqrt x5 Sum x1 x12 Mul Mul Sqrt Sqrt x9 Cbrt Sin x9 Sqrt Sqrt Sqrt x3 Sqrt Mul Sum Sin Sum x12 x8 Sin Cbrt x4 Sqrt Mul Sin x7 Sum x7 x7 Sqrt Mul Sqrt Sin Sqrt Sqrt x4 Mul Sin Mul Mul x12 x9 Cbrt x7 Sqrt Cbrt Sin x12",
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
@"-2.93,-0.1;0.87,1.87;-0.39,0.61;-3.58,0.11;-1.03,-0.03
0,0,0,0;-0.45,0.22,0.37,0.55;0.41,1.72,-0.03,0.42
Mul Sum Sub Atan Pow x4 Cbrt Sub Cbrt Cbrt Cbrt Sub Atan Pow x3 -7.49 Sub x0 x0 Sum x0 Cbrt Sin Cbrt Cbrt x1 Cbrt Sum Cbrt Pow x3 Atan Sub Sum Cbrt Cbrt x0 Cbrt Sin x4 Sin Cbrt Atan Pow x3 -5.53 Sin Sin Cbrt Sub Cbrt Cbrt Sin x1 Pow x3 Atan Sin x2 Sin Sub Cbrt Sub Sub Mul Cbrt Cbrt Sub Cbrt x0 Cbrt x3 Cbrt Sin Cbrt Atan Pow x2 x4 Sin Sin Sin Cbrt Sin x1 Sub Sin Sin Sin Cbrt Sin x1 Atan Pow Sub Sum Cbrt x3 Sin x2 Sin Sin x1 x1 Cbrt Sin Sub Sub Sin Sin Sum Sin x1 Mul x4 x2 Sin Cbrt Sin Sin x3 Cbrt Sin Sum Sub Sub x2 x0 Sin x3 Sin Cbrt x4 Sum Sin Sin Cbrt Sin Cbrt Atan Pow Cbrt Sum Sin x4 Cbrt x4 x3 Sin Cbrt Sum Sub Sub Cbrt Sub Sin Cbrt Sin x1 Cbrt Sin Cbrt x1 Sin Sin Sin Sub Sin x3 Sin x2 Atan Pow x2 Sum Sin Max Sin x0 Cbrt x3 Sin Atan Pow x1 x2 Sub Cbrt Sin Sin Cbrt Sub Sin x2 Cbrt x3 Sub Sub Sub Sin Sub Sub x1 x4 Sub x0 x3 Sub Sin Sin x3 Cbrt Sub x0 x3 Pow x2 Atan Sin Sub x3 x4 Cbrt Sin Sub Sub Sin x3 Sub x1 x1 Sin Sin x3",
                                                                                                                                                    
@"-9.96,1.15;-25.13,-23.19;4.67,5.76;-2.17,26.53;-39.24,-17.48;-0.1,31.48
-2.28,-1.07,2.62,0.98;-3.35,-3.95,-2.86,0.58;0.7,-4.09,-2.78,0.33
Sqrt Cbrt Sin Sqrt Sum Sum Cbrt Cbrt Sin Mul Sum Sin x5 Sum x4 x2 Cbrt Sin x0 Sin Sqrt Sqrt Sum 2.06 Sqrt Sqrt x2 Sum Sin Sum Sin Mul -0.36 Cbrt Sum x2 x5 Sin Sin Sqrt Cbrt x2 Sum Sin Sum Mul Atan Sin x4 Sum Sum -1.48 x4 Sqrt x5 Sqrt Cbrt Mul x4 5.13 Sum Max Cbrt Sqrt Sum x3 x5 Atan Atan Pow x4 -5.91 Sum Sum Sin Sum x4 x3 Sum -7.36 Sin x4 Cos Sqrt Mul x5 x1",

@"-10.69,11.88;-8.54,-0.58;-3.06,-1.38;1.32,2.38;-13.44,-8.8;13.97,29.43;3.08,4.96;10.92,14.15
0,0,0,0;5.02,0.34,6.9,0.04;2.91,0.25,0.27,0.41
Cbrt Sum Cbrt Cos Sum Sum Cbrt Cbrt Cbrt Cbrt x5 Sum Sum x0 Cbrt Sum x7 x4 Cos Cos Mul x2 x6 x7 Atan Cos Sum Sum Cbrt Cos Sum Sin x1 Sum x7 x6 Cos Mul x7 Sum Cbrt x1 x5 Sum Sum x3 Cbrt Cbrt Cos x6 x5",

@"-3.52,15.51;0.61,7.53;-8.3,3.01;1.3,7.41;-5,-2.04;-17.47,7.18;-10.05,-0.91;2.24,7.55;-0.38,1.04;-8.69,8.83;-16.48,7.49
1.46,2.92,-5.56,0.03;-0.04,0.02,0.83,0.47;-0.01,1.86,-0.09,0.19
Sub Sum Sin Sub Sin Cbrt Cbrt Sum Sqrt Sin x10 Sqrt Sin x3 Sub Sum Sin Atan Cbrt Sin x3 Sum Sin Cbrt Sin x2 Sum Cbrt Max x5 x1 Sqrt Sum x6 x1 Sqrt Sum Max Sin Cbrt x9 Sub 6.3 Cbrt x3 Sin Sub Cbrt x4 x3 Sum Sin Sqrt Sin Sum Sum Sqrt Cbrt x8 Sqrt Sub x10 x8 Sub Sum Cbrt x0 Sub x5 x8 Sum Sum x4 x8 Sum x9 x5 Sum Sub Sum Sum Sqrt Cbrt Sin x2 Sin Sin Sin x1 Sin Sin Cbrt Sub x4 x4 Sin Sum Cbrt Sqrt Sum x9 x3 7.97 Sum Sin Sin Sum Cbrt Sum x0 x8 Sqrt Sqrt x7 Sin Sum Cbrt Atan Pow 6.38 x2 Sin Sin Cbrt x10 Sin Sum Cbrt Sqrt Sin Cbrt Cbrt Sqrt Cbrt x5 Cbrt Sub Sum Sub Sum Sin Sum x3 x6 Sqrt Cbrt x7 Sin Sum Sin x2 Cbrt x0 Sub Sin Cbrt Sub x2 x10 Cbrt Cbrt Sin x8 Cbrt Max Sin Sum Sqrt x7 Sin x2 x0",

@"-4.17,0.98;-3.22,-1.97;-8.64,6.91;1.88,3.74;-4.76,-1.05;-10.1,6.24;-13.69,-8.15;-7.75,0.66;0.03,8.16;-28.48,-8.25
-0.11,0.67,-0.9,0.29;6.5,-1.56,-1.13,0.12;2,0.72,-1.16,0.1
Sin Cbrt Sqrt Sum Sub Cos Sum Cbrt Sin Sin x1 Sub Cos Sin x4 Sum Cos x5 Sin x8 Sub Cbrt Sub Sub Sub x0 x0 Sub x7 x6 Sin Cbrt x8 Sum Cos Sin Abs x3 Sub Sin Sum x1 x5 Sin Sub x2 x10 Sqrt Cbrt Sin Cos Sub Cos x4 Sub x6 -5.91",

@"-4.37,-2.49;-2.34,4.2;0.17,1.17;1.76,16.69;-9.46,-2.25;-7.37,-4.41;-19.99,-7.44;-10.25,1.5;-12.25,25.59;-6.32,0.21;-21.83,5.67;-15.05,1.09
-0.73,0.91,-2.97,0.29;-0.39,1.74,0.96,0.26;2.06,0.5,-1.77,0.25
Atan Ln Sum Mul Mul Sin Sin Atan Ln Atan Ln Sum x12 x7 Mul Sum Mul Sum Sum Mul Sin x0 Sin x13 Sin Mul x9 x12 Atan Ln Sin x0 Sum Atan Ln Atan Ln x0 Atan Ln Sin x1 Sum Mul Atan Ln Atan Ln x2 Sum Sin Mul x6 x10 Sin Sum x4 x2 Mul Sum Abs Sin x10 Atan Ln x7 Sin Sin Atan Ln x1 Atan Ln Sum Mul Sum Sin x4 Mul x5 x6 Atan Ln x2 Atan Ln Sum x2 x0 Atan Ln Mul Sin Sum Sum Atan Ln x4 Atan Ln x0 Atan Ln Sin x12 Mul Sum Atan Ln Sin x6 Sum Mul Atan Ln x1 Sin x10 Sum Atan Ln x8 Sum x12 x9 Atan Ln Atan Ln x1 Atan Ln Atan Ln Atan Ln Sin Sum Atan Ln x2 Atan Ln x2",

@"-1.04,0.01;0.64,1.64;-0.07,0.93;-0.26,0.74;-0.47,0.53;-0.15,0.85
4.55,-0.18,-3.57,0.05;2.86,0.75,4.41,0.17;-0.8,-0.07,-1.86,0.1
Sum Sum Sin Cos Atan Ln Sin Sin Sin Sum Sum x3 x4 Cbrt x0 Sin Sum Sum Abs Cbrt Cos Cos Cbrt Atan Ln x6 Cbrt Cbrt Atan Ln Sin Sin x6 Sum Sin Sum Sum Sin Atan Ln x4 Cbrt Sum Sum x5 x5 Cos x3 Cbrt Sum x3 Cbrt Sin x3 Sum Cbrt Sum Cbrt Sin Sum x5 x6 Cbrt Sin Sin x5 Cbrt Cbrt Sin Sum Sin x2 Cos x4 Cos Sin Sum Sum Sum Sin Sum Atan Ln Sin x2 Sum Sin Cos x5 Atan Ln x4 x4 Sin Sum Sum Cos Atan Ln x0 Sin Sin Sin x3 Sin Sin Sin Sin x3 x3",

@"-2.52,-0.02;-2.49,0.35;-5.89,0.07;-0.89,0.16;-0.43,0.57;-0.74,0.26;0.68,1.68;-1.61,-0.25;0.57,2.2
0,0,0,0;1.3,-1.27,-1.66,0.04;-2.62,0.95,0.9,0.29
Sin Sum Sqrt Sum Sum Sum Cbrt Atan Pow x6 x1 Cbrt Atan Ln x2 Sum Sqrt Sum Sqrt x6 Cbrt x5 Cos Sum Sqrt x0 Cos x4 Sum Atan Ln Sum Sum x3 x8 Sqrt x3 Sum Abs Atan Pow -9.93 x2 Sum x4 Sum Sum x2 x1 Sum x0 x4 Sum Cbrt Cos Sum Sum x7 Sum Sum x8 x3 Cbrt x7 Sum Sum Cbrt x3 Sum x2 x7 -0.03 Sum Sum Cbrt Sum Sum Sum x0 x3 x6 Cbrt Sum x7 x5 Cbrt Sum Sum Cbrt x2 Atan Pow 5.42 x3 Sum Sum x1 x6 x8 Sum Sum -4.32 Cbrt Sum Sum x2 x6 Sum x1 x1 Sum Cbrt Cbrt Sum x8 x5 Cos Cbrt Sum x1 x0",

@"-9.11,14.46;-7.59,7.76;-0.42,3.81;-19.2,-4.86;-9.98,2.08;-13.4,-2.62;2.57,11.01;-13.08,-0.69;1.19,17.47;-3.22,2.02
0.59,-0.42,0.2,0.24;0.12,0.61,-1.97,0.08;-1.16,-2.76,-0.01,0.29
Mul Sqrt Atan Cos Sub Cos Cos Sub Sum x5 x1 4.13 Atan Ln Abs Atan x2 Max Sub Cos Sin Cos Sin Cos Atan x10 Sum Cos Cos Cos Sub Sum x6 x13 Sqrt x6 Cos Sum Mul Sum Atan Ln x4 Cos x3 Sum Atan x10 Sum 6.36 x1 Cos Atan Ln x5 Cos Cos Sum Atan Sub Sub Sum x8 x2 Cos x0 -9.46 Sqrt Sum Sum Atan Ln x2 4.54 Cos Cos x8",

@"-6.67,12.78;-2.51,-1.51;-11.37,-1.62;-20.66,-11.1;-13.38,0.63;-0.16,12.13
-0.11,-0.55,0.03,0.11;0.34,-0.07,0.18,0.09;1.62,1.61,1.48,0.21
Sin Sin Sum Sum Sum x0 Sum Sum Atan Sin Sin Sum x1 x2 Sum Atan Sin Sin x2 Sin Sum Atan x5 Sum x0 x1 Sin Sum Sum -1.68 Sin Sum x5 -2.84 Atan Max Sin x4 Sin x4 Sin Sum -6.46 Sin Sin Sin Sum Sum x4 x2 Sin x3 Sin Sin Sin Atan Sum Sum Sin Atan x4 Sum Sum x2 x2 Sin x3 Atan Sum Sin x5 Sin x4",

@"-13.38,-2.29;-14.74,3.71;-8.75,9.58;0.04,8.23;4.34,5.34;-11.86,0.06;-5.8,17.69
0,0,0,0;-0.49,0.22,-0.08,0.28;1.36,-2.04,-2.46,0.2
Sub Sub Atan Pow x5 Atan Sub Sum Atan Pow x1 x3 Sin Sum x6 x3 Sin Sum Atan x5 Atan Pow x4 x5 Sin Atan Sub Atan Pow x1 Sin Atan x1 Sin Sub Sin Atan x2 Sin Atan x2 Sin Sum Sin Sin Sin Sin Atan Sin x2 Sin Sin Atan Atan Pow Sub x6 x3 x0",

@"-1.57,1;-1.01,0.76;0.78,1.78;-1.39,0.93
2.23,0.68,0.67,0.08;-0.13,-0.15,1.97,0.13;1.62,-1.87,1.84,0.05
Atan Ln Cos Sub Sub Sin Sub Cbrt Cos Sub Sqrt x0 Atan Ln x0 Sub Atan Ln Atan Ln x3 Cbrt Cbrt Cbrt x2 Sqrt Sub Sub Sqrt Sqrt Sub x2 x2 Sqrt Atan Ln x0 Atan Ln Sub Cos x3 Sub x1 x0 Sqrt Sub Cbrt Sub Cos Mul Cos x3 Sqrt x2 Atan Ln Sqrt x3 Cbrt Sqrt Sub Sqrt Atan Ln x3 Sqrt Cbrt x1",

@"-16.39,13.22;2.01,3.56;-9.77,-6.23;-2.32,1.33;4.28,21.66;10.83,12.89
-0.21,1.96,0.12,0.07;0.98,1.55,0.5,0.19;1.39,-0.91,2.61,0.2
Sin Sum Sin Sum Max Mul Sin Sum Sin x3 Sin x5 Sum Sin Sin x3 Sub Sub -7.13 x4 Max x2 x2 Sub Sin Max Sin x5 Atan Pow -9.55 x0 Sin Sin Sub x1 x1 Sin Sin Sin Max Sub x1 x4 Sin x5 Sin Sin Sub Sin Sin Sin Sin x5 Sum Sub Sub Sin x2 Sin x2 Sin Sin x1 Atan Pow 6.68 Mul x3 x4",

@"-0.17,2.3;-12.08,-11.08;-2.72,-1.72;-18.1,9.05;-10.91,15.55
-2.88,-0.54,0.52,0.13;-4.21,1.26,3.72,0.1;0,0,0,0
Sin Sub x12 Sin Atan Pow Sin Sum Sin Sin x1 Sum Sum x4 x0 Sum x4 x2 3.17",

@"-11.75,6.41;-18.57,1.67;-9.24,0.94;2.89,7.29;-1.2,-0.2;-19.42,-2.44;-7.49,4.97;-0.88,2.89;2.31,4.13;-9.04,8.58;14.92,16.1
-3.02,-4.28,-1.01,0.29;-0.16,0.12,-0.01,0.02;-2.12,1.75,0,0.27
Sum Sum Sum Atan Pow Sum Cos Sqrt Sin Cos Sum x6 x3 Atan Pow 8.12 Sum Pow 2.07 Atan x6 Cos Mul x5 x11 6.09 Cos Sqrt Sin Sum Cos Sum Sum Sin x3 Sum x11 x5 Atan Pow -9.33 x11 4.39 Cos Sum Sum Sum Atan Pow 1.01 Sin Sum Sum 3.97 x7 Atan Pow x7 x7 Atan Pow Sin Sin Pow -1.74 Atan x6 -4.99 Cos Sqrt Sum Sin Max Cbrt x3 Sum x0 x4 Sqrt Sqrt Cos x5 x3 Sum Cos Sum Cos Pow -1 Atan Pow -6.26 Atan Sum Sum x9 x9 Sqrt x8 Sum -9.64 Cbrt Sum Cos Pow -0.13 Atan Cos x2 Cos Sum Pow 3.03 Atan x1 Sum Atan Pow x3 -6.22 Sum x6 x1 Atan Pow Atan Pow 6.15 Cos Atan Pow Cbrt Atan Pow x0 x9 -6.14 -2.9",

@"-17.01,20.17;-10.03,-1.53;3.04,34.23;-17.03,1.79;-3.9,-1.52
1.57,0.88,0.05,0.1;-0.07,0.31,0.13,0.28;0.1,-0.07,-0.89,0.26
Sub Sin Sub Sum Sub Sin Sum Sub x1 Sin Sin x1 x4 Sin Mul Sin Sum Sum x3 x3 Sub x3 x1 Sub Sum Sub x0 x3 Sum x4 x3 Mul Sub -8.85 x2 Sin x2 Sin Sin Sin Sin Sub Sub x0 x0 Sum x2 x3 Sum Sub Sum Sin Sub Sub Sin x1 Mul x4 x0 Sub Mul x1 x1 Atan Pow x1 x0 Sin Sin Sin Sub x3 x3 Sub Sin Sin Sin Sin x2 Mul Sub Sin Mul x3 x3 Sin Sin x2 Sin Sin Sin x0 x0 Sin Sub Sin Sin Sum Sin Sin Sin Sin x4 Sub Sin Sub Sin x4 Sub x1 x0 Sin Sub Sub x1 x0 Sum x3 x0 Sum Sin Sin Sin Sin Sin Sub x2 x2 Sum Sin Sin Sub Sin Sin x4 Sub Sum x0 x2 Atan Pow x3 x1 x3",

@"-8.76,12.05;0.29,11.24;-7.22,3.32;-15.7,0.05;-6.06,8.42;2.59,3.59
0.01,0.99,1.5,0.19;0,0,0,0;0.37,-1.19,-0.59,0.09
Max Sqrt Sum Sqrt Sum Sin Max Sqrt Mul Sum x5 x3 Sum x1 x0 Sin Sqrt Sin x2 Sum Sum Abs Sum Abs x1 Cbrt x4 Sum Sum Sum x4 x5 Sin x4 x4 Sum Sum x5 Sum Sqrt x0 Sum x2 x4 Max Sum Sqrt x2 Sin x3 Sqrt Sum x1 x4 Sin Sum Sqrt Sum Sum Sum -5.63 Sum x0 x0 Cbrt Max x2 x3 Mul Max Sqrt x2 Sum x2 x5 Abs Mul x3 x4 Sum Cbrt Sin Sum Sum x2 x5 Atan Ln x1 Sum Sin Sum Sum x2 x0 Mul x0 x0 Atan Ln Sum x2 x2 Sqrt Sqrt Sum Sum Sum Sum Sin Sum Sum x1 x2 Sin x5 Sqrt Sum x2 Mul x1 x4 Sum Sin Sin Sum x1 x0 Sin Sum Cbrt x0 Sum x3 x2 Sum Mul Sin Sin Sum x1 x1 Sin Sum Max x2 x0 Mul x0 x1 Sqrt Sum Sum Sum x4 x2 Sqrt x5 Sqrt Cbrt x2 x2",

@"-2.93,-0.14;-0.87,0.17;-3.36,1.25;0.73,2;-2.3,-0.86;-2.62,-0.33;-1.77,1.61;-0.04,2.09;-0.11,0.89;-1.94,-0.94;-0.31,2.78;0.78,1.78;0.07,4.44;0.68,3.66
-1.3,1.58,-5.68,0.08;0,-0.41,-1.58,0.08;0.06,0.12,-0.09,0.16
Sub Sub Sin Cbrt Sin Cbrt Sin Sub Sin Sub Cbrt x3 Sin x5 Sub Sin Sum x4 x2 Sum Sin x11 Sin x11 Sum Sin Sin Atan Pow Max Sub -3.1 Sin Sin x6 Sub Atan Pow x0 -2.57 Cbrt Sum x10 x5 -0.71 Cbrt Sum Cbrt Sum Sum Atan Pow Sum x10 x9 -5.48 Sin Cbrt Sin x2 Sin Sin Sin Sin x2 Sum Sin Cbrt Sum Sub Sub x12 x13 Sub x0 x2 Sin Mul x4 x13 Sum Sub Sin Sum Sum x8 -1.48 Sin x2 Sum Sin Sub x8 x1 Sub Sin x1 Sum x0 x0 Sin Sum Sin Sum x1 x8 Sub Sin x3 Mul x11 -9.86 Sin Sum Sin Sin Sub Sin Sub Max Cbrt Sub x6 x13 Sin Pow 0.57 Atan x6 Sum Sin Sub x8 -4.03 Sub Atan Pow x2 7.05 1.9 Sub Sin Sin Sum -3.54 Cbrt x0 Sin Cbrt Max Cbrt x9 Sub x9 x13 Sub Sub Sub Max Atan Pow 1.18 Sum Sum 2.62 x2 Sum x3 x4 Sub Sin Sum Cbrt x8 Sub -3.98 x10 Sin Sin Sin x2 9.86 Cbrt Mul Sin Sub Sub Cbrt x6 Sub x4 x6 Atan Pow x0 -1.65 Sub Sin Sin Cbrt x7 Max Sin Sin x6 Sin Sum x1 x7 Sin Sub Sub Sin Sin Sub Sin x12 Cbrt x8 Sub Sin Sub Sub x10 x6 Sub x11 x10 Sum Sin Sin x2 Sin Sum x13 x0 Sub Cbrt Cbrt Sum Sub x2 x10 Sub x3 x1 Sub Sin Sin Sub x13 x2 Sub Max Sub x0 x2 Sub x13 x1 Sin Sum x1 x8",

@"-6.44,5.65;-2.07,-0.51;-1.27,25.58;-21.4,-7.89;2.06,18.24;-0.48,15.16;-0.8,33.64;-2.17,30.27
0,0,0,0;0.78,-1.87,-4.18,0.2;0.71,4.64,-1.58,0.05
Sqrt Sum Sum Sum Sum Atan Ln Sum Atan Ln Sqrt Atan Ln x6 Sum Sum Sum Sum x2 x6 x2 Sum Sum x3 x3 Sum x3 x7 Sum Sub Sum x0 x1 Sqrt x3 Sum Sum x4 x6 Sqrt x3 Sub Sum Sum Sum Sub Sqrt Atan Ln x6 Sqrt Sum x1 x0 Sum Sub Sqrt x5 Sqrt x0 Sum Atan Ln x4 Sum x5 x6 x3 -7.3 Sum Sqrt Atan Ln Sum Sum x0 x2 Sqrt x6 Sum Sum Atan Ln Sum x4 x0 Atan Ln Sub x6 x7 Sum Sum x4 Sqrt Sqrt x0 Sum Sum Atan Ln x7 Sum x0 x2 Sqrt Sin x4 Sin Sum Sqrt Sum Sum x3 Sqrt Atan Ln x5 Sum x0 Sum Sqrt Sub x6 x3 Sub Sum x3 x0 Atan Ln x5 Sum Atan Ln Sum Sum Sqrt x4 -5.36 Sum Sum x0 x0 Sqrt x5 Sum Sum Atan Ln Sum x4 x2 Sum Sum Sum x0 x2 Sum x4 x0 Sin Sum x2 x2 Sub Sum Sum Sum 7.7 x5 Atan Ln x6 Sum Sub x6 x6 Sum x2 x2 Atan Ln Atan Ln x0 Atan Ln Sum Sum Atan Ln Sqrt Sum Sin x1 Sum x2 x4 Sum Atan Ln Sub Sub x1 x2 Sum x5 x0 Sum Atan Ln Sum x7 x1 Sum Atan Ln x0 Atan Ln x6 Atan Ln Sum Sum Sum x3 Sum x5 x4 Sum Atan Ln x3 Sum x4 x7 Sum Sub Sum x3 x1 Sum x1 x5 Atan Ln x1 Sqrt Sqrt Sum Sqrt Sum Sum Sub Sum Sqrt Atan Ln x3 Atan Ln x7 Sum Sum Sum x5 x6 Sub x4 x6 Sqrt Sum x5 x5 Sum x1 Sum Sum Sum x2 x4 Sub x5 x4 Sum Sqrt x7 Sub x1 x6 Sum Sum Sqrt Sum Sum x0 x4 x0 Sqrt Sum Atan Ln x4 Sum x6 x7 Sqrt Sub Atan Ln x2 Sum Sqrt x2 Sum x3 x1 x7",

@"-5.69,6.33;3.12,4.12;-1.09,1.83;-0.17,1.5;-0.93,1.42;-3.14,0.27
-0.21,1.33,0.6,0.01;-0.59,-1.48,3.76,0.21;-1.63,-1.46,-0.69,0.09
Sum Sin Sum Sum Sum Sqrt Sqrt Sum Atan Ln Sum Atan x0 Atan x4 Sqrt Sqrt Sum Sum x3 x4 Sum x4 x2 Sqrt Atan Ln Sqrt Sum Atan Atan Ln x5 Sqrt Sum x3 x5 Sum Atan Ln Sum Sum Sqrt Sqrt Sqrt x3 Sum Sin Atan Ln x4 Sqrt Sum x5 5.48 Sqrt Atan Ln Sum x3 x3 -8.33 0.63 Sum Atan Sum Sum x4 Atan Ln Atan Ln Sum Sqrt Atan Ln x0 Atan Ln x4 Sqrt Sum Atan Ln Sqrt Atan Ln Atan x0 Sqrt Sum Sin Sum Sum Sum x2 x0 Sum x3 x2 Sum Atan x5 Sqrt x4 Sum Sum Sum Sum x2 x1 Sum x4 x1 Sum Sqrt x2 Sqrt x0 Atan Sum Atan Ln x0 Atan Ln x2 Sum Sum Sqrt Sum Atan Ln Sqrt Atan Ln Sum x2 x5 Sum Sum Sqrt Sum Sin Sqrt x0 Atan Ln x3 x1 Sum Atan Ln Atan Ln x2 Sum Atan Ln Sum 0.6 x4 Atan Ln Sin x5 Sin Atan Ln Atan Ln Atan Sin Atan Ln x4 Sum Sum Sum Atan Ln Sum Sqrt Sum Sqrt x2 Sum x5 x1 Sin Sum Sum x3 x1 Sqrt x4 Atan Ln Sqrt Sum Atan Ln x0 Atan Ln x4 Sqrt Sqrt Sqrt Sum Sum Sum Atan Ln x0 4.7 Atan Ln x2 Sum Sin Sum x1 x2 Atan Atan Ln x1 1.2",

@"-14.02,17.94;-12.36,1.03;1.57,6.66;-1.79,17.09;0.06,6.27
0.02,-3.09,-0.11,0.26;-1.2,-0.21,0.1,0.03;-1.7,-3.45,2.25,0.22
Sub Sin Sin Sum Sqrt Sin Sub Sub Sum x2 x0 Sub x1 x0 Sin Sum x3 x1 Sqrt Sub Sqrt Atan Pow x3 -3.38 Sqrt Sub Cbrt x4 Sqrt x0 Sin Cbrt Sum Sum Sub Sub Sum Sum x2 x1 Cbrt x0 Sub Sum x4 x1 Cbrt x3 Sum Sum Cbrt x2 Cbrt x2 Sqrt Sub x0 x1 Sin Cbrt Sub 0.41 Sqrt x4 Sub Sin Cbrt Sqrt Cbrt x0 Cbrt Sub Sub Cbrt x2 -7.25 Sqrt Pow 9.07 Atan x0",

@"-7.89,17.33;-9.79,1.4;-6.19,9.42;0.88,13.66;6.16,7.91;-10.21,-4.96;0.09,4.51
0.7,0.42,-5.04,0.01;-1.4,0.09,0.23,0.03;0.09,0.11,-1.64,0.24
Cos Cos Abs Sub Mul Sin Cbrt Sum Sin x13 Sub x5 x3 Cbrt Sum Sum Sub x13 x10 Cos x12 Sqrt Cbrt x11 Cbrt Sin Sqrt Sum -7.81 Sum x11 x6",

@"5.24,6.24;3.05,9.81;-1.45,10.09;-3,-0.76;0.29,9.62;-4,-1.15;-1.35,3.36;-4.45,-0.22
1.33,-0.95,-0.22,0.01;0,0,0,0;0.81,1.47,-0.16,0.12
Sum Sum Sin Sum Sum Sin Sin Cbrt Mul Cbrt x4 Sqrt x6 Sqrt Sum Sum Sum Cos x3 Cbrt x7 Sum Sin x0 Sum x7 x7 Sum Sin Sum x5 x5 Sum Sin x0 Sum x7 x4 Sin Sum Sum Sum Sum Cbrt x3 Mul x0 x2 Sqrt Sqrt x7 Sqrt Sqrt Sub x5 x5 Cbrt Sum Sqrt Cbrt x5 Sum Sin x7 Sqrt x2 Cbrt Sum Sum Cbrt Cos Sin Sum Cbrt x3 Sqrt x6 Sum Sqrt Sum Cbrt Sum x4 x2 Sin Cbrt x5 Mul Sub Sum Sqrt x3 Mul x7 x6 Sum Cbrt x0 Sqrt x2 Sin Sqrt Sum x1 x7 Sum Sum Sum Sum Sum Cbrt x5 Sum x2 x2 Sqrt Sqrt x1 Sum Cos Sum x7 x3 Mul Sqrt x1 Sum x6 x5 x1 Cos Sqrt Sin Cbrt Mul x1 x4 Cbrt Sin Sum Sqrt Sum Sin Mul Sin Cbrt x2 Cbrt Sum x1 x5 Sqrt Sum Cbrt Sum x3 x0 Cos Sin x4 Mul Cbrt Sum Sum 6.77 Sum Sin x7 Sin x5 Cos Sum Sum x3 x5 Sqrt x3 Sum Cbrt Sub Cbrt Cbrt x1 x6 Mul Sqrt Cbrt Cbrt x2 Sum Sum Sum x4 x4 Cbrt x4 Sub Sum x4 x2 Cbrt x4",

@"0.18,3.71;-3.03,-1.57;-1.96,0.23;-3.29,-1.35;-0.02,0.98;0.1,1.76;-1.02,1.69;-3.38,0.08;-1.67,1.45
-2.99,0.39,1.72,0.24;-5.06,3.27,0.5,0.03;-0.72,0.84,-0.21,0.06
Sqrt Cos Sub Cbrt Sum Sin Sum Cbrt Cos Sum x1 x4 Cbrt Cos Sub x5 x4 Sqrt Cbrt Sin Cbrt Sum x3 x0 Sqrt Sin Cbrt Sum Sin Sub Sum x8 x0 Sum x6 x7 Sub Sum Cbrt x0 Cos x2 Cbrt Sub x7 x8",

@"0.62,1.62;0.15,1.15;-2.49,-1.49;-2.04,-1.04;-2.16,0.05;-0.43,2.53;0.76,1.76
-2.47,-0.18,1.96,0.17;-1.59,1.28,-0.21,0.13;0.09,-0.37,0.15,0.24
Sin Sub Sub Sin Sin Abs Abs Sin Sum x4 Abs Abs Sub x4 x6 Sum Cos Sub Sub Sub Sin Sub Sub Cos x2 Sub x6 x0 Sum Abs x5 Sin x4 Abs Sin Sum Sub x1 x0 Sub x4 x0 Abs Abs Sub Abs Sub x2 x4 Cos Cos x1 Sin Sin Sub Sin Cos Sin x6 Sin Sin Sub x0 x2 Sum Sub Sub x3 Sin Sum Sub Sin Sum x5 x1 Abs Sin x6 Sin Sub Abs x4 Sum x1 x0 Sub Sub Sum Sub Abs Sub x1 x5 Cos Sub 9.2 x1 Abs Sub Cos x0 x4 x3 Sub x3 Sub Sum Sum Sub x0 x2 Sub x0 x0 Sum Sub x6 x0 Sub x1 x4 Sin Sin Sum x1 x3 Sum Sum Sin Sub Sin Sub Sub x1 x2 Sub x3 x5 Sin Sub Sum x5 x3 Sub x2 x0 Sin Abs Cos Abs Sub x0 x0 Sin Sub Sin Cos Abs Abs x4 Sub Sub Abs Sum -9.71 x6 Sin Cos x2 Sub Abs Sub x2 x6 Abs Sin x3 Sub Sum Sin Sub Sub x4 Cos Sub Sin Abs Sub x1 x5 Sub Sub x6 Sub x5 x5 Sin Sub x6 x0 Sin Sub Sin Cos Abs Sub x1 x4 Abs Sum Sin Abs x2 Sub Sin x6 Sub x1 x5 Abs Sum Sub Abs Sub Abs Abs Cos x1 Sub Sin Sub x4 x1 Sub -7.21 Sum 1.41 x1 Sub Sub Sub Sum Sub x6 x1 Sub x1 x3 Sin Sub x6 x1 Cos Abs Sub x0 x1 Sub Cos Sub Sin x5 Abs x3 Sin Abs Abs x4 Abs Sub Sin Sub Sub Cos x3 Sub x4 x3 Sub Sin x3 Sum x4 x4 x2 Sub Sub x0 Abs Sin Sub Abs Cos Sin Sin x5 Sum Sub Abs Cos x3 Sub Cos x0 Sum x0 x0 Sum Sin Sub x0 x3 Abs Sub x6 x1 Sub Cos Abs Sub x0 Sub Sub Sub Sub x0 x1 Sub x3 x2 Sub Sub x5 x5 Sub x2 x1 Sin Sin Cos x4 Sub Sum Abs Sin Sub Sin Cos x4 Sub Abs x0 Sub x3 x2 x0 Cos Cos Sum Sub Sub Sub x1 x5 Sin x4 Sub Abs x4 Abs x6 Sub Sub Sin x0 Sum x3 x4 Sub Sin x4 Sin x3",

@"-4.91,18.22;-0.48,0.52;-16.24,1.53;-2.23,2.73;-12.61,12.85;-8.58,-7.15;-6.7,-5.01;-0.89,20.38;-4.98,-3.59;-0.98,1.45
0,0,0,0;-0.05,-0.32,-0.61,0.28;0.13,-1.08,0.49,0.16
Sin Cbrt Cbrt Sum Mul Sin Sum Sub Sub x1 x8 Sin x2 Sum Abs x11 Sin x7 Sum -5.12 Cbrt Sum Cbrt x12 Sin x13 Sub Sub x9 Cbrt Sum Sum x7 x10 Sum x13 x10 Cbrt Sub Cbrt Cbrt x10 Cbrt Sum x13 x3",

@"-5.6,-0.65;-3.34,16.42;7.11,12.27;-19.04,-1.72;-26.29,2.25;-13.19,-11.9
-0.62,2.18,1.11,0.13;0.08,0.87,0.32,0.3;4.84,6.57,-0.79,0.11
Sin Mul Sqrt Mul Sqrt Sum Sqrt Cos Sqrt x13 Cos Cos Atan Ln x2 Atan Ln Sqrt Sum Sum x2 x11 Sqrt x2 Sqrt Sin Cbrt Cbrt Mul Mul Mul x6 x2 Cos x10 Sin Cbrt x9",

@"-1.45,1.04;-2.23,3.19;0.06,1.06;-0.53,0.47;-0.35,2.27;-0.61,0.53;-0.65,1.63
2.91,-2.3,0.23,0.06;0.24,-0.16,0.01,0.13;-0.16,1.2,0.57,0.07
Sum Sin Sum Sum Atan Atan Pow Sum Sin x2 Sum x5 x2 -2.82 Sin Atan Atan Ln Cos x7 Sin Max Cos Atan Sum Sum x6 x7 Sin x1 x3 Atan Ln Sum Sin Sum Max Sin Atan x6 Cos Cos x4 Cos Sin Sin x1 Atan Pow 8.94 Atan Atan Pow x2 x4",

@"-4.91,0.78;-6.43,2.2;-32.19,-10.83;-14.02,0.81;-7.22,-0.39;-28.19,5.35;-6.37,8.79;-20.77,24.55
-4.18,-0.7,0.31,0.01;0.34,-3.4,-0.68,0.06;-0.46,-0.74,-0.86,0.02
Cbrt Sum Sum Sum Sum Cbrt Cbrt Sum x1 Sub x8 x5 Cbrt Cbrt Sum Cbrt x3 Sum x6 x2 Sum Cbrt Sum Sum Atan x6 Sum x2 x1 Sum Cbrt x4 Sin x6 -0.19 Sum Sum Sin Atan Sum Abs x5 Atan Pow 3.31 x2 Sin Sub Sub x5 Sum x2 x0 Atan Sum x8 x2 Cbrt Sum Cbrt Sum Cbrt x0 Cbrt x4 x8 Sum Cbrt Cbrt Cbrt Sum Cbrt Sum x6 x2 Sub Sub x8 x1 Max x2 x1 Cbrt Sum Sum Sum Mul Cbrt x6 Cbrt x2 Cbrt Sin x8 Cbrt Cbrt Sum x6 x4 Max Atan Sum Sub x4 -1.31 Atan x3 Sin Atan Cbrt x8",

@"-9.8,6.48;-5.86,-0.57;-0.25,1.61;-1.11,-0.11;-11.11,3.55;-3.37,-2.37;-3.52,-1.56;-0.75,1.85
1.38,-3.06,-4.62,0.15;-4.41,0.09,-0.19,0.11;-3.03,1.08,0.4,0.2
Sum Sum Sum Sin Sum Sum Cos Sin Cos Atan Cbrt x2 Sum Sum Cos Sum Cos x6 Sum x1 x1 Sum Sum Cbrt x1 Sum x5 x7 Sum Cos x3 Sum x6 x0 Sum Sum Sum Max x3 x2 Sum x5 x1 Sin Atan x6 Sin Sum Max x3 x2 Cos x5 Sum Sum Sum Sum Sum Sum x3 x4 Sin x6 Sin Max x1 x1 Sum Sum Sum x0 x4 Sum x3 4.85 Sum Sum x3 x1 Sum x6 x3 Max Cbrt Sum Sum x0 x4 Sum x1 x2 Sum Sum Sum x1 x2 Sum x6 x6 Sum Cos x1 3.44 Sum Cos Cos Sum Cbrt x4 Sum x0 x0 Sum Cos Cos Sum x6 x7 Sum x4 Cos Sum x6 x1 Cos Sum Sum Atan Sum Cbrt Cos Cbrt x4 Abs Cos Sum x5 x6 -6.53 Sum Sum Sum 5.91 Atan Sum Sin x7 Cos x4 Sum Sum Sum Sum x5 -7.1 Sum x2 x4 Sum Sum x5 x2 Atan x6 Sin Cbrt Sum x7 x3 Sum Cbrt Sin Cbrt Sum x4 x7 Sin Cos Sum Sin x7 9.11 -1.39 Cbrt Sum Sum -6.21 Sum -4.61 Sum Sum Sum Sum Sin Cos x5 Sum Sum x4 x3 Sum x0 x0 Sin Sum Cos x5 Sin x2 Sum Sum Cbrt Cos x1 Cos Cos x4 Atan Ln Sum x2 x0 Cbrt Sum Sum Cbrt Sum x7 x0 Cos Cbrt x3 Sin Cbrt Cos x4 Sin Sum Sum Sin Sum Sum 8.66 Sum Cos x1 Cos x0 Sum Cos Cos x2 Cbrt Atan x5 Sum Sum Sum Sin Atan x0 Sqrt Cos x3 Cos Max Sum x0 x4 Sin x1 Cos Sum Cbrt Cos x0 Sum Max x2 x2 Sum x6 x2 Cbrt Atan Ln Sum Sum Atan Ln x0 Max x0 x7 Sum Cbrt x2 Sum x6 x4",

@"-16.38,-0.52;-17.98,1.89;-10.18,4.29;-0.04,3.81;0.44,2.43
-1.24,-0.09,0.71,0.12;1.09,-0.27,-1.28,0.11;-0.7,-1.55,0.06,0.16
Sin Sin Sin Sqrt Sin Sum Sqrt Sum Sum Sin Sin Sqrt x4 Sqrt Sqrt Sin x1 Sqrt Sum Sqrt Sqrt x2 Sum Sin x3 Sqrt x1 Sqrt Sin Sum Sin Sum Sin x2 Sin x0 Sin Sin Sqrt x2"
        };
    }
}
