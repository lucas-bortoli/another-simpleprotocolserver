using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace loopbackaudio
{
    class NetworkServer
    {
        private TcpListener listener;
        private TcpClient? client;

        private bool listening = false;

        public int port;

        public NetworkServer(int portNumber = 12345)
        {
            port = portNumber;
            listener = new TcpListener(System.Net.IPAddress.Loopback, port);
        }

        public async void Start()
        {
            listener.Start(1);

            listening = true;

            Console.WriteLine("Servidor iniciado");

            while (listening)
            {
                TcpClient _client = await listener.AcceptTcpClientAsync();
                _client.NoDelay = true;

                if (client != null)
                {
                    client.GetStream().Close();
                    client.Dispose();
                    client = null;
                }

                client = _client;

                Console.WriteLine("Cliente conectado");
            }
        }

        public void Stop()
        {
            listening = false;

            if (client != null)
            {
                client.GetStream().Close();
                client.Dispose();
                client = null;
            }

            listener.Stop();
        }

        public async void WriteAudioData(byte[] data, int offset, int byteCount) { 
            if (listening && client != null && client.GetStream().CanWrite)
            {
                try
                {
                    client.GetStream().Write(data, offset, byteCount);
                } catch (System.IO.IOException ex)
                {
                    Console.WriteLine("Erro ao enviar áudio ao cliente: " + ex.ToString());
                    client.Dispose();
                    client = null;
                }
            }
        }
    }
}
