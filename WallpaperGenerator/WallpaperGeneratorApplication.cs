﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WallpaperGenerator.FormulaRendering;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.MainWindowControls.ControlPanelControls;

namespace WallpaperGenerator
{
    public class WallpaperGeneratorApplication : Application
    {
        #region Constants

        private const int RangeLowBound = -50;
        private const int RangeHighBound = 50;
        private const int ColorChannelPolinomialTransformationCoefficientLowBound = -50;
        private const int ColorChannelPolinomialTransformationCoefficientHighBound = 50;
        private const double ColorChannelZeroProbabilty = 0.2;

        private const int ImageWidth = 700;
        private const int ImageHeight = 700;

        #endregion

        #region Fields

        private readonly Random _random = new Random();
        private readonly MainWindow _mainWindow;
        private readonly WallpaperImage _wallpaperImage;
        
        #endregion

        #region Properties

        public FormulaRenderingArguments GetFormulaRenderingArguments()
        {
            return FormulaRenderingArguments.FromString(_mainWindow.FormulaTexBox.Text);
        }

        #endregion


        #region Constructors

        public WallpaperGeneratorApplication()
        {
            _wallpaperImage = new WallpaperImage(ImageWidth, ImageHeight);
            _mainWindow = new MainWindow { WindowState = WindowState.Maximized };

            _mainWindow.ControlPanel.GenerateFormulaButton.Click += (s, a) =>
            {
                FormulaTreeNode formulaTreeRoot = CreateRandomFormulaTreeRoot();
                FormulaTree formulaTree = new FormulaTree(formulaTreeRoot);
                VariableValuesRangesFor2DProjection variableValuesRanges = 
                    VariableValuesRangesFor2DProjection.CreateRandom(_random, formulaTree.Variables.Length, 
                        ImageWidth, ImageHeight, RangeLowBound, RangeHighBound);
                ColorTransformation colorTransformation = ColorTransformation.CreateRandomPolynomialColorTransformation(_random,
                    ColorChannelPolinomialTransformationCoefficientLowBound, ColorChannelPolinomialTransformationCoefficientHighBound,
                    ColorChannelZeroProbabilty);
                FormulaRenderingArguments formulaRenderingArguments = new FormulaRenderingArguments(formulaTree, variableValuesRanges, colorTransformation);

                _mainWindow.FormulaTexBox.Text = formulaRenderingArguments.ToString();
            };

            _mainWindow.ControlPanel.RenderFormulaButton.Click += (s, a) =>
            {
                _mainWindow.Cursor = Cursors.Wait;

                FormulaRenderingArguments formulaRenderingArguments = GetFormulaRenderingArguments();
                
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();  

                RenderedFormulaImage renderedFormulaImage = FormulaRender.Render(
                    formulaRenderingArguments.FormulaTree,
                    formulaRenderingArguments.VariableValuesRanges,
                    formulaRenderingArguments.ColorTransformation, 
                    _wallpaperImage.WidthInPixels, _wallpaperImage.HeightInPixels);

                _wallpaperImage.Update(renderedFormulaImage);
                _mainWindow.WallpaperImage.Source = _wallpaperImage.Source;

                stopwatch.Stop();

                _mainWindow.StatusPanel.RenderedTime = stopwatch.Elapsed; 
                _mainWindow.Cursor = Cursors.Arrow;
            };
        }

        #endregion

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _mainWindow.Show();
        }

        private FormulaTreeNode CreateRandomFormulaTreeRoot()
        {
            int dimensionsCount = (int)_mainWindow.ControlPanel.DimensionsCountSlider.Value;
            int variablesCount = (int)_mainWindow.ControlPanel.VariablesCountSlider.Value;
            int constantsCount = (int)_mainWindow.ControlPanel.ConstantsCountSlider.Value;
            int unaryOperatorsCount = (int)_mainWindow.ControlPanel.UnaryOperatorsCountSlider.Value;
            IEnumerable<OperatorCheckBox> checkedOperatorCheckBoxes = _mainWindow.ControlPanel.OperatorCheckBoxes.Where(cb => cb.IsChecked == true);
            IEnumerable<Operator> operators = checkedOperatorCheckBoxes.Select(cb => cb.Operator);

            return FormulaTreeGenerator.CreateRandomFormulaTree(_random, dimensionsCount, variablesCount, constantsCount, unaryOperatorsCount, operators);
        }
    }
}
