using EEGCore.Data;
using EEGCore.Processing.Model;
using MathNet.Numerics;
using System.Text.Json;

namespace EEGCoreTests
{
    [TestClass]
    public class ScalpSphereTests
    {
        [TestMethod]
        public void Test1_XYZ()
        {
            // all 10-20 leads except synonyms
            var leads = Enum.GetValues(typeof(LeadCode))
                            .Cast<LeadCode>()
                            .Where(l => !((l >= LeadCode.T7) && (l <= LeadCode.P8)))
                            .ToList();

            foreach (var lead in leads)
            {
                var leadXYZ = ScalpSphere.GetLeadXYZ(lead);
                Assert.IsFalse(leads.Where(l => l != lead)
                                    .Any(l =>
                                    {
                                        var ortherXYZ = ScalpSphere.GetLeadXYZ(l);
                                        var res = ortherXYZ.X.AlmostEqual(leadXYZ.X) &&
                                                  ortherXYZ.Y.AlmostEqual(leadXYZ.Y) &&
                                                  ortherXYZ.Z.AlmostEqual(leadXYZ.Z);
                                        return res;
                                     }));
            }
        }

        [TestMethod]
        public void Test1_Spherical()
        {
            foreach (var lead in Enum.GetValues(typeof(LeadCode)).Cast<LeadCode>())
            {
                var sCoordinate = EEGScheme.Scheme1020[lead].Coordinate;
                Assert.IsTrue(sCoordinate.R.AlmostEqual(1, 2));

                var eCoordinate = ScalpSphere.CalcSpherical(ScalpSphere.CalcXYZ(sCoordinate));
                Assert.IsTrue(eCoordinate.Alpha.AlmostEqual(sCoordinate.Alpha, 5) &&
                              eCoordinate.Beta.AlmostEqual(sCoordinate.Beta, 5) &&
                              eCoordinate.R.AlmostEqual(sCoordinate.R, 5));
            }
        }

        [TestMethod]
        public void Test1_CenterToNasionDipole()
        {
            var dipole = new Dipole()
            {
                Location = new PolarCoordinate(0, 0, 0),
                Moment = new PolarCoordinate(0, 0, 1)
            };

            var positive = dipole.CalcPotential(EEGScheme.Scheme1020[LeadCode.Fpz].Coordinate);
            var negative = dipole.CalcPotential(EEGScheme.Scheme1020[LeadCode.Oz].Coordinate);

            Assert.IsTrue(positive > 0);
            Assert.IsTrue(negative < 0);
            Assert.IsTrue(Math.Abs(positive).AlmostEqual(Math.Abs(negative), 5));

            foreach (var lead in new[] { LeadCode.T3, LeadCode.C3, LeadCode.Cz, LeadCode.C4, LeadCode.T4 })
            {
                var zero = dipole.CalcPotential(EEGScheme.Scheme1020[lead].Coordinate);
                Assert.IsTrue(zero.AlmostEqual(0, 5) || Math.Abs(positive / zero) > 1000);
            }
        }

        [TestMethod]
        public void Test1_CenterToInionDipole()
        {
            var dipole = new Dipole()
            {
                Location = new PolarCoordinate(0, 0, 0),
                Moment = new PolarCoordinate(180, 0, 1)
            };

            var positive = dipole.CalcPotential(EEGScheme.Scheme1020[LeadCode.Oz].Coordinate);
            var negative = dipole.CalcPotential(EEGScheme.Scheme1020[LeadCode.Fpz].Coordinate);

            Assert.IsTrue(positive > 0);
            Assert.IsTrue(negative < 0);
            Assert.IsTrue(Math.Abs(positive).AlmostEqual(Math.Abs(negative), 5));

            foreach (var lead in new[] { LeadCode.T3, LeadCode.C3, LeadCode.Cz, LeadCode.C4, LeadCode.T4 })
            {
                var zero = dipole.CalcPotential(EEGScheme.Scheme1020[lead].Coordinate);
                Assert.IsTrue(zero.AlmostEqual(0, 5) || Math.Abs(positive / zero) > 1000);
            }
        }

        [TestMethod]
        public void Test1_CenterToVertexDipole()
        {
            var dipole = new Dipole()
            {
                Location = new PolarCoordinate(0, 0, 0),
                Moment = new PolarCoordinate(0, 90, 1)
            };

            var positive = dipole.CalcPotential(EEGScheme.Scheme1020[LeadCode.Cz].Coordinate);
            Assert.IsTrue(positive > 0);

            foreach (var lead in new[] { LeadCode.Fpz, LeadCode.Oz, LeadCode.T3, LeadCode.T4 })
            {
                var zero = dipole.CalcPotential(EEGScheme.Scheme1020[lead].Coordinate);
                Assert.IsTrue(zero.AlmostEqual(0, 5) || Math.Abs(positive / zero) > 1000);
            }
        }

        [TestMethod]
        public void Test1_Synonims()
        {
            foreach(var pair in new[] { Tuple.Create(LeadCode.T7, LeadCode.T3),
                                        Tuple.Create(LeadCode.T8, LeadCode.T4),
                                        Tuple.Create(LeadCode.P7, LeadCode.T5),
                                        Tuple.Create(LeadCode.P8, LeadCode.T6) })
            {
                var sXYZ = ScalpSphere.GetLeadXYZ(pair.Item1);
                var e3XYZ = ScalpSphere.GetLeadXYZ(pair.Item2);

                Assert.IsTrue(sXYZ.X.AlmostEqual(e3XYZ.X) &&
                              sXYZ.Y.AlmostEqual(e3XYZ.Y) &&
                              sXYZ.Z.AlmostEqual(e3XYZ.Z));
            }

            foreach (var pair in new[] { Tuple.Create("T7", "T3"),
                                         Tuple.Create("T8", "T4"),
                                         Tuple.Create("P7", "T5"),
                                         Tuple.Create("P8", "T6") })
            {
                var sXYZ = ScalpSphere.GetLeadXYZ(pair.Item1);
                var eXYZ = ScalpSphere.GetLeadXYZ(pair.Item2);

                Assert.IsTrue(sXYZ.HasValue && eXYZ.HasValue);
                Assert.IsTrue(sXYZ.Value.X.AlmostEqual(eXYZ.Value.X) &&
                              sXYZ.Value.Y.AlmostEqual(eXYZ.Value.Y) &&
                              sXYZ.Value.Z.AlmostEqual(eXYZ.Value.Z));
            }
        }

        [TestMethod]
        public void Test1_Dipole()
        {
            var dipole = new Dipole()
            {
                Location = new PolarCoordinate(20, 0, 0.9),
                Moment = new PolarCoordinate(0, 0, 1)
            };

            var leads = new[]
            {
                LeadCode.AF3,
                LeadCode.AF4,
                LeadCode.F3,
                LeadCode.F4,
                LeadCode.F7,
                LeadCode.F8,
                LeadCode.FC5,
                LeadCode.FC6,
                LeadCode.T7,
                LeadCode.T8,
                LeadCode.P7,
                LeadCode.P8,
                LeadCode.O1,
                LeadCode.O2,
            };

            var values = leads.Select(l => dipole.CalcPotential(ScalpSphere.GetLeadXYZ(l)))
                              .ToArray();
            var json = JsonSerializer.Serialize(values);

            Assert.IsFalse(string.IsNullOrEmpty(json));
        }
    }
}
