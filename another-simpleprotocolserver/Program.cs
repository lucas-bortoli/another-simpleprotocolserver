using System;

namespace loopbackaudio
{
    class Program
    {
        static void Main(string[] args)
        {
            AudioCapture capture = new AudioCapture();

            AppDomain.CurrentDomain.ProcessExit += (object sender, EventArgs e) =>
            {
                capture.Stop();
                capture.capture.Dispose();
            };

            capture.Start();
        }
    }
}
