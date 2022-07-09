using Chams.Vtumanager.Provisioning.Entities.Common;
using Chams.Vtumanager.Provisioning.Entities.ViewModels;
using Chams.Vtumanager.Provisioning.Services.TransactionRecordService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Core.DependencyInjection;
using RabbitMQ.Client.Core.DependencyInjection.Services;
using RabbitMQ.Client.Core.DependencyInjection.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Services.QueService
{
    public class AMQService :IAMQService
    {
        private readonly IProducingService _producingService;

        //private readonly IConnectionFactory _quefactory;
        private readonly ILogger<IAMQService> _logger;
        //private readonly IQueueService _queueService;
        //private readonly IConfiguration _config;

        public AMQService(
            //IConnectionFactory quefactory,
            IProducingService producingService,
            ITransactionRecordService transactionRecordService,
            ILogger<IAMQService> logger)
{
            //_quefactory = quefactory;
            _producingService = producingService;
            _logger = logger;

        }

        public async Task<bool> SubmitTopupOrder(RechargeRequest rechargeRequest)
        {
            bool msgStatus = false;

            switch (rechargeRequest.ServiceProviderId)
            {
                case (int)ServiceProvider.Mtn:
                    _logger.LogInformation("queing message in mtn.que.");
                    await _producingService.SendAsync(@object: rechargeRequest, exchangeName: "amq.direct", routingKey: "mtn.queue");
                    break;
                case (int)ServiceProvider.Airtel:
                    _logger.LogInformation("queing message in airtel.que.");
                    await _producingService.SendAsync(@object: rechargeRequest, exchangeName: "amq.direct", routingKey: "airtel.queue");
                    break;
                case (int)ServiceProvider.GLO:
                    _logger.LogInformation("queing message in glo.que.");
                    await _producingService.SendAsync(@object: rechargeRequest, exchangeName: "amq.direct", routingKey: "glo.queue");
                    break;
                case (int)ServiceProvider.NineMobile:
                    _logger.LogInformation("queing message in 9m.que.");
                    await _producingService.SendAsync(@object: rechargeRequest, exchangeName: "amq.direct", routingKey: "ninemobile.queue");
                    break;

                default:
                    break;
            }
            //await _producingService.SendAsync(@object: rechargeRequest, exchangeName: "vtu.exchange", routingKey: "mtnque");

            //await _queueService.SendAsync(
            //    @object: rechargeRequest,
            //    exchangeName: "mtn_exchange",
            //    routingKey: "mtnque");

            //AmqpServerSettings _settings = new AmqpServerSettings();
            //_settings = _config.GetSection("AmqpServerSettings").Get<AmqpServerSettings>();
            //Uri uri = new Uri("amqp://vtuadmin:Vtu@adm1na@139.59.174.247:5672");
            //ConnectionFactory factory = new ConnectionFactory();
            //factory.Uri = uri;

            //using (var connection = _quefactory.CreateConnection())
            //{
            //    using (var channel = connection.CreateModel())
            //    {
            //        switch (rechargeRequest.ServiceProviderId)
            //        {
            //            case (int)ServiceProvider.Mtn:
            //                channel.ExchangeDeclare(exchange: "mtn_exchange", type: ExchangeType.Fanout);
            //                break;
            //            case (int)ServiceProvider.Airtel:
            //                channel.ExchangeDeclare(exchange: "airtel_exchange", type: ExchangeType.Fanout);
            //                break;
            //            case (int)ServiceProvider.GLO:
            //                channel.ExchangeDeclare(exchange: "glo_exchange", type: ExchangeType.Fanout);
            //                break;
            //            case (int)ServiceProvider.NineMobile:
            //                channel.ExchangeDeclare(exchange: "ninemobile_exchange", type: ExchangeType.Fanout);
            //                break;

            //            default:
            //                break;
            //        }

            //        var stringfiedMessage = JsonConvert.SerializeObject(rechargeRequest);
            //        var byteMessage = Encoding.UTF8.GetBytes(stringfiedMessage);

            //        var body = Encoding.UTF8.GetBytes(stringfiedMessage);
            //        channel.BasicPublish(exchange: "mtn_exchange",
            //                             routingKey: "mtnque",
            //                             basicProperties: null,
            //                             body: body);

            //        _logger.LogInformation($"Submitted message : {rechargeRequest.TransactionReference}");

            //    }
            //}

            return msgStatus;
        }
    }
}
