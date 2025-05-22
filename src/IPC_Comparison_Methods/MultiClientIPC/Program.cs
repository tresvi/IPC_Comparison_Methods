using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.IO.Pipes;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiClientIPC
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("📨 ¿Cuántos mensajes desea enviar? (Enter = 1000): ");
            string input = Console.ReadLine();
            int cantidad = string.IsNullOrWhiteSpace(input) ? 1000 : int.Parse(input);

            Console.WriteLine("📡 Elija el método de IPC:");
            Console.WriteLine("1 - Memory-Mapped File");
            Console.WriteLine("2 - Named Pipe");
            Console.WriteLine("3 - Socket TCP");

            Console.Write("👉 Ingrese una opción (1-3): ");
            string metodo = Console.ReadLine();

            var sw = Stopwatch.StartNew();

            switch (metodo)
            {
                case "1":
                    EnviarPorMemoryMapped(cantidad);
                    break;
                case "2":
                    EnviarPorNamedPipe(cantidad);
                    break;
                case "3":
                    EnviarPorSocket(cantidad);
                    break;
                default:
                    Console.WriteLine("❌ Opción no válida.");
                    return;
            }

            sw.Stop();
            Console.WriteLine($"⏱️ Total time: {sw.ElapsedMilliseconds} ms");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void EnviarPorMemoryMapped(int cantidad)
        {
            const string mapName = "Local\\MiMMF";
            const int size = 1024;

            using (var mmf = MemoryMappedFile.OpenExisting(mapName))
            {
                for (int i = 0; i < cantidad; i++)
                {
                    using (var accessor = mmf.CreateViewAccessor())
                    {
                        string mensaje = $"Message {i + 1}";
                        byte[] buffer = Encoding.UTF8.GetBytes(mensaje);

                        accessor.WriteArray(0, buffer, 0, buffer.Length);
                    }

                    // Esperar hasta que el servidor borre el contenido
                    bool enviado = false;
                    while (!enviado)
                    {
                        using (var accessor = mmf.CreateViewAccessor())
                        {
                            byte b = accessor.ReadByte(0);
                            if (b == 0)
                                enviado = true;
                        }
                        Thread.Sleep(1);
                    }
                }
            }
        }

        static void EnviarPorNamedPipe(int cantidad)
        {
            using (var pipeClient = new NamedPipeClientStream(".", "MyPipe", PipeDirection.Out))
            {
                Console.WriteLine("Connecting to server...");
                pipeClient.Connect();
                Console.WriteLine("Connected to server");

                using (var writer = new StreamWriter(pipeClient) { AutoFlush = true })
                {
                    for (int i = 0; i < cantidad; i++)
                    {
                        string mensaje = $"Mensaje {i + 1}";
                        writer.WriteLine(mensaje);
                    }
                }
            }
        }

        static void EnviarPorSocket(int cantidad)
        {
            using (var cliente = new TcpClient("127.0.0.1", 12345))
            using (NetworkStream stream = cliente.GetStream())
            {
                for (int i = 0; i < cantidad; i++)
                {
                    string mensaje = $"Mensaje {i + 1}";
                    byte[] buffer = Encoding.UTF8.GetBytes(mensaje);
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }


    }
}
