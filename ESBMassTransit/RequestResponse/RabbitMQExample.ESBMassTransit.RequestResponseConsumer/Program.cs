
using MassTransit;
using RabbitMQExample.ESB.MassTransit.Shared.RequestResponseMessages;
using RabbitMQExample.ESBMassTransit.RequestResponseConsumer.Consumers;

string rabbitMQUri = "yourlocalhost";

string requestQueue = "request-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);
    factory.ReceiveEndpoint(requestQueue, endponit =>
    {
        endponit.Consumer<RequestMessageConsumer>();
    });
});

await bus.StartAsync();
