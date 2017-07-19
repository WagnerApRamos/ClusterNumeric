#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace WarSystem.MachineLearning.Regressions
{
    #region Interfaces

    public interface ISimpleLinearRegression
    {
        double RSquared { get; }
        double YIntercept { get; }
        double Slope { get; }
        double ResidualSumOfSquare { get; }
        SimpleRegressionType RegressionType { get; }


        void Fit(double[] x, double[] y);
        IEnumerable<double> Predict(double[] x);
    }

    public enum SimpleRegressionType
    {
        Exponential,
        Inverse,
        Linear,
        Logarithmic,
        Power
    }

    #endregion
    
    /// <summary>
    /// Class to Create a simple linear regression applying machine learning techniques.
    /// </summary>
    public class SimpleRegression :
        ISimpleLinearRegression
    {
        #region Properties, Variables and Constructors

        /// <summary>
        /// Residual squared.
        /// </summary>
        public double RSquared { private set; get; }

        /// <summary>
        /// This represents the ˆβ1 of estimated coefficient. As known like "a".
        /// </summary>
        public double Slope { private set; get; }

        /// <summary>
        /// This represents the ˆβ0 of estimated coefficient. As known like "b".
        /// </summary>
        public double YIntercept { private set; get; }

        /// <summary>
        /// Type of the prediction method to be applied.
        /// </summary>
        public SimpleRegressionType RegressionType { private set; get; }
        
        /// <summary>
        /// Residual sum of square.
        /// </summary>
        public double ResidualSumOfSquare { private set;  get; }

        /// <summary>
        /// Calculates the prediction.
        /// arg1: Slope,
        /// arg2: Y Intercept,
        /// arg3 X - Undependable variable.
        /// 
        /// Returns predicted Y
        /// </summary>
        /// <returns>returns the predicted value</returns>
        private readonly Func<double, double, double, double> _funcRegression;

        /// <summary>
        /// Constructor of Simple Linear Regression.
        /// By default, the class will use the Linear method to predict.
        /// </summary>
        public SimpleRegression() 
            : this(SimpleRegressionType.Linear)
        { }

        /// <summary>
        /// Constructor of Simple Linear Regression.
        /// </summary>
        /// <param name="regressionType">Type of the prediction method to be applied.</param>
        public SimpleRegression(SimpleRegressionType regressionType)
        {
            RegressionType = regressionType;

        } //SimpleLinearRegression

        #endregion
        
        /// <summary>
        /// Trains the Model.
        /// </summary>
        /// <param name="xVals">Array of undependable variable</param>
        /// <param name="yVals">Array of dependable variable</param>
        public void Fit(double[] xVals, double[] yVals)
        {
            if (xVals.Length != yVals.Length) throw new ArgumentException("The dimension of xVals has to be the same as yVals.");

            var count = xVals.Length;

            double sumOfX = 0, sumOfY = 0, sumOfXSq = 0, sumOfYSq = 0, sumCodeviates = 0;
            Thread.Sleep(100);
            for (int i = 0; i < count; i++)
            {
                double x = xVals[i], y = yVals[i];
                sumCodeviates += (x * y);
                sumOfX += x;
                sumOfXSq += (x * x);
                sumOfY += y;
                sumOfYSq += (y * y);
            };

            var ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
            var ssY = sumOfYSq - ((sumOfY * sumOfY) / count);
            var meanX = sumOfX / count;
            var meanY = sumOfY / count;

            RSquared = ((count * sumCodeviates) - (sumOfX * sumOfY)) /
                       ((count * sumOfXSq - (sumOfX * sumOfX)) * (count * sumOfYSq - (sumOfY * sumOfY)));
            RSquared *= RSquared;

            Slope = (sumOfX - meanX) * (sumOfY - meanY) / ssX;
            YIntercept = meanY - (Slope * meanX);

            RSS(xVals, yVals);
        } //Fit

        /// <summary>
        /// Calculates the Residual of Sum of Square.
        /// </summary>
        /// <param name="x">Array of undependable variable</param>
        /// <param name="y">Array of dependable variable</param>
        private void RSS(double[] x, double[] y)
        {
            ResidualSumOfSquare = 0;
            for (int i = 0; i < x.Length; i++)
            {
                ResidualSumOfSquare += Math.Sqrt(y[i] - YIntercept - (Slope * x[i]));
            }
        } //RSS
        /// <summary>
        /// Predicts the array  of X.
        /// </summary>
        /// <param name="xVals">Arra of undependable variable</param>
        /// <returns>returns an enumerable of predicted Y</returns>
        public IEnumerable<double> Predict(double[] xVals)
        {
            foreach (var x in xVals)
            {
                yield return _funcRegression(Slope, YIntercept, x);
            }
        } //Predict

    }
}
