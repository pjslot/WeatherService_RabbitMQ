using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rabbit_Producer.RabbitMq
{
	public class RabbitMqService : IRabbitMqService
	{
		public void SendMessage(object obj)
		{
			var message = JsonSerializer.Serialize(obj);
			SendMessage(message);
		}
		public void SendMessage(string message)
		{
			//рэбит поднят на https://www.cloudamqp.com
			var factory = new ConnectionFactory() { Uri = new Uri("amqps://mwxdvlrv:Re0VE-6_WPQM_doK1rN7OJ3-crF_BqrL@kangaroo.rmq.cloudamqp.com/mwxdvlrv") };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queue: "MyQueue",
							   durable: false,
							   exclusive: false,
							   autoDelete: false,
							   arguments: null);

				var body = Encoding.UTF8.GetBytes(message);

				channel.BasicPublish(exchange: "",
							   routingKey: "MyQueue",
							   basicProperties: null,
							   body: body);
			}
		}
	}
}
