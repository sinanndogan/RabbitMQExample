
using MassTransit;
using RabbitMQExample.ESB.MassTransit.Shared.RequestResponseMessages;

string rabbitMQUri = "yourlocalhost";

string requestQueue = "request-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);
});


await bus.StartAsync();

var request = bus.CreateRequestClient<RequestMessage>(new Uri($"{rabbitMQUri}/{requestQueue}"));


int i = 1;
while (true)
{
    await Task.Delay(200);
   var response = await   request.GetResponse<ResponseMessage>(new() { MessageNo = i, Text = $"{i}.Request" });

    Console.WriteLine($"Response Received : {response.Message.Text}");
}