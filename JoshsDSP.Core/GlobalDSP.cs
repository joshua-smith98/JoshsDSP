using System.IO;

using NAudio.Wave;

namespace JoshsDSP.Core
{
    public static class GlobalDSP
    {
        static DirectSoundOut _outputDevice;
        static AudioFileReader _audioFileReader;

        public static bool PlayFile(string dir)
        {
            if (!File.Exists(dir)) return false;

            _audioFileReader?.Dispose();
            _audioFileReader = new AudioFileReader(dir);
            _outputDevice?.Dispose();
            _outputDevice = new DirectSoundOut();
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
