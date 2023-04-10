

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://vgoeohfm:NmkSmsxycy31BqcSuJ7Me8qFIdR0E6Y_@shrimp.rmq.cloudamqp.com/vgoeohfm");



using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();


channel.ExchangeDeclare(exchange: "header-exchange-example", type: ExchangeType.Headers);

Console.Write("Lütfen header value'sunu giriniz : ");
string value =Console.ReadLine();

string queueName = channel.QueueDeclare().QueueName;


channel.QueueBind(queue: queueName, exchange: "header-exchange-example", routingKey: string.Empty, new Dictionary<string, object>
{
    ["x-match"]="any",
    ["no"]=value
});

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue:queueName, autoAck:true, consumer:consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};



Console.Read();