using System.Diagnostics;

namespace EEGCore.Processing.Model
{
    // Window function that accumlated (holds) some count of sample in internal buffer
    public class WindowFunction
    {
        // Window with
        public int Width
        {
            get => m_samples.Length; 
            init { Debug.Assert(value > 0); m_samples = new double[value]; }
        }

        // Count of samples in buffer
        public int Count => m_nextPosition <= Width ? m_nextPosition : Width;

        // Total accumulated samples count, only `Count` of them are present in buffer
        public int AccumulatedCount => m_nextPosition;

        // Unordered buffer of samples, only first `Count` are valid
        public double[] Samples => m_samples;

        // Ordered buffer of samples, this copy of samples buffer where samples are sorted in accumulation order
        public double[] OrderedSamples => GetOrderedSamples();

        // Fill whole buffer by sample value, don't change accumulated samples count
        public void Fill(double sample)
        {
            Array.Fill(m_samples, sample);
        }

        // Add (accumulate) new sample into buffer, increase accumulated samples count
        public virtual void Add(double sample)
        {
            m_samples[m_nextPosition % Width] = sample;
            m_nextPosition++;
        }

        #region Helper Methods

        double[] GetOrderedSamples()
        {
            var res = default(double[]);

            if (m_nextPosition <= Width)
            {
                res = new double[m_nextPosition];
                Array.Copy(m_samples, res, res.Length);
            }
            else
            {
                res = new double[Width];

                var borderIndex = m_nextPosition % Width;
                Array.Copy(m_samples, borderIndex, res, 0, Width - borderIndex);
                Array.Copy(m_samples, 0, res, Width - borderIndex, borderIndex);
            }

            return res;
        }

        #endregion

        #region Members

        double[] m_samples = Array.Empty<double>();
        int m_nextPosition = 0;

        #endregion
    }
}
