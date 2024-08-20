using System.Text;
using System.Threading.Channels;
using RabbitMQ.Client;

ConnectionFactory factory = new();
factory.Uri = new("amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "Rabbit Sender App";

IConnection connection = factory.CreateConnection();

IModel channel = connection.CreateModel();

string exchangeName = "DemoExchange";
string routingKey = "demo-routing-key";
string queueName = "DemoQueue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, exchangeName, routingKey, null);

for (int i = 0; i < 69; i++)
{
    Console.WriteLine($"Sending message #{i}");
    byte[] messageBodyBytes = Encoding.UTF8.GetBytes($"message #{i}");
    channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);
    Thread.Sleep(1000);
}

channel.Close();
connection.Close();