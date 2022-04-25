using NAudio.Wave;

namespace JoshsDSP.Core
{
    public abstract class BaseDSP : ISampleProvider
    {
        protected long _samplesCounter = 0;
        
        protected ISampleProvider? _sourceProvider;
        
        protected float[]? _currentSamples;

        public WaveFormat WaveFormat => _sourceProvider is not null ? _sourceProvider.WaveFormat : null; //Visual Studio complains here that _sourceProvider might be null - very unlikely so imma just leave that.

        protected BaseDSP() { }

        public void Init(ISampleProvider source)
        {
            _sourceProvider = source;
            _currentSamples = new float[WaveFormat.Channels];
        }

        public int Read(float[] buffer, int offset, int count)
        {
            //...but we do want to do a null check here, just in case.
            if (_sourceProvider is null) return 0; //Instinct is to return -1, but if we return 0 then anything reading will just think this is empty; which is far better than the flurry of errors that returning -1 is bound to set off.
            
            int numSamples = _sourceProvider.Read(buffer, offset, count);

            for (int i = offset; i < numSamples;  i += WaveFormat.Channels)
            {
                _samplesCounter++;
                
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
