using EEGCore.Data;
using System.Runtime.CompilerServices;

namespace EEGCore.Processing.Model
{
    public class DipoleResult
    {
        public ComponentLead Lead { get; set; } = new ComponentLead();

        public Dipole Dipole { get; set; } = new Dipole();

        public Vector[] WeightLocations { get; set; } = Array.Empty<Vector>();

        public double[] ComponentWeights { get; set; } = Array.Empty<double>();

        public double[] ModelWeights { get; set; } = Array.Empty<double>();

        public double Correlation { get; set; } = 0;

        public double Nonconformance { get; set; } = 0;
    }

    public class Dipole
    {
        public PolarCoordinate Location { get; set; } = new PolarCoordinate();
        public PolarCoordinate Moment { get; set; } = new PolarCoordinate();
        public double Epsilon => m_epsilon;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double CalcPotential(PolarCoordinate scalpLocation) => CalcPotential(ScalpSphere.CalcXYZ(scalpLocation));

        // V(r) = (1/4πε) * [p · r / |r|^3]
        // p - dipole moment
        // r - direction vector from dipole center to potential point
        // ε - brain dielectric constant (10-50)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public double CalcPotential(Vector scalpLocation)
        {
            var scalpXYZ = scalpLocation.Sub(ScalpSphere.CalcXYZ(Location));
            var scalpLength = scalpXYZ.Length();
            var momentXYZ = ScalpSphere.CalcXYZ(Moment);

            var res = m_factor * momentXYZ.Dot(scalpXYZ) / Math.Pow(scalpLength, 3);
            return res;
        }

        const double m_epsilon =  0.3 /*10*/;
        const double m_factor = 1 / (4 * Math.PI * m_epsilon);
    }
}
