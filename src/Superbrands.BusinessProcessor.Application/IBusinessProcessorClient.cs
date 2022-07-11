using System.Threading;
using System.Threading.Tasks;

namespace Superbrands.BusinessProcessor.Application
{
    public interface IBusinessProcessorClient
    {
        Task SendSignalToProcessor<T>(string signalName, T message, CancellationToken cancellationToken) where T : class;
    }
}