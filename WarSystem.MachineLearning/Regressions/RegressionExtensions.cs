using System;

namespace WarSystem.MachineLearning.Regressions
{
    internal static class RegressionExtensions
    {
        /// <summary>
        /// Calculates the prediction by exponential method: "a * b^x".
        /// </summary>
        /// <param name="a">Slope variable</param>
        /// <param name="b">Y Intercept variable</param>
        /// <param name="x">Undependable variable</param>
        /// <returns>returns the predicted value</returns>
        internal static double ExponentialCalc(double a, double b, double x)
        {
            return a * Math.Pow(b, x);
        } //ExponentialCalc

        /// <summary>
        /// Calculates the prediction by Inverse method: "a / x + b".
        /// </summary>
        /// <param name="a">Slope variable</param>
        /// <param name="b">Y Intercept variable</param>
        /// <param name="x">Undependable variable</param>
        /// <returns>returns the predicted value</returns>
        internal static double InverseCalc(double a, double b, double x)
        {
            return a / x + b;
        } //InverseCalc

        /// <summary>
        /// Calculates the prediction by Linear method: "a * x + b".
        /// </summary>
        /// <param name="a">Slope variable</param>
        /// <param name="b">Y Intercept variable</param>
        /// <param name="x">Undependable variable</param>
        /// <returns>returns the predicted value</returns>
        internal static double LinearCalc(double a, double b, double x)
        {
            return a * x + b;
        } //LinearCalc

        /// <summary>
        /// Calculates the prediction by Logarithmic method: "a * log(x) + b".
        /// </summary>
        /// <param name="a">Slope variable</param>
        /// <param name="b">Y Intercept variable</param>
        /// <param name="x">Undependable variable</param>
        /// <returns>returns the predicted value</returns>
        internal static double LogarithmicCalc(double a, double b, double x)
        {
            return a * Math.Log(x) + b;
        } //LogarithmicCalc

        /// <summary>
        /// Calculates the prediction by Power method: "a * x^b".
        /// </summary>
        /// <param name="a">Slope variable</param>
        /// <param name="b">Y Intercept variable</param>
        /// <param name="x">Undependable variable</param>
        /// <returns>returns the predicted value</returns>
        internal static double PowerCalc(double a, double b, double x)
        {
            return a * Math.Pow(x, b);
        } //PowerCalc

        internal static Func<double, double, double, double> GetFuncRegression(this SimpleRegression regression)
        {
            switch (regression.RegressionType)
            {
                case SimpleRegressionType.Exponential:
                    return ExponentialCalc;

                case SimpleRegressionType.Inverse:
                    return InverseCalc;

                case SimpleRegressionType.Linear:
                    return LinearCalc;

                case SimpleRegressionType.Logarithmic:
                    return LogarithmicCalc;

                case SimpleRegressionType.Power:
                    return PowerCalc;

                default:
                    return null;
            }
        }
    }
}
