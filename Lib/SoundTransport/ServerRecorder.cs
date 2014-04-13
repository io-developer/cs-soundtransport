using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio;
using NAudio.Wave;
using NAudio.Wave.WaveFormats;
using System.IO;

namespace SoundTransport
{
    public class ServerRecorder
    {
        public ServerRecorder(IServer server, Int32 sampleRate = 48000, Int32 numChannels = 2)
        {
            this.server = server;
            this.sampleRate = sampleRate;
            this.numChannels = numChannels;
        }


        private IServer server;
        private Int32 sampleRate;
        private Int32 numChannels;
        private WaveIn waveSource;


        public IServer Server
        {
            get { return server; }
            set { server = value; }
        }

        public void Start(Int32 bufferMilliseconds = 10, Int32 numBuffers = 2 )
        {
            this.Stop();

            waveSource = new WaveIn();
            waveSource.BufferMilliseconds = bufferMilliseconds;
            waveSource.NumberOfBuffers = numBuffers;
            waveSource.WaveFormat = new WaveFormat(this.sampleRate, this.numChannels);
            waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
            waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);
            waveSource.StartRecording();
        }

        public void Stop()
        {
            if (waveSource != null)
                waveSource.StopRecording();
        }

        private void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (server == null)
                return;

            byte[] bytes = new byte[e.BytesRecorded];
            Array.Copy(e.Buffer, bytes, e.BytesRecorded);
            server.BroadcastBytes(bytes);
        }

        private void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }
        }
    }
}
