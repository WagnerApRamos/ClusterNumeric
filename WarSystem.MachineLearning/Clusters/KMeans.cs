#region Using

using System;
using System.Collections.Generic;

#endregion

namespace WarSystem.MachineLearning.Clusters
{
    #region Interfaces 

    public interface IKMeans
    {
        double[][] Centroids { get; }
        int Clusters { get; }

        void Fit(double[][] data);
        IEnumerable<int> Predict(double[][] data);
    }

    #endregion

    public class KMeans :
        IKMeans
    {
        #region Constructors and Variables

        private readonly Random _rnd;
        private int[] _clustering;

        public KMeans(int numClusters)
        {
            Clusters = numClusters;
            Centroids = new double[numClusters][];
            _rnd = new Random(0);
        }

        public KMeans(double[][] centroids)
        {
            Centroids = centroids;
            Clusters = centroids.Length;
            _rnd = new Random(0);
        }

        #endregion

        public int Clusters { private set; get; }
        public double[][] Centroids { private set; get; }

        public void Fit(double[][] data)
        {
            int numTuples = data.Length;
            int numValues = data[0].Length;
            _clustering = new int[numTuples];

            for (int k = 0; k < Clusters; ++k) Centroids[k] = new double[numValues];

            InitRandom(data);

            Console.WriteLine("\nInitial random clustering:");
            for (int i = 0; i < _clustering.Length; i++) Console.Write(_clustering[i] + " ");
            Console.WriteLine("\n");

            bool changed = true;
            int maxCount = numTuples * 10;
            int ct = 0;

            while (changed == true && ct < maxCount)
            {
                ++ct;
                UpdateCentroids(data);
                changed = UpdateClustering(data);
            }
        } // Fit

        public IEnumerable<int> Predict(double[][] data)
        {
            var clusters = new List<int>();
            var distances = new double[Clusters];

            for (int i = 0; i < data.Length; i++)
            {
                for (int k = 0; k < Clusters; k++)
                {
                    distances[k] = Distance(data[i], Centroids[k]);
                } //k
                yield return MinIndex(distances);
            } //i
        } //Predict

        private bool UpdateClustering(double[][] data)
        {
            var changed = false;
            var newClustering = new int[_clustering.Length];
            Array.Copy(_clustering, newClustering, newClustering.Length);
            var distances = new double[Clusters];

            for (int i = 0; i < data.Length; i++)
            {
                for (int k = 0; k < Clusters; k++)
                {
                    distances[k] = Distance(data[i], Centroids[k]);
                }
                var newClusterId = MinIndex(distances);

                if (newClusterId != newClustering[i])
                {
                    changed = true;
                    newClustering[i] = newClusterId;
                }
            }

            if (changed == false) return false;

            var clusterCounts = new int[Clusters];
            for (int i = 0; i < data.Length; i++)
            {
                var clusterId = newClustering[i];
                ++clusterCounts[clusterId];
            }

            for (int k = 0; k < Clusters; k++)
            {
                if (clusterCounts[k] == 0) return false;
            }

            Array.Copy(newClustering, _clustering, newClustering.Length);

            return true;

        } //UpdateClustering

        private int MinIndex(double[] distances)
        {
            int indexOfMin = 0;
            var smallDist = distances[0];
            for (int k = 1; k < distances.Length; ++k)
            {
                if (distances[k] < smallDist)
                {
                    smallDist = distances[k];
                    indexOfMin = k;
                }
            }
            return indexOfMin;
        } //MinIndex

        private static double Distance(double[] tuple, double[] centroid)
        {
            var sumSquaredDiffs = 0.0;
            for (int j = 0; j < tuple.Length; ++j)
                sumSquaredDiffs += Math.Pow(tuple[j] - centroid[j], 2);
            return Math.Sqrt(sumSquaredDiffs);
        } //Distance

        private void UpdateCentroids(double[][] data)
        {
            var clusterCounts = new int[Clusters];
            for (int i = 0; i < data.Length; i++)
            {
                var clusterId = _clustering[i];
                ++clusterCounts[clusterId];
            }

            for (int k = 0; k < Centroids.Length; k++)
            {
                for (int j = 0; j < Centroids[k].Length; j++)
                {
                    Centroids[k][j] = 0.0;
                }
            }

            for (int i = 0; i < data.Length; i++)
            {
                var clusterId = _clustering[i];
                for (int j = 0; j < data[i].Length; j++)
                {
                    Centroids[clusterId][j] += data[i][j];
                }
            }

            for (int k = 0; k < Centroids.Length; k++)
            {
                for (int j = 0; j < Centroids[k].Length; j++)
                {
                    Centroids[k][j] /= clusterCounts[k];
                }
            }

        } //UpdateCentroids

        private void InitRandom(double[][] data)
        {
            var numTuples = data.Length;
            var clusterId = 0;
            for (int i = 0; i < numTuples; ++i)
            {
                _clustering[i] = clusterId++;
                if (clusterId == Clusters) clusterId = 0;
            }

            for (int i = 0; i < numTuples; i++)
            {
                var r = _rnd.Next(i, _clustering.Length);
                var tmp = _clustering[r];
                _clustering[r] = _clustering[i];
                _clustering[i] = tmp;
            }
        } // InitRandom
    }
}
