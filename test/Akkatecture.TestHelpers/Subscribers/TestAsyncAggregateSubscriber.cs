using System.Threading.Tasks;
using Akkatecture.Aggregates;
using Akkatecture.Subscribers;
using Akkatecture.TestHelpers.Aggregates;
using Akkatecture.TestHelpers.Aggregates.Events;

namespace Akkatecture.TestHelpers.Subscribers
{
    public class TestAsyncAggregateSubscriber: DomainEventSubscriber,
        ISubscribeToAsync<TestAggregateId, TestCreatedEvent>,
        ISubscribeToAsync<TestAggregateId, TestAddedEvent>
    {
        public Task HandleAsync(IDomainEvent<TestAggregateId, TestCreatedEvent> domainEvent)
        {
            var handled = new TestAsyncSubscribedEventHandled<TestCreatedEvent>(domainEvent.AggregateEvent);
            Context.System.EventStream.Publish(handled);
            return Task.CompletedTask;
        }
        
        public Task HandleAsync(IDomainEvent<TestAggregateId, TestAddedEvent> domainEvent)
        {
            var handled = new TestAsyncSubscribedEventHandled<TestAddedEvent>(domainEvent.AggregateEvent);
            Context.System.EventStream.Publish(handled);
            
            return Task.CompletedTask;
        }
    }

    public class TestAsyncSubscribedEventHandled<TAggregateEvent> 
    {
        public TAggregateEvent AggregateEvent { get;}

        public TestAsyncSubscribedEventHandled(TAggregateEvent aggregateEvent)
        {
            AggregateEvent = aggregateEvent;
        }
    }
}