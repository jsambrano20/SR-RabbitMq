using RabbitMQ.Client;
using System.Text;

namespace Sender
{
    public class SenderClass
    {
        public void SendMessage()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            }; 
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Declarar a fila chamada "conversa"
            channel.QueueDeclare(queue: "conversa",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            string message;
            do
            {
                Console.WriteLine("Digite a mensagem a ser enviada (ou 'sair' para encerrar):");
                message = Console.ReadLine();  // Ler a entrada do usuário

                if (message.ToLower() != "sair")
                {
                    var body = Encoding.UTF8.GetBytes(message);

                    // Enviar a mensagem para a fila "conversa"
                    channel.BasicPublish(exchange: string.Empty,
                                         routingKey: "conversa", 
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine($" [x] Enviado: {message}");
                }

            } while (message.ToLower() != "sair");

            Console.WriteLine("Encerrando o programa.");
        }
    }
}
