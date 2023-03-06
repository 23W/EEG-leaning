using EEGCore.Data;
using EEGCore.Processing;
using EEGCore.Processing.ICA;
using EEGCore.Utilities;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;
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
        public void Test1_Decomposition()
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
                var icaComponents = ica.Decompose(mixedRecord);
                aEst = Matrix<double>.Build.DenseOfRowArrays(icaComponents.A);

                // find sources that correlate with components
                foreach (var (component, componentIndex) in icaComponents.Leads.WithIndex())
                {
                    var correlations = sourceRecord.Leads
                                                   .Select(lead => Correlation.Pearson(lead.Samples, component.Samples))
                                                   .ToList();
                    var foundIndex = correlations.FindIndex(correlation => Math.Abs(correlation) > 0.9);

                    isValidDecomposition = foundIndex >= 0;
                    if (isValidDecomposition)
                    {
                        permitations[componentIndex] = foundIndex;
                    }
                    else
                    {
                        if (attempt == MaxDecompositionAttampts)
                        {
                            Assert.IsTrue(false, $"Component {componentIndex} was not found in Sources in {MaxDecompositionAttampts} attempts");
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

        [TestMethod]
        public void Test2_Composition()
        {
            var (dataRecord, a) = ReadSource("Test1_X.arff", "Test1_A.json");
            var sourceRecord = new ICARecord()
            {
                Name = dataRecord.Name,
                SampleRate = dataRecord.SampleRate,
                A = a.ToRowArrays(),
                W = a.PseudoInverse().ToRowArrays(),
                Leads = dataRecord.Leads.Select(lead => new ComponentLead()
                {
                    Name = lead.Name,
                    ComponentType = ComponentType.Unknown,
                    Samples = lead.Samples,
                }).Cast<Lead>().ToList(),
            };

            var mixedRecord = new Record()
            {
                Name = dataRecord.Name,
                SampleRate = dataRecord.SampleRate,
                Leads = dataRecord.Leads.Select(lead => new Lead()
                {
                    Name = lead.Name,
                    Samples = lead.Samples,
                }).ToList(),
            };

            // manual build mixture
            {
                var s = Matrix<double>.Build.DenseOfRowArrays(dataRecord.GetLeadMatrix());
                var x = a * s;

                mixedRecord.SetLeadMatrix(x.ToRowArrays());
            }

            // do ICA composition
            var ica = new FastICA();
            var icaMixedResult = ica.Compose(sourceRecord);

            // compare result
            Assert.IsTrue(mixedRecord.LeadsCount == icaMixedResult.LeadsCount);
            Assert.IsTrue(mixedRecord.SampleRate == icaMixedResult.SampleRate);

            for (var leadIndex = 0; leadIndex < mixedRecord.LeadsCount; leadIndex++)
            {
                var mixedLead = new DenseVector(mixedRecord.Leads[leadIndex].Samples);
                var icaMixedLead = new DenseVector(icaMixedResult.Leads[leadIndex].Samples);

                var correlation = Correlation.Pearson(mixedLead, icaMixedLead);
                var vectorEquality = mixedLead.AlmostEqual(icaMixedLead, 6);

                Assert.IsTrue(correlation > 0.999);
                Assert.IsTrue(vectorEquality);
            }
        }

        #region Helper Methods

        (Record, Matrix<double>) ReadSource(string sourceFile, string mixingMatrixFile)
        {
            var factory = new RecordFactory();
            var source = factory.FromFile(sourceFile, new RecordFactoryOptions() { SortLeads = false, ZeroMean = false });

            var matrix = JsonSerializer.Deserialize<double[][]>(File.ReadAllText(mixingMatrixFile));
            var mixingMatrix = Matrix<double>.Build.DenseOfRowArrays(matrix);

            return (source, mixingMatrix);
        }

        #endregion
    }
}