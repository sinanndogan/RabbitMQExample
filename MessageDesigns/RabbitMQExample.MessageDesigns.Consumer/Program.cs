
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://vgoeohfm:NmkSmsxycy31BqcSuJ7Me8qFIdR0E6Y_@shrimp.rmq.cloudamqp.com/vgoeohfm");

using IConnection connection = factory.CreateConnection();
using IModel channel =connection.CreateModel();

#region P2P (Point-to-Point) Tasarımı
//string queueName = "example-p2p-eueue";
//Bir kuyruğa göndermiş olduğumuz mesajları o kuyruğu dinleyen consumerler tarafından tüketilmesine P2P tasarımı diyoruz.

//channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false);

//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
//consumer.Received += (sender, e) =>
//{
//    string message = Encoding.UTF8.GetString(e.Body.Span);
//    Console.WriteLine(message);
//};
#endregion


#region Publish/Subscribe (Pub/Sub) Tasarımı
//Bu tasarım bir mesajın birçok tüketici tarafından işlenmesi gerektiği durumlarda kullanışlıdır. 

string exchangeName = "example-pub-sub-exchange";

channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

string queueName = channel.QueueDeclare().QueueName;
channel.QueueBind(
    queue: queueName,
    exchange: exchangeName,
    routingKey: string.Empty
    );

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

#endregion