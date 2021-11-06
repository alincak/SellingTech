using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;
using PaymentService.Api.IntegrationEvents.Events;
using System.Threading.Tasks;

namespace NotificationService.IntegrationEvents.EventHandlers
{
  class OrderPaymentSuccessIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentSuccessIntegrationEvent>
  {

    private readonly ILogger<OrderPaymentSuccessIntegrationEventHandler> logger;

    public OrderPaymentSuccessIntegrationEventHandler(ILogger<OrderPaymentSuccessIntegrationEventHandler> logger)
    {
      this.logger = logger;
    }

    public Task Handle(OrderPaymentSuccessIntegrationEvent @event)
    {
      // Send Fail Notification (Sms, EMail, Push)

      logger.LogInformation($"Order Payment Success with OrderId: {@event.OrderId}");

      return Task.CompletedTask;
    }
  }
}
