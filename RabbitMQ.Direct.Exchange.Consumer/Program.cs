
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://localhost");



using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//1.Adım
channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

//2.Adım
string queueName=channel.QueueDeclare().QueueName;

//3.Adım
channel.QueueBind(queue:queueName, exchange: "direct-exchange-example", routingKey: "direct-queue-example");

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

Console.Read();


//1.Adım : Publisher'da ki exchange ile birebir aynı isim ve type'a sahip sahip bir exhange tanımlanmalıdır!

//2.Adım : Publisher tarafından routing key'de bulunan değerdeki kuyruğa gönderlien mesajları kendi oluşturduğumuz kuyruğa yönlendirerek tüketmemiz gerekmektedir. Bunun için öncellikle bir kuyruk oluşturulmalıdır.


//3.Adım : 
