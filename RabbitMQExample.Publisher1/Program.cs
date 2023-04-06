
using RabbitMQ.Client;
using System.Text;

//Önce factory sınfı ile rabbitMQ bağlantısı oluşturulur
//RabitMQ sunucusuna bağlandık.
ConnectionFactory factory = new ();
factory.Uri = new("amqps://vgoeohfm:NmkSmsxycy31BqcSuJ7Me8qFIdR0E6Y_@shrimp.rmq.cloudamqp.com/vgoeohfm");


//Bağlantıyı aktfileştirme ve kanal açma

using IConnection connection = factory.CreateConnection();  //bağlantı
using IModel channel = connection.CreateModel();    //kanal 


//Kuyruk Queue Oluşturma 
//kuyruk adı example-queue
channel.QueueDeclare(queue:"example-queue",exclusive:false);


//Queue'ya Mesaj Gönderme
//RabbitMQ kuyruğa atacağı mesajarı byte türünden kabul etmektedir.Haliyle mesajları bizim byte dönüştürmemiz gerekecektir.

byte [] message = Encoding.UTF8.GetBytes("Merhaba");
channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message);

Console.Read();
