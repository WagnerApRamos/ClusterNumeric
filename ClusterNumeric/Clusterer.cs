using System;

namespace ClusterNumeric
{
    public class Clusterer
    {
        private readonly int _numClusters;
        private readonly Random _rnd;
        private double[][] _centroids;
        private int[] _clustering;
        public Clusterer(int numClusters)
        {
            _numClusters = numClusters;
            _centroids = new double[numClusters][];
            _rnd = new Random(0);
        }

        internal int[] Cluster(double[][] data)
        {
            int numTuples = data.Length;
            int numValues = data[0].Length;
            _clustering = new int[numTuples];

            for (int k = 0; k < _numClusters; ++k) _centroids[k] = new double[numValues];

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

            var result = new int[numTuples];
            Array.Copy(_clustering, result, _clustering.Length);

            return result;
        }

        private bool UpdateClustering(double[][] data)
        {
            var changed = false;
            var newClustering = new int[_clustering.Length];
            Array.Copy(_clustering, newClustering, newClustering.Length);
            var distances = new double[_numClusters];

            for (int i = 0; i < data.Length; i++)
            {
                for (int k = 0; k < _numClusters; k++)
                {
                    distances[k] = Distance(data[i], _centroids[k]);
                }
                var newClusterId = MinIndex(distances);

                if (newClusterId != newClustering[i])
                {
                    changed = true;
                    newClustering[i] = newClusterId;
                }
            }

            if (changed == false) return false;

            var clusterCounts = new int[_numClusters];
            for (int i = 0; i < data.Length; i++)
            {
                var clusterId = newClustering[i];
                ++clusterCounts[clusterId];
            }

            for (int k = 0; k < _numClusters; k++)
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
        }

        private void UpdateCentroids(double[][] data)
        {
            var clusterCounts = new int[_numClusters];
            for (int i = 0; i < data.Length; i++)
            {
                var clusterId = _clustering[i];
                ++clusterCounts[clusterId];
            }

            for (int k = 0; k < _centroids.Length; k++)
            {
                for (int j = 0; j < _centroids[k].Length; j++)
                {
                    _centroids[k][j] = 0.0;
                }
            }

            for (int i = 0; i < data.Length; i++)
            {
                var clusterId = _clustering[i];
                for (int j = 0; j < data[i].Length; j++)
                {
                    _centroids[clusterId][j] += data[i][j];
                }
            }

            for (int k = 0; k < _centroids.Length; k++)
            {
                for (int j = 0; j < _centroids[k].Length; j++)
                {
                    _centroids[k][j] /= clusterCounts[k];
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
                if (clusterId == _numClusters) clusterId = 0;
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
