using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using EventBus.UnitTest.Events.EventHandlers;
using EventBus.UnitTest.Events.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xunit;

namespace EventBus.UnitTest
{
  public class EventBusTests
  {
    private ServiceCollection services;

    public EventBusTests()
    {
      services = new ServiceCollection();
      services.AddLogging(configure => configure.AddConsole());
    }

    [Fact]
    public void subscribe_event_on_rabbitmq_test()
    {
      services.AddSingleton<IEventBus>(sp =>
      {
        return EventBusFactory.Create(GetRabbitMQConfig(), sp);
      });


      var sp = services.BuildServiceProvider();

      var eventBus = sp.GetRequiredService<IEventBus>();

      eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
      //eventBus.UnSubscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
      Task.Delay(2000).Wait();
    }

    [Fact]
    public void subscribe_event_on_azure_test()
    {
      services.AddSingleton<IEventBus>(sp =>
      {
        return EventBusFactory.Create(GetAzureConfig(), sp);
      });


      var sp = services.BuildServiceProvider();

      var eventBus = sp.GetRequiredService<IEventBus>();

      eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
      //eventBus.UnSubscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();

      Task.Delay(2000).Wait();
    }

    [Fact]
    public void send_message_to_rabbitmq_test()
    {
      services.AddSingleton<IEventBus>(sp =>
      {
        return EventBusFactory.Create(GetRabbitMQConfig(), sp);
      });


      var sp = services.BuildServiceProvider();

      var eventBus = sp.GetRequiredService<IEventBus>();

      eventBus.Publish(new OrderCreatedIntegrationEvent(1));
    }

    [Fact]
    public void send_message_to_azure_test()
    {
      services.AddSingleton<IEventBus>(sp =>
      {
        return EventBusFactory.Create(GetAzureConfig(), sp);
      });


      var sp = services.BuildServiceProvider();

      var eventBus = sp.GetRequiredService<IEventBus>();

      eventBus.Publish(new OrderCreatedIntegrationEvent(1));
    }

    private EventBusConfig GetAzureConfig()
    {
      return new EventBusConfig()
      {
        ConnectionRetryCount = 5,
        SubscriberClientAppName = "EventBus.UnitTest",
        DefaultTopicName = "SellingTechTopicName",
        EventBusType = EventBusType.AzureServiceBus,
        EventNameSuffix = "IntegrationEvent",
        EventBusConnectionString = ""
      };
    }

    private EventBusConfig GetRabbitMQConfig()
    {
      return new EventBusConfig()
      {
        ConnectionRetryCount = 5,
        SubscriberClientAppName = "EventBus.UnitTest",
        DefaultTopicName = "SellingTechTopicName",
        EventBusType = EventBusType.RabbitMQ,
        EventNameSuffix = "IntegrationEvent"
      };
    }
  }

}
