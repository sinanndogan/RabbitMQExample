using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://vgoeohfm:NmkSmsxycy31BqcSuJ7Me8qFIdR0E6Y_@shrimp.rmq.cloudamqp.com/vgoeohfm");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();


#region P2P (Point-to-Point) Tasarımı
//Bir kuyruğa göndermiş olduğumuz mesajları o kuyruğu dinleyen consumerler tarafından tüketilmesine P2P tasarımı diyoruz.


//string queueName = "example-p2p-eueue";

//channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false);

//byte[] message = Encoding.UTF8.GetBytes("Merhaba");

//channel.BasicPublish(exchange: string.Empty, routingKey: queueName, body: message);
#endregion

#region Publish/Subscribe (Pub/Sub) Tasarımı
//Bu tasarım bir mesajın birçok tüketici tarafından işlenmesi gerektiği durumlarda kullanışlıdır.

string exchangeName = "example-pub-sub-exchange";

channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

for (int i = 0; i < 10; i++)
{
   await Task.Delay(200);
byte[] message = Encoding.UTF8.GetBytes("merhba");
channel.BasicPublish(exchange: exchangeName, routingKey: string.Empty, body: message);
}


#endregion