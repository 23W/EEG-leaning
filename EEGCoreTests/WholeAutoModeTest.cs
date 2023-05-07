using EEGCore.Data;
using EEGCore.Processing;
using EEGCore.Processing.Analysis;
using EEGCore.Processing.Generators;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;

namespace EEGCoreTests
{
    [TestClass]
    [DeploymentItem("TestData\\")]
    public class WholeAutoModeTest
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
             
                TestContext?.Write($"EyeArtifact 2Hz was found and cleaned. Clean and Source correlation is {correlation*100} %");
            }
        }

        [TestMethod]
        public void Test3_ReferenceArtifactAutoRemove()
        {
            var factory = new RecordFactory();
            var source = factory.FromFile("cleanEEG.arff", RecordFactoryOptions.DefaultEEGNoFilter);
            var mixture = source.Clone();

            // mixing
            {
                var generator = new SinGenerator() { Amplitude = 40, Frequence = 10, SampleRate = source.SampleRate};
                mixture.Leads.ForEach(l => generator.Generate(l.Samples, true));
            }

            // auto cleaning
            var autoCleaner = new AutoArtifactCleaner() { Input = mixture, WholeRecord = true, CleanEyeArtifacts = false, CleanSingleElectrodeArtifacts = false };
            var result = autoCleaner.Analyze();
            Assert.IsTrue(result.Succeed);

            // compare result
            {
                var estimation = result.Output!;
                Assert.IsTrue(result.Ranges.All(r => r.HasReferenceElectrodeArtifact));

                factory.ToFile("autoclean3_mixedEEG.arff", mixture);
                factory.ToFile("autoclean3_estimationEEG.arff", estimation);

                var rangeLength = (int)Math.Round(source.SampleRate * 17);
                var correlation = 0.0;
                for (var leadIndex = 0; leadIndex < source.LeadsCount; leadIndex++)
                {
                    var sourceLeadSamples = source.Leads[leadIndex].Samples.Take(rangeLength);
                    var estimationLeadSamples = estimation.Leads[leadIndex].Samples.Take(rangeLength);

                    correlation += Correlation.Pearson(sourceLeadSamples, estimationLeadSamples);
                }

                correlation /= source.LeadsCount;

                Assert.IsTrue(correlation > 0.65);

                TestContext?.Write($"Reference Electrode Artifact was found and cleaned. Clean and Source correlation is {correlation * 100} %");
            }
        }

        [TestMethod]
        public void Test4_SingleArtifactAutoRemove()
        {
            var factory = new RecordFactory();
            var source = factory.FromFile("cleanEEG.arff", RecordFactoryOptions.DefaultEEGNoFilter);
            var mixture = source.Clone();

            // mixing
            {
                var generator = new SinGenerator() { Amplitude = 40, Frequence = 13, SampleRate = source.SampleRate };
                mixture.Leads.Skip(1).Take(1).ToList().ForEach(l => generator.Generate(l.Samples, true));
            }

            // auto cleaning
            var autoCleaner = new AutoArtifactCleaner() { Input = mixture, WholeRecord = true, CleanEyeArtifacts = false, CleanReferenceElectrodeArtifacts = false };
            var result = autoCleaner.Analyze();
            Assert.IsTrue(result.Succeed);

            // compare result
            {
                var estimation = result.Output!;
                Assert.IsTrue(result.Ranges.All(r => r.HasSingleElectrodeArtifact));

                factory.ToFile("autoclean4_mixedEEG.arff", mixture);
                factory.ToFile("autoclean4_estimationEEG.arff", estimation);

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

                TestContext?.Write($"Single Electrode Artifact was found and cleaned. Clean and Source correlation is {correlation * 100} %");
            }
        }
    }
}
