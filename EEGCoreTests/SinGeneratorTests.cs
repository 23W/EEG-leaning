using EEGCore.Processing.Generators;
using MathNet.Numerics.LinearAlgebra.Double;

namespace EEGCoreTests
{
    [TestClass]
    public class SinGeneratorTests
    {
        [TestMethod]
        public void Sinusoidal()
        {
            var period = (int)Math.Floor(Math.PI * 2 * 1000);
            var length = period + 1;
            var samples = new double[length];

            var generator = new SinGenerator() { Amplitude = 200, Frequence = 1, SampleRate = period };
            generator.Generate(samples, false);

            Assert.AreEqual(Math.Abs(samples[0]), 0.0, 0.001);
            Assert.AreEqual(Math.Abs(samples[length - 1]), 0.0, 0.001);

            var vector = new DenseVector(samples);
            var maxIndex = vector.MaximumIndex();
            var minIndex = vector.MinimumIndex();

            Assert.AreEqual(maxIndex, length / 4, 1);
            Assert.AreEqual(minIndex, (length * 3) / 4, 1);
            Assert.AreEqual(samples[maxIndex], -samples[minIndex], 0.001);
        }

        [TestMethod]
        public void SinusoidalAdd()
        {
            var period = (int)Math.Floor(Math.PI * 2 * 1000);
            var length = period + 1;
            var mean = 10.0;
            var samples = new double[length];
            Array.Fill(samples, mean);

            var generator = new SinGenerator() { Amplitude = 200, Frequence = 1, SampleRate = period };
            generator.Generate(samples, true);

            Assert.AreEqual(Math.Abs(samples[0]), mean, 0.001);
            Assert.AreEqual(Math.Abs(samples[length - 1]), mean, 0.001);

            var vector = new DenseVector(samples);
            var maxIndex = vector.MaximumIndex();
            var minIndex = vector.MinimumIndex();

            Assert.AreEqual(maxIndex, length / 4, 1);
            Assert.AreEqual(minIndex, (length * 3) / 4, 1);
            Assert.AreEqual(samples[maxIndex] - mean, mean - samples[minIndex], 0.001);
        }
    }
}
