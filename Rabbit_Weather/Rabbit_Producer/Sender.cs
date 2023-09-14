using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rabbit_Producer
{
	internal class Sender
	{
		public static void SendMessage(string forecastToGo)
		{

			var factory = new ConnectionFactory()
			{
				//поднял сервер на cloudamqp, при коннекте
				//HostName = "amqps://mwxdvlrv:Re0VE-6_WPQM_doK1rN7OJ3-crF_BqrL@kangaroo.rmq.cloudamqp.com/mwxdvlrv"
				//выдаёт "None of the specified endpoints were reachable" как ни крути. пока не разобрался.
				//поэтому поднимаем локальный контейнер в докере:
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

					var message = forecastToGo;
					var body = Encoding.UTF8.GetBytes(message);
					chanel.BasicPublish(exchange: "",
						routingKey: "weatherQueue",
						basicProperties: null,
						body: body);
					Console.WriteLine(message+"done!");
				}
			}
		}
	}
}
