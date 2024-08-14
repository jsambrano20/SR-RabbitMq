using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Receiver
{
    public class ReceiverClass
    {
        public void ReceiveMessage()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "conversa",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            Console.WriteLine(" [*] Aguardando mensagens.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Recebido: {message}");
            };

            // Consumir mensagens da fila "conversa"
            channel.BasicConsume(queue: "conversa",
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Pressione [Ctrl+C] para sair.");
            while (true)
            {
                // Isso mantém o programa ativo indefinidamente.
                // O loop só terminará quando o programa for interrompido manualmente (por exemplo, com Ctrl+C).
                Thread.Sleep(1000);  // Sleep para evitar consumo excessivo de CPU
            }
        }
    }
}
