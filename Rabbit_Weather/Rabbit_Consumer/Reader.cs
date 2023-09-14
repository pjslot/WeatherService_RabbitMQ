using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rabbit_Consumer
{
	internal class Reader
	{

		public static string ReadMessage()
		{

			var factory = new ConnectionFactory()
			{
				//работаем с локальным рэббитом в докере
				HostName = "localhost"
			};

			using (var connection = factory.CreateConnection())
			{
				using (var chanel = connection.CreateModel())
				{
					chanel.QueueDeclare(queue: "weatherQueue",
						exclusive: false,
						durable: true,
						autoDelete: false,
						arguments: null);

					var consumer = new EventingBasicConsumer(chanel);


					//consumer.Received += (model, es) =>
					//{
					//	var body = es.Body.ToArray();
					//	var message = Encoding.UTF8.GetString(body);						
					//	Console.WriteLine(message);						
					//};

					consumer.Received += (model, es) =>
					{
						var body = es.Body.ToArray();
						var message = Encoding.UTF8.GetString(body);
						Console.WriteLine(message);
						Console.WriteLine("город принят!");
						//написать что-то чтоб строку выкидывало наружу
						//либо тут десериализовать и уже наваливать в базу
					};
						
						
						

					chanel.BasicConsume(queue: "weatherQueue",
						autoAck: true,
						consumer: consumer
						);

					return null; //!!!
				}
			}
		}
	}
}
