namespace ChatServer_SignalR
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.UseUrls("http://localhost:12345"); //puerto personalizado
            builder.Services.AddSignalR();

            var app = builder.Build();
            app.MapHub<ChatHub>("/chat");
            app.Run();
        }
    }
}
