using MassTransit;
using RabbitMQExample.ESB.MassTransit.Shared.RequestResponseMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQExample.ESBMassTransit.RequestResponseConsumer.Consumers
{
    public class RequestMessageConsumer : IConsumer<RequestMessage>
    {
        public  async Task Consume(ConsumeContext<RequestMessage> context)
        {
            //gelen mesajla ilgli yapılacak işlemler
            Console.WriteLine(context.Message.Text);
            await context.RespondAsync<ResponseMessage>(new() { Text = $"{context.Message.MessageNo}.response to request" });
            
        }
    }
}
