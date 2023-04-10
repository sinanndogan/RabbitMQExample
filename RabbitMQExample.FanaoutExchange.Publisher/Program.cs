using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://vgoeohfm:NmkSmsxycy31BqcSuJ7Me8qFIdR0E6Y_@shrimp.rmq.cloudamqp.com/vgoeohfm");



using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//Mesajların bu exchange'e bind olmuş olan tüm kuyruklara gönderilmesini sağlar.Publisher mesajların gönderildiği kuyruk isimlerini dikkate almaz ve mesajları tüm kuyruklara gönderir
channel.ExchangeDeclare(exchange: "fanout-exchange-example", type: ExchangeType.Fanout);

//100 adet mesaj yayınlamak istiyorum 
for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba{i}");
    channel.BasicPublish(exchange: "fanout-exchange-example", 
    routingKey:string.Empty,
    body:message);
}

//routingkey : "" boş bir değer ya da string.Empty olabilir.