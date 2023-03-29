using EEGCore.Data;
using MathNet.Numerics;
using System.Diagnostics;

namespace EEGCore.Processing.Model
{
    public class Dipole
    {
        public EEGCoordinate Location { get; set; } = new EEGCoordinate();
        public EEGCoordinate Moment { get; set; } = new EEGCoordinate();
        public double Epsilon => 10;

        // V(r) = (1/4πε) * [p · r / |r|^3]
        // p - dipole moment
        // r - direction vector from dipole center to potential point
        // ε - brain dielectric constant (10-50)
        public double CalcPotential(EEGCoordinate scalpLocation)
        {
            Debug.Assert(scalpLocation.R.AlmostEqual(1, 5));

            var factor = 1 / (4 * Math.PI * Epsilon);

            var scalpXYZ = ScalpSphere.CalcXYZ(scalpLocation).Sub(ScalpSphere.CalcXYZ(Location));
            var scalpLength = scalpXYZ.Length();
            var momentXYZ = ScalpSphere.CalcXYZ(Moment);

            var res = factor * momentXYZ.Dot(scalpXYZ) / Math.Pow(scalpLength, 3);
            return res;
        }
    }
}
