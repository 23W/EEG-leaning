namespace EEGCore.Data
{
    // Polar (spherical) coordinates in degrees
    public struct PolarCoordinate
    {
        public double Alpha { get => m_a; set => m_a = value; }
        public double Beta { get => m_b; set => m_b = value; }
        public double R { get => m_r; set => m_r = value; }

        public PolarCoordinate()
        {
            m_a = 0;
            m_b = 0;
            m_r = 1;
        }

        public PolarCoordinate(double alpha, double beta, double r)
        {
            m_a = alpha;
            m_b = beta;
            m_r = r;
        }

        public PolarCoordinate(PolarCoordinate coordinate)
        {
            m_a = coordinate.m_a;
            m_b = coordinate.m_b;
            m_r = coordinate.m_r;
        }

        double m_a;
        double m_b;
        double m_r;
    }
}
