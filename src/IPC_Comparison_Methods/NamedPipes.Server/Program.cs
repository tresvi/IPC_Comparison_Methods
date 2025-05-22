using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamedPipes.Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Pipe Server iniciado...");
            using (var server = new NamedPipeServerStream("MyPipe", PipeDirection.InOut, 1, PipeTransmissionMode.Message))
            { 
                await server.WaitForConnectionAsync();
                Console.WriteLine("Client connected.");

                var buffer = new byte[256];
                while (true)
                {
                    var bytesRead = await server.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break;

                    var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Recibido del cliente: " + message);

                    var response = "Mensaje recibido: " + message;
                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    await server.WriteAsync(responseBytes, 0, responseBytes.Length);
                    await server.FlushAsync();
                }
            }
            Console.WriteLine("Conexión finalizada.");
        }
    }
}
