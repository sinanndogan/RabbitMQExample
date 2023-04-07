using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;



//Bağlantı Oluşturma

ConnectionFactory factory = new();
factory.Uri = new("amqps://vgoeohfm:NmkSmsxycy31BqcSuJ7Me8qFIdR0E6Y_@shrimp.rmq.cloudamqp.com/vgoeohfm");


//Bağlantı Aktifleştirme ve Kanal açma 

using IConnection connection = factory.CreateConnection();
using IModel channel =connection.CreateModel();

//Queue Oluşturma 

//Consumer'da da kuyruk kuyruk publisher'daki ile birebir aynı yapılandırmada tanımlanmalıdır!
channel.QueueDeclare(queue: "example-queue", exclusive: false);



// Queue'dan mesaj Okuma

EventingBasicConsumer consumer = new(channel);
//1.kuyruk hangi kanal diye soruyor
//2.parametre kuyruktan gelen veri okundauktan sonra silinsinmi diye verilmiştir 
channel.BasicConsume(queue: "example-queue",false,consumer);
consumer.Received += (sender, e) =>
{
    // Kuyruğa gelen masajın işlendiği yerdir !
    //e.Body : Kuyruktaki mesajın verisini bütünsel olrak getirecektir 
    //e.Body.Span veya e.Body.ToArry() : Kuyruktaki mesajın byte verisini getirecektir.
    //Nasıl ki publisher üzerinden mesaj gönderirken mesajı byte tipine çevirdik burada da stringe çevirme işlemi yapmalıyız encoding ile 
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};

Console.Read();