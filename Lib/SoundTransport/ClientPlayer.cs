using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio;
using NAudio.Wave;
using NAudio.Wave.WaveFormats;

namespace SoundTransport
{
    public class ClientPlayer
    {
        public ClientPlayer(IClient client, Int32 sampleRate = 48000, Int32 numChannels = 2)
        {
            this.client = client;
            client.OnBytes += new EventHandler<ByteEventArgs>(onBytes);

            bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(sampleRate, numChannels));
            bufferedWaveProvider.BufferLength = 4 * 1024 * 1024;
            wavePlayer = new DirectSoundOut();
            wavePlayer.Init(bufferedWaveProvider);
            wavePlayer.Play();
        }


        private IClient client;
        private DirectSoundOut wavePlayer;
        private BufferedWaveProvider bufferedWaveProvider;


        public void Stop()
        {
            client.OnBytes -= new EventHandler<ByteEventArgs>(onBytes);
            wavePlayer.Stop();
        }

        private void onBytes(object sender, ByteEventArgs e)
        {
            bufferedWaveProvider.AddSamples(e.Bytes, 0, e.Bytes.Length);
        }
    }
}
