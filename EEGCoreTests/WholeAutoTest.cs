using EEGCore.Data;
using EEGCore.Processing;
using EEGCore.Processing.Analysis;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;

namespace EEGCoreTests
{
    [TestClass]
    [DeploymentItem("TestData\\")]
    public class WholeAutoTest
    {
        public TestContext? TestContext { get; set; }

        [TestMethod]
        public void Test1_EyeArtifactAutoRemove()
        {
            var factory = new RecordFactory();
            var source = factory.FromFile("cleanEEG.arff", RecordFactoryOptions.DefaultEEGNoFilter);
            var artifact = factory.FromFile("eyeA_1Hz.arff", RecordFactoryOptions.DefaultEEGNoFilter);
            var mixture = source.Clone();

            // mixing
            {
                const double mixingFactor = 2;

                Assert.AreEqual(source.LeadsCount, artifact.LeadsCount);
                for (var leadIndex = 0; leadIndex < 2; leadIndex++)
                {
                    var sourceLead = source.Leads[leadIndex];
                    var artifactLead = artifact.Leads[leadIndex];
                    var mixtureLead = mixture.Leads[leadIndex];

                    var src = new DenseVector(sourceLead.Samples);
                    var art = new DenseVector(artifactLead.Samples);
                    var dst = src.Add(art.Multiply(mixingFactor));

                    mixtureLead.Samples = dst.ToArray();
                }
            }

            // auto cleaning
            var autoCleaner = new AutoArtifactCleaner() { Input = mixture, CleanReferenceElectrodeArtifacts = false, CleanSingleElectrodeArtifacts = false };
            var result = autoCleaner.Analyze();
            Assert.IsTrue(result.Succeed);

            // compare result
            {
                var estimation = result.Output!;
                Assert.AreEqual(source.LeadsCount, artifact.LeadsCount);
                Assert.IsTrue(result.Ranges.All(r => r.HasEyeArtifact));

                factory.ToFile("autoclean1_mixedEEG.arff", mixture);
                factory.ToFile("autoclean1_estimationEEG.arff", estimation);

                var rangeLength = (int)Math.Round(source.SampleRate * 17);
                var correlation = 0.0;
                for (var leadIndex = 0; leadIndex < source.LeadsCount; leadIndex++)
                {
                    var sourceLeadSamples = source.Leads[leadIndex].Samples.Take(rangeLength);
                    var estimationLeadSamples = estimation.Leads[leadIndex].Samples.Take(rangeLength);

                    correlation += Correlation.Pearson(sourceLeadSamples, estimationLeadSamples);
                }

                correlation /= source.LeadsCount;

                Assert.IsTrue(correlation > 0.8);

                TestContext?.Write($"EyeArtifact 1Hz was found and cleaned. Clean and Source correlation is {correlation*100} %");
            }
        }

        [TestMethod]
        public void Test2_EyeArtifactAutoRemove()
        {
            var factory = new RecordFactory();
            var source = factory.FromFile("cleanEEG.arff", RecordFactoryOptions.DefaultEEGNoFilter);
            var artifact = factory.FromFile("eyeA_2Hz.arff", RecordFactoryOptions.DefaultEEGNoFilter);
            var mixture = source.Clone();

            // mixing
            {
                const double mixingFactor = 2;

                Assert.AreEqual(source.LeadsCount, artifact.LeadsCount);
                for (var leadIndex = 0; leadIndex < 2; leadIndex++)
                {
                    var sourceLead = source.Leads[leadIndex];
                    var artifactLead = artifact.Leads[leadIndex];
                    var mixtureLead = mixture.Leads[leadIndex];

                    var src = new DenseVector(sourceLead.Samples);
                    var art = new DenseVector(artifactLead.Samples);
                    var dst = src.Add(art.Multiply(mixingFactor));

                    mixtureLead.Samples = dst.ToArray();
                }
            }

            // auto cleaning
            var autoCleaner = new AutoArtifactCleaner() { Input = mixture, CleanReferenceElectrodeArtifacts = false, CleanSingleElectrodeArtifacts = false };
            var result = autoCleaner.Analyze();
            Assert.IsTrue(result.Succeed);

            // compare result
            {
                var estimation = result.Output!;
                Assert.AreEqual(source.LeadsCount, artifact.LeadsCount);
                Assert.IsTrue(result.Ranges.All(r => r.HasEyeArtifact));

                factory.ToFile("autoclean2_mixedEEG.arff", mixture);
                factory.ToFile("autoclean2_estimationEEG.arff", estimation);

                var rangeLength = (int)Math.Round(source.SampleRate * 17);
                var correlation = 0.0;
                for (var leadIndex = 0; leadIndex < source.LeadsCount; leadIndex++)
                {
                    var sourceLeadSamples = source.Leads[leadIndex].Samples.Take(rangeLength);
                    var estimationLeadSamples = estimation.Leads[leadIndex].Samples.Take(rangeLength);

                    correlation += Correlation.Pearson(sourceLeadSamples, estimationLeadSamples);
                }

                correlation /= source.LeadsCount;

                Assert.IsTrue(correlation > 0.8);
             
                TestContext?.Write($"EyeArtifact 1Hz was found and cleaned. Clean and Source correlation is {correlation*100} %");
            }
        }
    }
}
