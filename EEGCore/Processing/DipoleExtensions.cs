using EEGCore.Data;
using EEGCore.Processing.Model;
using System.Text.Json;

namespace EEGCore.Processing
{
    public static class DipoleExtensions
    {
        public static Dipole Clone(this Dipole dipole)
        {
            var clone = new Dipole()
            {
                Location = new PolarCoordinate(dipole.Location),
                Moment = new PolarCoordinate(dipole.Moment),
            };

            return clone;
        }

        public static string ToJson(this Dipole dipole)
        {
            var json = JsonSerializer.Serialize(dipole);
            return json;
        }
    }
}
