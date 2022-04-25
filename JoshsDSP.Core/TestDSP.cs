using NAudio.Wave;

namespace JoshsDSP.Core
{
    public class TestDSP : BaseDSP
    {
        protected override void Process()
        {
            double multiplier = (Math.Sin((double)(DateTime.Now.Ticks) * 0.0000001) / 2) + 0.5f;

            //Console.WriteLine(multiplier);

            for (int i = 0; i < _currentSamples.Length; i++) _currentSamples[i] *= (float)multiplier;
        }
    }
}
