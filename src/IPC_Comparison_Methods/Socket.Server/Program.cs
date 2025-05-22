using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Socket.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int puerto = 12345;

            TcpListener servidor = new TcpListener(IPAddress.Loopback, puerto);
            servidor.Start();

            Console.WriteLine("🌐 Socket TCP: Esperando conexión en el puerto 12345...");

            using (TcpClient cliente = servidor.AcceptTcpClient())
            using (NetworkStream stream = cliente.GetStream())
            {
                Console.WriteLine("📡 Cliente conectado. Esperando mensajes...");
                byte[] buffer = new byte[1024];

                int bytesLeidos;
                while ((bytesLeidos = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string mensaje = Encoding.UTF8.GetString(buffer, 0, bytesLeidos);
                    Console.WriteLine($"[Socket] Mensaje recibido: {mensaje}");
                }
            }

            servidor.Stop();
        }
    }
}
