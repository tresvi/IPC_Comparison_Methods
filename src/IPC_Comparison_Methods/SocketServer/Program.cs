using System.Net.Sockets;
using System.Net;
using System.Text;

namespace SocketServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var listener = new TcpListener(IPAddress.Any, 12345);
            listener.Start();
            Console.WriteLine("Servidor esperando conexión...");

            using (TcpClient client = listener.AcceptTcpClient())
            using (NetworkStream stream = client.GetStream())
            using (StreamReader reader = new StreamReader(stream))
            using (StreamWriter writer = new StreamWriter(stream) { AutoFlush = true })
            {
                Console.WriteLine("Cliente conectado. ¡Puedes comenzar a chatear!");

                // Hilo para recibir mensajes
                Thread recibir = new Thread(() =>
                {
                    string mensaje;
                    while ((mensaje = reader.ReadLine()) != null)
                    {
                       // Console.WriteLine("Cliente: " + mensaje);
                    }
                });
                recibir.Start();

                // Hilo principal para enviar mensajes
                string entrada;
                while ((entrada = Console.ReadLine()) != null)
                {
                    writer.WriteLine(entrada);
                }
            }

            listener.Stop();
        }
    }
}
