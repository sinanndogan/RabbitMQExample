
using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri= new("amqps://vgoeohfm:NmkSmsxycy31BqcSuJ7Me8qFIdR0E6Y_@shrimp.rmq.cloudamqp.com/vgoeohfm");



using IConnection connection = factory.CreateConnection();
using IModel channel =connection.CreateModel();


channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

while (true)
{
    Console.Write("Mesaj : ");
    string message = Console.ReadLine();
    byte[] byteMessage = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "direct-exchange-example",routingKey:"direct-queue-example",body:byteMessage);
}
Console.Read();


//Biz burda herhangi bir kuyruk oluşturmadık burda exchange type oluşturduk ve onun üzerinden consumer tarafında oluşturulacak olan kuyruk ismine gideceğini routingkey ile belirtmiş olduk 