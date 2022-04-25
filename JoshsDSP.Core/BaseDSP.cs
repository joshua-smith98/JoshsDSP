using NAudio.Wave;

namespace JoshsDSP.Core
{
    public abstract class BaseDSP : ISampleProvider
    {
        protected ISampleProvider _sourceProvider;
        
        protected float[] _currentSamples;

        public WaveFormat WaveFormat => _sourceProvider.WaveFormat;

        public BaseDSP(ISampleProvider source)
        {
            _sourceProvider = source;
            _currentSamples = new float[WaveFormat.Channels];
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int numSamples = _sourceProvider.Read(buffer, offset, count);

            for (int i = offset; i < numSamples;  i += WaveFormat.Channels)
            {
                for (int channel = 0; channel < WaveFormat.Channels; channel++)
                {
                    _currentSamples[channel] = buffer[i + channel];
                }

                Process();

                for (int channel = 0; channel < WaveFormat.Channels; channel++)
                {
                    buffer[i + channel] = _currentSamples[channel];
                }
            }

            return numSamples;
        }

        protected abstract void Process();
    }
}
