using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rabbit_Consumer
{
	internal class Reader
	{

		public static List<string> ReadMessage()
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

					List<string> all = new List<string>();

					consumer.Received += (model, es) =>
					{
						var body = es.Body.ToArray();
						var message = Encoding.UTF8.GetString(body);
						Console.WriteLine(message);
						all.Add(message);
						Log.Information("City accepted from the Rabbit queue.");
					};

				
						

					chanel.BasicConsume(queue: "weatherQueue",
						autoAck: true,
						consumer: consumer
						);

					return all;
				}
			}
		}
	}
}
