using EEGCore.Data;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace EEGCore.Processing.Model
{
    public static class ScalpSphere
    {
        public static Vector? GetLeadXYZ(string leadName)
        {
            var res = default(Vector?);
            Dictionary.Value.TryGetValue(leadName.ToLower(), out res);

            return res;
        }

        public static Vector GetLeadXYZ(LeadCode leadCode)
        {
            var res = GetLeadXYZ(leadCode.ToString())!;
            return res;
        }

        public static EEGCoordinate? GetLeadSpherical(string leadName)
        {
            var res = default(EEGCoordinate?);

            var xyz = GetLeadXYZ(leadName);
            if (xyz != default)
            {
                res = CalcSpherical(xyz);
            }

            return res;
        }

        public static EEGCoordinate GetLeadSpherical(LeadCode leadCode)
        {
            var res = CalcSpherical(GetLeadXYZ(leadCode));
            return res;
        }

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
