using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MemoryMappedFiles.Server
{
    internal class Program
    {
        static void Main()
        {
            const string mapName = "Local\\MiMMF";
            const int size = 1024;

            using (var mmf = MemoryMappedFile.CreateOrOpen(mapName, size))
            {
                Console.WriteLine("🧠 MMF: Esperando mensajes...");

                while (true)
                {
                    using (var accessor = mmf.CreateViewAccessor())
                    {
                        byte[] buffer = new byte[size];
                        accessor.ReadArray(0, buffer, 0, size);

                        string message = Encoding.UTF8.GetString(buffer).Trim('\0');
                        if (!string.IsNullOrWhiteSpace(message))
                        {
                           // Console.WriteLine($"[MMF] Received message: {message}");

                            // limpiar la memoria compartida
                            accessor.WriteArray(0, new byte[size], 0, size);
                        }
                    }

                   // Thread.Sleep(100); // para no saturar CPU
                }
            }
        }
    }
}
