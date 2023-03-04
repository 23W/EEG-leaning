using EEGCore.Data;
using EEGCore.Processing;
using EEGCore.Processing.ICA;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;
using System.Diagnostics;
using System.Text.Json;

namespace EEGCoreTests
{
    [TestClass]
    [DeploymentItem("TestData\\")]
    public class ICATest
    {
        int MaxDecompositionAttampts => 5;

        public TestContext? TestContext { get; set; }

        [TestMethod]
        public void Decomposition_Test1()
        {
            // read data
            var (sourceRecord, a) = ReadSource("Test1_X.arff", "Test1_A.json");
            var aEst = a;
            var mixedRecord = sourceRecord.Clone();

            // build mixture
            {
                var s = Matrix<double>.Build.DenseOfRowArrays(sourceRecord.GetLeadMatrix());
                var x = a * s;

                mixedRecord.SetLeadMatrix(x.ToRowArrays());
            }

            // do ICA decomposition, limit by attempts
            var attempt = 0;
            var isValidDecomposition = false;
            var permitations = default(int[]);
            do
            {
                permitations = new int[sourceRecord.LeadsCount];
                attempt++;

                var ica = new FastICA()
                {
                    MaxIterationCount = 10000,
                    Tolerance = 1E-06,
                };

                // decompose
                var icaComponents = ica.Solve(mixedRecord);
                aEst = Matrix<double>.Build.DenseOfRowArrays(icaComponents.A);

                // find sources that correlate with components
                foreach (var component in icaComponents.Leads.Select((component, index) => (component.Samples, index)))
                {
                    var correlations = sourceRecord.Leads
                                                   .Select(lead => Correlation.Pearson(lead.Samples, component.Samples))
                                                   .ToList();
                    var foundIndex = correlations.FindIndex(correlation => Math.Abs(correlation) > 0.9);

                    isValidDecomposition = foundIndex >= 0;
                    if (isValidDecomposition)
                    {
                        permitations[component.index] = foundIndex;
                    }
                    else
                    {
                        if (attempt == MaxDecompositionAttampts)
                        {
                            Assert.IsTrue(false, $"Component {component.index} was not found in Sources in {MaxDecompositionAttampts} attempts");
                        }
                        else
                        {
                            // we will try again
                            break;
                        }
                    }
                }
            }
            while (!isValidDecomposition && (attempt < MaxDecompositionAttampts));

            // Normilize (L2 Norm) A and A estimation by columns
            var aNorm = a;
            var aEstNorm = aEst;
            {
                var norms = aNorm.ColumnNorms(2.0);
                var it = a.EnumerateColumns().Select((column, index) => column / norms[index]);
                aNorm = Matrix<double>.Build.DenseOfColumnVectors(it);


                norms = aEst.ColumnNorms(2.0);
                it = aEst.EnumerateColumns().Select((column, index) => column / norms[index]);
                aEstNorm = Matrix<double>.Build.DenseOfColumnVectors(it);
            }

            // Output the first valid result
            TestContext?.WriteLine($"Valid in {attempt} attemp(s) from {MaxDecompositionAttampts}");
            TestContext?.WriteLine($"Permutations: {string.Join(",", permitations)}");
            TestContext?.Write($"L2 Nrom A src: {aNorm}");
            TestContext?.Write($"L2 Nrom A est: {aEstNorm}");
        }

        (Record, Matrix<double>) ReadSource(string sourceFile, string mixingMatrixFile)
        {
            var factorey = new RecordFactory();
            var source = factorey.FromFile(sourceFile, new RecordFactoryOptions() { SortLeads = false, ZeroMean = false });

            var matrix = JsonSerializer.Deserialize<double[][]>(File.ReadAllText(mixingMatrixFile));
            var mixingMatrix = Matrix<double>.Build.DenseOfRowArrays(matrix);

            return (source, mixingMatrix);
        }
    }
}