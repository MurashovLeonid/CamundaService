using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Camunda.Api.Client;
using Camunda.Api.Client.Signal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Superbrands.BusinessProcessor.Application.Camunda
{
    internal class SuperbrandsCamundaClient : IBusinessProcessorClient
    {
        private readonly CamundaClient _camundaClient;
        public SuperbrandsCamundaClient(IOptions<BusinessProcessorOptions> options)
        {
            _camundaClient = CamundaClient.Create(options.Value.ProcessorUrl);
        }

        /// <inheritdoc />
        public async Task SendSignalToProcessor<T>(string signalName, T message, CancellationToken cancellationToken)
        where T : class
        {
            await _camundaClient.Signals.ThrowSignal(new Signal
            {
                Variables = new Dictionary<string, VariableValue>
                {
                    ["Payload"] = VariableValue.FromObject(message)
                },
                Name = signalName
            });
        }
    }
}