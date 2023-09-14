using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rabbit_Producer.RabbitMq
{
	public interface IRabbitMqService
	{
		void SendMessage(object obj);
		void SendMessage(string message);
	}
}
