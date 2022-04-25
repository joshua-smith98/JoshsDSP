using NAudio.Wave;

namespace JoshsDSP.Core
{
    public class TestDSP : BaseDSP
    {
        protected override void Process()
        {
            double multiplier = (Math.Sin(_samplesCounter * 0.0005) / 2) + 0.5f;

            for (int i = 0; i < _currentSamples.Length; i++) _currentSamples[i] *= (float)multiplier;
        }
    }
}
