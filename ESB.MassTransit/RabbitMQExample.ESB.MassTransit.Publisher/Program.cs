

using MassTransit;
using RabbitMQExample.ESB.MassTransit.Shared.Messages;

string rabbitMQUri = "yourlocalhost";

string queueName = "example-queue";


IBusControl bus =Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);
});

//Gönderileecek endpoint mesaj buraya gidecek

ISendEndpoint sendEndpoint = await  bus.GetSendEndpoint(new($"{rabbitMQUri}/{queueName}"));


//Send tek bir kuyruğa tek bir hedefe mesaj göndermek için kullandığımız yapıdır.

Console.Write("Gönderilecek Mesaj :  ");
string message =Console.ReadLine();
await sendEndpoint.Send<IMessage>(new ExampleMessage()
{
    Text = message
}); 

Console.Read();