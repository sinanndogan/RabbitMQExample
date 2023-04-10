

using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://vgoeohfm:NmkSmsxycy31BqcSuJ7Me8qFIdR0E6Y_@shrimp.rmq.cloudamqp.com/vgoeohfm");



using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();


channel.ExchangeDeclare(exchange: "header-exchange-example", type: ExchangeType.Headers);


for (int i = 0; i < 80; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
    Console.Write("Lütfen header value'sunu giriniz : ");
    string value=Console.ReadLine();

    IBasicProperties properties =channel.CreateBasicProperties();
    
    properties.Headers = new Dictionary<string, object>
    {
        ["no"] = value,
        ["yes"] =value
    };

    channel.BasicPublish(exchange: "header-exchange-example", routingKey: string.Empty, body: message, basicProperties: properties);
}

