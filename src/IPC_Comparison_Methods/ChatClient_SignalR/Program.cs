using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;

//Reusltados preliminares: Socket Puro: 0.0055ms, SignlR: 0.7 ms

namespace ChatClient_SignalR
{
    internal class Program
    {
        private const int NMBR_MESSAGES_TO_SEND = 10000;

        static async Task Main()
        {
            Console.Write("Tu nombre: ");
            var usuario = Console.ReadLine();

            var conexion = new HubConnectionBuilder()
                .WithUrl("http://localhost:12345/chat")
                .WithAutomaticReconnect()
                .Build();

            conexion.On<string, string>("RecibirMensaje", (user, message) =>
            {
                // Console.WriteLine($"{user}: {message}");
                try
                {
                    Console.WriteLine($"{user}: {message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error procesando mensaje: {ex.Message}");
                }
            });

            conexion.Closed += async (error) =>
            {
                Console.WriteLine("Conexión cerrada.");
            };

            conexion.Reconnected += connectionId =>
            {
                Console.WriteLine("Reconectado al servidor.");
                return Task.CompletedTask;
            };

            conexion.Reconnecting += error =>
            {
                Console.WriteLine("Intentando reconectar...");
                return Task.CompletedTask;
            };


            try
            {
                await conexion.StartAsync();
                Console.WriteLine("Conectado al servidor SignalR.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar: {ex.Message}");
                return;
            }

            while (true)
            {
                var mensaje = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(mensaje)) continue;

                try
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    for (int i = 0; i < NMBR_MESSAGES_TO_SEND; i++)
                        await conexion.InvokeAsync("EnviarMensaje", usuario, mensaje);
                    sw.Stop();
                    Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms");
                    Console.WriteLine($"Total messages sended: {NMBR_MESSAGES_TO_SEND}");
                    Console.WriteLine($"Average duration of message: {sw.ElapsedMilliseconds / (float)NMBR_MESSAGES_TO_SEND} ms");

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error enviando mensaje: {ex.Message}");
                }
            }
        }
    }
}
