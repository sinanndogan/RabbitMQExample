using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("localhost");

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

//string exchangeName = "example-pub-sub-exchange";

//channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

//for (int i = 0; i < 10; i++)
//{
//   await Task.Delay(200);
//byte[] message = Encoding.UTF8.GetBytes("merhba");
//channel.BasicPublish(exchange: exchangeName, routingKey: string.Empty, body: message);
//}


#endregion



#region Work Queue(İş Kuyruğu) Tasarımı
//Publisher tarafından yayınlanmış bir mesajın birden fazla consumer arsından yalnızca biri tarafınan tüketilmesi amaçlanmaktadır.Böylece mesajların işlenmesi sürecinde tüm consumer'lar aynı iş yüküne ve eşit görev dağılımına sahip olacaktırlar. 

//string queueName = "example-work-queue";

//channel.QueueDeclare(queue: queueName,
//    durable: false,
//    exclusive: false,
//    autoDelete: false);

//for (int i = 0; i < 100; i++)
//{
//    await Task.Delay(200);
//    byte[] message =Encoding.UTF8.GetBytes("Merhaba"+i);
//    channel.BasicPublish(exchange: string.Empty, routingKey: queueName, body: message);
//}
#endregion


#region Request/Response Tasarımı

string requestQueueName = "example-request-response-queue";

channel.QueueDeclare(queue: requestQueueName, durable: false, exclusive: false, autoDelete: false);

//geri dönüş kuyruğu
string responseQueueName = channel.QueueDeclare().QueueName;


//corelationıd olusturma

string correlationId=Guid.NewGuid().ToString();

#region Request mesajını oluşturma ve gönderme
IBasicProperties properties = channel.CreateBasicProperties();
properties.CorrelationId = correlationId;
properties.ReplyTo = requestQueueName;
//replyto = bizim  publisher'e dönüş yolundaki kuyruk tanımlandı

byte[] message = Encoding.UTF8.GetBytes("Deneme");
channel.BasicPublish(exchange: string.Empty, routingKey: requestQueueName, body: message,
    basicProperties: properties);

#endregion


#region response kuyruğu dinleme  consumer davranış sergilenecek yer 
EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
channel.BasicConsume(queue: responseQueueName,autoAck:true,consumer:consumer);

consumer.Received += (sender, e) =>
{
	if (e.BasicProperties.CorrelationId==correlationId)
	{
        Console.WriteLine($"Response : {Encoding.UTF8.GetString(e.Body.Span)}");
    }
};
#endregion



#endregion

Console.Read();