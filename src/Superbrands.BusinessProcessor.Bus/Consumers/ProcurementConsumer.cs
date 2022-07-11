using System.Threading;
using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using Superbrands.Bus.Contracts.CSharp;
using Superbrands.Bus.Contracts.CSharp.MsSelections.Procurement;
using Superbrands.BusinessProcessor.Application;

namespace Superbrands.BusinessProcessor.Bus.Consumers
{
    /// <summary>
    /// Задачи связанные с отборкой и закупкой
    /// </summary>
    internal class ProcurementConsumer : IConsumeAsync<BusMessages<Procurement>>
    {
        private readonly IBusinessProcessorClient _client;

        public ProcurementConsumer(IBusinessProcessorClient client)
        {
            _client = client;
        }

        /// <inheritdoc />
        public async Task ConsumeAsync(BusMessages<Procurement> messages, CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var message in messages.Messages)
            {
               await _client.SendSignalToProcessor(nameof(Procurement), message, cancellationToken);
            }
        }
    }
}