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
channel.QueueDeclare(queue: "example-queue", exclusive: false, durable:true);



// Queue'dan mesaj Okuma

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: "example-queue", autoAck:false,consumer);//1.kuyruk hangi kanal diye soruyor
channel.BasicQos(0, 1, true); // fair dispatch

consumer.Received += (sender, e) =>
{
    // Kuyruğa gelen masajın işlendiği yerdir !
    //e.Body : Kuyruktaki mesajın verisini bütünsel olrak getirecektir 
    //e.Body.Span veya e.Body.ToArry() : Kuyruktaki mesajın byte verisini getirecektir.
    //Nasıl ki publisher üzerinden mesaj gönderirken mesajı byte tipine çevirdik burada da stringe çevirme işlemi yapmalıyız encoding ile 
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
    //deliverytag bildirimde bulunacağımız mesaja karşın unique bir değerdir 
    //multiple:false yaparak sadece bu mesaja dair bir çalışma yapacağımızı belirtmiş olduk.
    channel.BasicAck(deliveryTag:e.DeliveryTag, multiple:false);

};

Console.Read();