using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Diagnostics;

namespace SocketClient
{
    internal class Program
    {
        private const int NMBR_MESSAGES_TO_SEND = 1000000;
        static void Main(string[] args)
        {
            using (TcpClient client = new TcpClient("127.0.0.1", 12345))
            using (NetworkStream stream = client.GetStream())
            using (StreamReader reader = new StreamReader(stream))
            using (StreamWriter writer = new StreamWriter(stream) { AutoFlush = true })
            {
                Console.WriteLine("Conectado al servidor. ¡Puedes comenzar a chatear!");

                // Hilo para recibir mensajes
                Thread recibir = new Thread(() =>
                {
                    string mensaje;
                    while ((mensaje = reader.ReadLine()) != null)
                    {
                        Console.WriteLine("Servidor: " + mensaje);
                    }
                });
                recibir.Start();

                // Hilo principal para enviar mensajes
                string entrada;
                while ((entrada = Console.ReadLine()) != null)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    for (int i= 0; i < NMBR_MESSAGES_TO_SEND; i++)
                        writer.WriteLine(entrada);
                    sw.Stop();
                    Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms");
                    Console.WriteLine($"Total messages sended: {NMBR_MESSAGES_TO_SEND}");
                    Console.WriteLine($"Average duration of message: {sw.ElapsedMilliseconds/ (float)NMBR_MESSAGES_TO_SEND} ms");
                }
            }
        }
    }
}
