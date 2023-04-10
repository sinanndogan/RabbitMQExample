using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://vgoeohfm:NmkSmsxycy31BqcSuJ7Me8qFIdR0E6Y_@shrimp.rmq.cloudamqp.com/vgoeohfm");



using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//1.Adım Exchange tanımla 
channel.ExchangeDeclare(exchange: "fanout-exchange-example", type: ExchangeType.Fanout);

Console.Write("Kuyruk Adını giriniz : ");
string _queueName=Console.ReadLine();
channel.QueueDeclare(queue: _queueName, exclusive:false);


channel.QueueBind(queue: _queueName, exchange: "fanout-exchange-example",routingKey:string.Empty);


//Mesajları okuma

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: _queueName, autoAck:true, consumer:consumer);
consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

Console.Read();