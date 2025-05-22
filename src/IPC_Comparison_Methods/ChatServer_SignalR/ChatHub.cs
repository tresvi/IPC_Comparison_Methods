using Microsoft.AspNetCore.SignalR;

namespace ChatServer_SignalR
{
    public class ChatHub : Hub
    {
        public async Task EnviarMensaje(string usuario, string mensaje)
        {
           // Console.WriteLine($"[{usuario}] {mensaje}"); //Mensaje de entrada
         //   await Clients.All.SendAsync("RecibirMensaje", usuario, mensaje);
        }
    }
}
