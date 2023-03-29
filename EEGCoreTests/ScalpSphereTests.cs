using EEGCore.Data;
using EEGCore.Processing.Model;
using MathNet.Numerics;

namespace EEGCoreTests
{
    [TestClass]
    public class ScalpSphereTests
    {
        [TestMethod]
        public void Test1_XYZ()
        {
            var leads = Enum.GetValues(typeof(LeadCode))
                            .Cast<LeadCode>()
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
                Location = new EEGCoordinate() { Alpha = 0, Beta = 0, R = 0 },
                Moment = new EEGCoordinate() { Alpha = 0, Beta = 0, R = 1 }
            };

            var positive = dipole.CalcPotential(EEGScheme.Scheme1020[LeadCode.Fpz].Coordinate);
            var negative = dipole.CalcPotential(EEGScheme.Scheme1020[LeadCode.Oz].Coordinate);

            Assert.IsTrue(positive > 0);
            Assert.IsTrue(negative < 0);
            Assert.IsTrue(Math.Abs(positive).AlmostEqual(Math.Abs(negative), 5));

            var zero = dipole.CalcPotential(EEGScheme.Scheme1020[LeadCode.Cz].Coordinate);
            Assert.IsTrue(zero.AlmostEqual(0, 5));
        }

        [TestMethod]
        public void Test1_CenterToInionDipole()
        {
            var dipole = new Dipole()
            {
                Location = new EEGCoordinate() { Alpha = 0, Beta = 0, R = 0 },
                Moment = new EEGCoordinate() { Alpha = 180, Beta = 0, R = 1 }
            };

            var positive = dipole.CalcPotential(EEGScheme.Scheme1020[LeadCode.Oz].Coordinate);
            var negative = dipole.CalcPotential(EEGScheme.Scheme1020[LeadCode.Fpz].Coordinate);

            Assert.IsTrue(positive > 0);
            Assert.IsTrue(negative < 0);
            Assert.IsTrue(Math.Abs(positive).AlmostEqual(Math.Abs(negative), 5));

            var zero = dipole.CalcPotential(EEGScheme.Scheme1020[LeadCode.Cz].Coordinate);
            Assert.IsTrue(zero.AlmostEqual(0, 5));
        }

        [TestMethod]
        public void Test1_CenterToVertexDipole()
        {
            var dipole = new Dipole()
            {
                Location = new EEGCoordinate() { Alpha = 0, Beta = 0, R = 0 },
                Moment = new EEGCoordinate() { Alpha = 0, Beta = 90, R = 1 }
            };

            foreach (var lead in new[] { LeadCode.Fpz, LeadCode.Oz, LeadCode.T3, LeadCode.T4 })
            {
                var zero = dipole.CalcPotential(EEGScheme.Scheme1020[lead].Coordinate);
                Assert.IsTrue(zero.AlmostEqual(0, 5));
            }

            var positive = dipole.CalcPotential(EEGScheme.Scheme1020[LeadCode.Cz].Coordinate);
            Assert.IsTrue(positive > 0);
        }
    }
}
