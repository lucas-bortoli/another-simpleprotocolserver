using CSCore;
using CSCore.SoundIn;
using CSCore.SoundOut;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.IO;
using CSCore.Streams;

namespace loopbackaudio
{
    class AudioCapture
    {
        public WaveFormat format;
        public WasapiLoopbackCapture capture;
        public WasapiOut silenceOutput;

        public NetworkServer server;

        private WriteableBufferingSource silenceSource;
        

        public AudioCapture(int port = 12345)
        {
            format = new WaveFormat(44100, 16, 2);

            silenceSource = new WriteableBufferingSource(format) { FillWithZeros = true };
            silenceOutput = new WasapiOut();
            capture = new WasapiLoopbackCapture(0, format, System.Threading.ThreadPriority.Normal);
            server = new NetworkServer(port);
            
            capture.Initialize();
            silenceOutput.Initialize(silenceSource);

            capture.DataAvailable += (s, e) =>
            {
                server.WriteAudioData(e.Data, e.Offset, e.ByteCount);
            };
        }

        public void Start()
        {
            server.Start();
            capture.Start();
            silenceOutput.Play();
        }

        public void Stop()
        {
            server.Stop();
            capture.Stop();
        }
    }
}
