using MathNet.Numerics.LinearAlgebra.Double;

namespace EEGCore.Data
{
    public class Vector
    {
        public double X { get => Data[0]; set => Data[0] = value; }
        public double Y { get => Data[1]; set => Data[1] = value; }
        public double Z { get => Data[2]; set => Data[2] = value; }
        public double[] Array { get => Data.ToArray(); set { if (value.Length < 3) { throw new ArgumentException(); } Data = new DenseVector(value); } }

        internal DenseVector Data { get; set; } = new DenseVector(3);

        public Vector Add(Vector v)
        {
            var res = new Vector() { Data = new DenseVector(Data.Add(v.Data).AsArray()) };
            return res;
        }

        public Vector Sub(Vector v)
        {
            var res = new Vector() { Data = new DenseVector(Data.Subtract(v.Data).AsArray()) };
            return res;
        }

        public double Dot(Vector v)
        {
            var res = Data.DotProduct(v.Data);
            return res;
        }

        public double Length()
        {
            var res = Data.L2Norm();
            return res;
        }
    }
}
