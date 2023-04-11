
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("localhost");

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

//string exchangeName = "example-pub-sub-exchange";

//channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

//string queueName = channel.QueueDeclare().QueueName;
//channel.QueueBind(
//    queue: queueName,
//    exchange: exchangeName,
//    routingKey: string.Empty
//    );

//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

//consumer.Received += (sender, e) =>
//{
//    string message = Encoding.UTF8.GetString(e.Body.Span);
//    Console.WriteLine(message);
//};

#endregion



#region Work Queue(İş Kuyruğu) Tasarımı
//Publisher tarafından yayınlanmış bir mesajın birden fazla consumer arsından yalnızca biri tarafınan tüketilmesi amaçlanmaktadır.Böylece mesajların işlenmesi sürecinde tüm consumer'lar aynı iş yüküne ve eşit görev dağılımına sahip olacaktırlar. 


//herbir consumerin tek bir mesaj işlemesini  garanti altına almış olduk 
//string queueName = "example-work-queue";

//channel.QueueDeclare(queue: queueName,
//    durable: false,
//    exclusive: false,
//    autoDelete: false);

//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(queue: queueName, autoAck: true, consumer:consumer);

////tüm consumerlar 1 adet mesaj okuyacak boyutu önemsiz 
//channel.BasicQos(prefetchCount: 1, prefetchSize: 0, global: false);

//consumer.Received += (sender, e) =>
//{
//    string message = Encoding.UTF8.GetString(e.Body.Span);
//    Console.WriteLine(message);
//};


#endregion



#region Request/Response Tasarımı
string requestQueueName = "example-request-response-queue";

channel.QueueDeclare(queue: requestQueueName, durable: false, exclusive: false, autoDelete: false);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: requestQueueName, autoAck: true, consumer: consumer);

consumer.Received += (sender, e) =>
{
   string message =Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
    byte[] responseMessage = Encoding.UTF8.GetBytes($"İşlem tamamlandı : {message} ");
    IBasicProperties properties =channel.CreateBasicProperties();
    properties.CorrelationId=e.BasicProperties.CorrelationId;
    channel.BasicPublish(exchange: string.Empty, routingKey: e.BasicProperties.ReplyTo,basicProperties:properties,body:responseMessage);
};
#endregion


Console.Read();