using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Camunda.Api.Client;
using Camunda.Api.Client.Message;
using Camunda.Api.Client.ProcessDefinition;
using Camunda.Api.Client.Signal;
using Microsoft.AspNetCore.Mvc;
using Superbrands.Bus.Contracts.CSharp;
using Superbrands.Bus.Contracts.CSharp.MsSelections.Enums;
using Superbrands.Bus.Contracts.CSharp.MsSelections.Procurement;
using Superbrands.Bus.Contracts.CSharp.MsSelections.Selections;
using Superbrands.BusinessProcessor.Application;
using Swashbuckle.AspNetCore.Filters;

namespace Superbrands.BusinessProcessor.WebApi.Controllers
{
    [ApiController]
    [Route("/api/v1/camunda")]
    public class CamundaTestController : Controller
    {
        private readonly IBusinessProcessorClient _client;

        public CamundaTestController(IBusinessProcessorClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        [HttpGet]
        [Route("ProcurementCreatedSendTest")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        public async Task SendProcurement(CancellationToken cancellationToken)
        {
            var procurement = new Procurement {Selections = new ChangedEntitiesCollection<Selection>(), Id = 1};
            procurement.Selections.AddCreated(new Selection {Id = 1, BuyerId = 1});
            procurement.Selections.AddCreated(new Selection {Id = 2, BuyerId = 2});
            procurement.Selections.AddCreated(new Selection {Id = 3, BuyerId = 4});

            await _client.SendSignalToProcessor(nameof(Procurement),
                new BusMessage<Procurement>(procurement, CrudEventType.Create, 1), cancellationToken);
        }

        [HttpGet]
        [Route("SelectionsStatusChangesSendTest")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        public async Task SelectionsStatusChanges(CancellationToken cancellationToken)
        {
            var procurement = new Procurement {Selections = new ChangedEntitiesCollection<Selection>(), Id = 1,PartnerId = 9};
            procurement.Selections.AddUpdated(new Selection {Id = 1, BuyerId = 1, Status = SelectionStatus.OnApproval});
            procurement.Selections.AddUpdated(new Selection {Id = 2, BuyerId = 2, Status = SelectionStatus.InProgress});
            procurement.Selections.AddUpdated(new Selection {Id = 3, BuyerId = 4,Status = SelectionStatus.Agreed});
            
            var originalProcurement = new Procurement {Selections = new ChangedEntitiesCollection<Selection>(), Id = 1, PartnerId = 9};
            originalProcurement.Selections.AddUpdated(new Selection {Id = 1, BuyerId = 1, Status = SelectionStatus.InProgress});
            procurement.Selections.AddUpdated(new Selection {Id = 2, BuyerId = 2, Status = SelectionStatus.OnApproval});
            procurement.Selections.AddUpdated(new Selection {Id = 3, BuyerId = 4,Status = SelectionStatus.OnApproval});

            await _client.SendSignalToProcessor(nameof(Procurement),
                new BusMessage<Procurement>(procurement, CrudEventType.Update, 1, originalProcurement), cancellationToken);
        }
    }
}