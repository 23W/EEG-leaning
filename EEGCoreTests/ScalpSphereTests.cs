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
            var xyzFpz = ScalpSphere.CalcXYZ(EEGScheme.Scheme1020[LeadCode.Fpz].Coordinate);
            Assert.IsTrue(xyzFpz.X.AlmostEqual(1) &&
                          xyzFpz.Y.AlmostEqual(0) &&
                          xyzFpz.Z.AlmostEqual(0));

            var xyzOz = ScalpSphere.CalcXYZ(EEGScheme.Scheme1020[LeadCode.Oz].Coordinate);
            Assert.IsTrue(xyzOz.X.AlmostEqual(-1) &&
                          xyzOz.Y.AlmostEqual(0) &&
                          xyzOz.Z.AlmostEqual(0));

            var xyzT3 = ScalpSphere.CalcXYZ(EEGScheme.Scheme1020[LeadCode.T3].Coordinate);
            Assert.IsTrue(xyzT3.X.AlmostEqual(0) &&
                          xyzT3.Y.AlmostEqual(1) &&
                          xyzT3.Z.AlmostEqual(0));

            var xyzT4 = ScalpSphere.CalcXYZ(EEGScheme.Scheme1020[LeadCode.T4].Coordinate);
            Assert.IsTrue(xyzT4.X.AlmostEqual(0) &&
                          xyzT4.Y.AlmostEqual(-1) &&
                          xyzT4.Z.AlmostEqual(0));

            var xyzCz = ScalpSphere.CalcXYZ(EEGScheme.Scheme1020[LeadCode.Cz].Coordinate);
            Assert.IsTrue(xyzCz.X.AlmostEqual(0) &&
                          xyzCz.Y.AlmostEqual(0) &&
                          xyzCz.Z.AlmostEqual(1));
        }

        [TestMethod]
        public void Test1_Spherical()
        {
            foreach (var lead in Enum.GetValues(typeof(LeadCode)).Cast<LeadCode>())
            {
                var sCoordinate = EEGScheme.Scheme1020[lead].Coordinate;
                var eCoordinate = ScalpSphere.CalcSpherical(ScalpSphere.CalcXYZ(sCoordinate));

                Assert.IsTrue(eCoordinate.Alpha.AlmostEqual(sCoordinate.Alpha) &&
                              eCoordinate.Beta.AlmostEqual(sCoordinate.Beta) &&
                              eCoordinate.R.AlmostEqual(sCoordinate.R));
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
            Assert.IsTrue(Math.Abs(positive).AlmostEqual(Math.Abs(negative)));

            var zero = dipole.CalcPotential(EEGScheme.Scheme1020[LeadCode.Cz].Coordinate);
            Assert.IsTrue(zero.AlmostEqual(0));
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
            Assert.IsTrue(Math.Abs(positive).AlmostEqual(Math.Abs(negative)));

            var zero = dipole.CalcPotential(EEGScheme.Scheme1020[LeadCode.Cz].Coordinate);
            Assert.IsTrue(zero.AlmostEqual(0));
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
                Assert.IsTrue(zero.AlmostEqual(0));
            }

            var positive = dipole.CalcPotential(EEGScheme.Scheme1020[LeadCode.Cz].Coordinate);
            var negative = dipole.CalcPotential(EEGScheme.Scheme1020[LeadCode.Cz].Coordinate);
            Assert.IsTrue(positive > 0);
        }
    }
}
