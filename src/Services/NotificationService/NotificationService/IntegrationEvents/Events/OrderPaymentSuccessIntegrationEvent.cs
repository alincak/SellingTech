using EventBus.Base.Events;
using System;

namespace NotificationService.IntegrationEvents.Events
{
  public class OrderPaymentSuccessIntegrationEvent : IntegrationEvent
  {
    public Guid OrderId { get; }

    public OrderPaymentSuccessIntegrationEvent(Guid orderId) => OrderId = orderId;
  }
}
