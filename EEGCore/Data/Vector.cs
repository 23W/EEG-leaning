using MathNet.Numerics.LinearAlgebra.Double;
using System.Diagnostics;

namespace EEGCore.Data
{
    public struct Vector
    {
        public double X { get => m_x; set => m_x = value; }
        public double Y { get => m_y; set => m_y = value; }
        public double Z { get => m_z; set => m_z = value; }

        public Vector()
        {
            m_x = 0;
            m_y = 0;
            m_z = 0;
        }

        public Vector(double x, double y, double z)
        {
            m_x = x;
            m_y = y;
            m_z = z;
        }

        public Vector Add(Vector v)
        {
            var res = new Vector(m_x + v.m_x, m_y + v.m_y, m_z + v.m_z);
            return res;
        }

        public Vector Sub(Vector v)
        {
            var res = new Vector(m_x - v.m_x, m_y - v.m_y, m_z - v.m_z);
            return res;
        }

        public double Dot(Vector v)
        {
            var res = m_x * v.m_x + m_y * v.m_y + m_z * v.m_z;
            return res;
        }

        public double Length()
        {
            var res = Math.Sqrt(m_x * m_x + m_y * m_y + m_z * m_z);
            return res;
        }

        double m_x;
        double m_y;
        double m_z;
    }
}
