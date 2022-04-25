using System.IO;

using NAudio.Wave;

namespace JoshsDSP.Core
{
    public static class GlobalDSP
    {
        static WaveOut? _outputDevice;
        static AudioFileReader? _audioFileReader;

        public static bool IsPlaying => _outputDevice is not null ? _outputDevice.PlaybackState == PlaybackState.Playing : false;
        public static float Position
        {
            get => _audioFileReader is not null ? (float)_audioFileReader.Position / _audioFileReader.Length : 0;
            set
            {
                if (_audioFileReader is not null)
                    _audioFileReader.Position = (long)(value * _audioFileReader.Length);
            }
        }

        public static bool PlayFile(string dir)
        {
            if (!File.Exists(dir)) return false;

            _audioFileReader?.Dispose();
            _audioFileReader = new AudioFileReader(dir);
            _outputDevice?.Dispose();
            _outputDevice = new WaveOut();
            _outputDevice.Init(_audioFileReader);
            _outputDevice.Play();

            return true;
        }

        public static bool Stop()
        {
            if (_outputDevice is null || _outputDevice.PlaybackState == PlaybackState.Stopped) return false;
            if (_audioFileReader is null) return false;

            _outputDevice.Stop();

            return true;
        }
    }
}
