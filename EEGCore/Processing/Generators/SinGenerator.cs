namespace EEGCore.Processing.Generators
{
    public class SinGenerator : ISampleGenerator
    {
        public double Amplitude { get; set; } = 1;

        public double Mean { get; set; } = 0;

        public double Frequence { get; set; } = 1;

        public double SampleRate { get; set; } = 128;

        public void Generate(double[] samples, bool addGenerated = false)
        {
            var sequence = MathNet.Numerics.Generate.SinusoidalSequence(SampleRate, Frequence, Amplitude, Mean);
            using (var sequenceEnum = sequence.GetEnumerator())
            {
                if (addGenerated)
                {
                    foreach (ref var sample in samples.AsSpan())
                    {
                        sequenceEnum.MoveNext();

                        var generated = sequenceEnum.Current;
                        sample += generated;
                    }
                }
                else
                {
                    foreach (ref var sample in samples.AsSpan())
                    {
                        sequenceEnum.MoveNext();

                        var generated = sequenceEnum.Current;
                        sample = generated;
                    }
                }
            }
        }
    }
}
