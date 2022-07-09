using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Fulfillment.WindowsService._9M.Services
{
    public class RequestConsumer
    {
        private readonly RabbitMQService _rabbitMQService;

        public RequestConsumer(string queueName)
        {
            _rabbitMQService = new RabbitMQService();

            using (var connection = _rabbitMQService.GetRabbitMQConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var consumer = new EventingBasicConsumer(channel);
                    // Received event'i sürekli listen modunda olacaktır.
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body.ToArray());

                        string path = AppDomain.CurrentDomain.BaseDirectory + "\\News";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\News\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
                        if (!File.Exists(filepath))
                        {
                            using (StreamWriter sw = File.CreateText(filepath))
                            {
                                sw.WriteLine(message);
                            }
                        }
                        else
                        {
                            using (StreamWriter sw = File.AppendText(filepath))
                            {
                                sw.WriteLine(message);
                            }
                        }
                    };
                    //queue: Hangi Queue’nun mesajları alınacak ise.
                    //noAck: True olarak set edildiği taktirde, consumer mesajı aldığı zaman otomatik olarak mesaj Queue’dan silinecektir. Eğer Queue üzerinden silinmesini istemiyor iseniz, False olarak set etmeniz gerekmektedir.
                    //channel.BasicConsume(queueName, true, consumer, false, true);
                }
            }
        }
    }
}
