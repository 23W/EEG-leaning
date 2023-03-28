using EEGCore.Data;
using EEGCore.Processing;
using EEGCore.Utilities;

namespace EEGCoreTests
{
    [TestClass]
    [DeploymentItem("TestData\\")]
    public class BaseTests
    {
        static IEnumerable<EEGLead> Tes1Leads => new List<EEGLead>()
        {
            new EEGLead() { Name = "AF3", LeadType = LeadType.Frontal },
            new EEGLead() { Name = "AF4", LeadType = LeadType.Frontal },
            new EEGLead() { Name = "F3", LeadType = LeadType.Frontal },
            new EEGLead() { Name = "F4", LeadType = LeadType.Frontal },
            new EEGLead() { Name = "F7", LeadType = LeadType.Frontal },
            new EEGLead() { Name = "F8", LeadType = LeadType.Frontal },
            new EEGLead() { Name = "FC5", LeadType = LeadType.Frontal },
            new EEGLead() { Name = "FC6", LeadType = LeadType.Frontal },
            new EEGLead() { Name = "T7", LeadType = LeadType.Temporal },
            new EEGLead() { Name = "T8", LeadType = LeadType.Temporal },
            new EEGLead() { Name = "P7", LeadType = LeadType.Parietal },
            new EEGLead() { Name = "P8", LeadType = LeadType.Parietal },
            new EEGLead() { Name = "O1", LeadType = LeadType.Occipital },
            new EEGLead() { Name = "O2", LeadType = LeadType.Occipital },
        };

        [TestMethod]
        public void Test1_LeadCode()
        {
            foreach(var leadCode in Enum.GetValues(typeof(LeadCode))
                                        .Cast<LeadCode>()
                                        .Select(l => Tuple.Create(l.ToString().ToLower(), l)))
            {
                Assert.IsTrue(DataUtilities.GetEEGLeadCodeByName(leadCode.Item1) == leadCode.Item2);
            }

            Assert.IsTrue(DataUtilities.GetEEGLeadCodeByName("Unknown Name") == default(LeadCode?));
        }

        [TestMethod]
        public void Test1_ReadFactory()
        {
            var factory = new RecordFactory();
            var record = factory.FromFile("Test2_EEG.arff", new RecordFactoryOptions() { SortLeads = true, ZeroMean = false });

            Assert.IsTrue(record.LeadsCount == 14);

            foreach (var (actLead, leadIndex) in Tes1Leads.WithIndex())
            {
                var estLead = record.Leads[leadIndex];

                Assert.IsTrue(estLead.GetType() == actLead.GetType());
                Assert.IsTrue(estLead.Name.Equals(actLead.Name));
                Assert.IsTrue((estLead as EEGLead)!.LeadType == actLead.LeadType);
            }
        }

        [TestMethod]
        public void Test2_Clone()
        {
            var factory = new RecordFactory();
            var actRecord = factory.FromFile("Test2_EEG.arff", new RecordFactoryOptions() { SortLeads = true, ZeroMean = false });
            var estRecord = actRecord.Clone();

            Assert.IsTrue(actRecord.LeadsCount == estRecord.LeadsCount);
            Assert.IsTrue(actRecord.Duration == estRecord.Duration);
            Assert.IsTrue(actRecord.SampleRate == estRecord.SampleRate);

            foreach (var (actLead, leadIndex) in actRecord.Leads.WithIndex())
            {
                var estLead = estRecord.Leads[leadIndex];

                Assert.IsTrue(estLead.GetType() == actLead.GetType());
                Assert.IsTrue(estLead.Name.Equals(actLead.Name));
                Assert.IsTrue((estLead as EEGLead)!.LeadType == (actLead as EEGLead)!.LeadType);
            }
        }
    }
}
