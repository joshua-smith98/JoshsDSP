using NAudio.Wave;

namespace JoshsDSP.Core
{
    public class TestDSP : BaseDSP
    {
        public TestDSP(ISampleProvider source) : base(source) { }
        
        protected override void Process()
        {
            for (int i = 0; i < _currentSamples.Length; i++) _currentSamples[i] *= ((float)(Math.Sin(DateTime.Now.Ticks * 0.1f)) / 2) + 0.5f;
        }
    }
}
