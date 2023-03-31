using EEGCore.Data;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EEGCore.Processing.Model
{
    public static class ScalpSphere
    {
        public static Vector? GetLeadXYZ(string leadName)
        {
            var res = default(Vector?);

            if (Dictionary.Value.TryGetValue(leadName.ToLower(), out Vector coordinates))
            {
                res = coordinates;
            }

            return res;
        }

        public static Vector GetLeadXYZ(LeadCode leadCode) => GetLeadXYZ(leadCode.ToString())!.Value;

        public static PolarCoordinate? GetLeadSpherical(string leadName)
        {
            var res = default(PolarCoordinate?);

            var xyz = GetLeadXYZ(leadName);
            if (xyz.HasValue)
            {
                res = CalcSpherical(xyz.Value);
            }

            return res;
        }

        public static PolarCoordinate GetLeadSpherical(LeadCode leadCode) => CalcSpherical(GetLeadXYZ(leadCode));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector CalcXYZ(PolarCoordinate coordinate) => CalcXYZ(coordinate.Alpha, coordinate.Beta, coordinate.R);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector CalcXYZ(double alpha, double beta, double r)
        {
            var a = Math.SinCos(alpha.ToRadians());
            var b = Math.SinCos((90 - beta).ToRadians());
            var x = r * b.Sin * a.Cos;
            var y = r * b.Sin * a.Sin;
            var z = r * b.Cos;

            var res = new Vector(x, y, z);
            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PolarCoordinate CalcSpherical(Vector coordinate) => CalcSpherical(coordinate.X, coordinate.Y, coordinate.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PolarCoordinate CalcSpherical(double x, double y, double z)
        {
            var res = new PolarCoordinate()
            {
                R = Math.Sqrt(x*x + y*y + z*z),
                Alpha = Math.Atan2(y, x).ToDegrees(),
                Beta = 90 - Math.Atan2(Math.Sqrt(x*x + y*y), z).ToDegrees()
            };

            return res;
        }

        static Lazy<Dictionary<string, Vector>> Dictionary => new Lazy<Dictionary<string, Vector>>(() =>
        {
            var res = new Dictionary<string, Vector>();

            var assembly = Assembly.GetExecutingAssembly();
            var streamName = assembly.GetManifestResourceNames().Single(s => s.EndsWith("sphere.txt"));

            using (var resourceStream = assembly.GetManifestResourceStream(streamName)!)
            using (var reader = new StreamReader(resourceStream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line != default(string))
                    {
                        var values = line.Split('\t');
                        Debug.Assert(values.Length == 4);

                        res.Add(values[0].ToLower(),
                                new Vector() 
                                { 
                                    X = Convert.ToDouble(values[1], CultureInfo.InvariantCulture),
                                    Y = Convert.ToDouble(values[2], CultureInfo.InvariantCulture),
                                    Z = Convert.ToDouble(values[3], CultureInfo.InvariantCulture),
                                });
                    }
                }
            }

            return res;
        });
    }
}
