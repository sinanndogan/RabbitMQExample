
using RabbitMQ.Client;
using System.Text;

//Önce factory sınfı ile rabbitMQ bağlantısı oluşturulur
//RabitMQ sunucusuna bağlandık.
ConnectionFactory factory = new ();
factory.Uri = new("amqps://localhost");


//Bağlantıyı aktfileştirme ve kanal açma

using IConnection connection = factory.CreateConnection();  //bağlantı
using IModel channel = connection.CreateModel();    //kanal 

 
//Kuyruk Queue Oluşturma 
//kuyruk adı example-queue
channel.QueueDeclare(queue:"example-queue",exclusive:false, durable: true);


//Queue'ya Mesaj Gönderme
//RabbitMQ kuyruğa atacağı mesajarı byte türünden kabul etmektedir.Haliyle mesajları bizim byte dönüştürmemiz gerekecektir.

//byte [] message = Encoding.UTF8.GetBytes("Merhaba");
//channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message);

//kuyruk mesajı kalıcılığı sağladık
IBasicProperties properties =channel.CreateBasicProperties();
properties.Persistent = true;

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("Merhaba");
    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message, basicProperties: properties);
}

Console.Read();
