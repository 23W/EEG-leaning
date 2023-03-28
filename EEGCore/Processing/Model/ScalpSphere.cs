using EEGCore.Data;

namespace EEGCore.Processing.Model
{
    public static class ScalpSphere
    {
        public static Vector CalcXYZ(EEGCoordinate coordinate) => CalcXYZ(coordinate.Alpha, coordinate.Beta, coordinate.R);

        public static Vector CalcXYZ(double alpha, double beta, double r)
        {
            var beta1 = 90.0 - beta;
            var res = new Vector()
            {
                X = r * Math.Sin(beta1.ToRadians()) * Math.Cos(alpha.ToRadians()),
                Y = r * Math.Sin(beta1.ToRadians()) * Math.Sin(alpha.ToRadians()),
                Z = r * Math.Cos(beta1.ToRadians())
            };

            return res;
        }

        public static EEGCoordinate CalcSpherical(Vector coordinate) => CalcSpherical(coordinate.X, coordinate.Y, coordinate.Z);

        public static EEGCoordinate CalcSpherical(double x, double y, double z)
        {
            var res = new EEGCoordinate()
            {
                R = Math.Sqrt(x*x + y*y + z*z),
                Alpha = Math.Atan2(y, x).ToDegrees(),
                Beta = 90 - Math.Atan2(Math.Sqrt(x*x + y*y), z).ToDegrees()
            };

            return res;
        }

    }
}
