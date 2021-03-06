using System.Threading.Tasks;
using Akka.Actor;
using Akkatecture.Aggregates;
using Akkatecture.Aggregates.Snapshot;
using Akkatecture.Aggregates.Snapshot.Strategies;
using Akkatecture.Commands;
using Akkatecture.Sagas;
using Akkatecture.Sagas.AggregateSaga;
using Akkatecture.TestHelpers.Aggregates.Commands;
using Akkatecture.TestHelpers.Aggregates.Events;
using Akkatecture.TestHelpers.Aggregates.Sagas.TestAsync.Events;
using Akkatecture.TestHelpers.Aggregates.Snapshots;

namespace Akkatecture.TestHelpers.Aggregates.Sagas.TestAsync
{
    [SagaName("TestAsyncSaga")]
    public class TestAsyncSaga: AggregateSaga<TestAsyncSaga,TestAsyncSagaId,TestAsyncSagaState>,
        ISagaIsStartedByAsync<TestAggregateId, TestSentEvent>,
        ISagaHandlesAsync<TestAggregateId, TestReceivedEvent>
    {
        public TestAsyncSaga()
        {
            
            Command<EmitTestSagaState>(Handle);
            // ReSharper disable once VirtualMemberCallInConstructor
            SetSnapshotStrategy(SnapshotAlwaysStrategy.Instance);
        }

        public async Task HandleAsync(IDomainEvent<TestAggregateId, TestSentEvent> domainEvent)
        {
            if (IsNew)
            {
                var command = new ReceiveTestCommand(
                    domainEvent.AggregateEvent.RecipientAggregateId,
                    CommandId.New,
                    domainEvent.AggregateIdentity,
                    domainEvent.AggregateEvent.Test);

                Emit(new TestAsyncSagaStartedEvent(domainEvent.AggregateIdentity, domainEvent.AggregateEvent.RecipientAggregateId, domainEvent.AggregateEvent.Test), new EventMetadata{{"some-key","some-value"}});

                await PublishCommandAsync(command);

            }
        }

        public Task HandleAsync(IDomainEvent<TestAggregateId, TestReceivedEvent> domainEvent)
        {
            if (!IsNew)
            {
                Emit(new TestAsyncSagaTransactionCompletedEvent());
                Self.Tell(new EmitTestSagaState());

            }
            return Task.CompletedTask;
        }

        protected override IAggregateSnapshot<TestAsyncSaga, TestAsyncSagaId> CreateSnapshot()
        {
            return new TestAsyncSagaSnapshot
            {
                ReceiverId = State.Receiver.Value,
                SenderId = State.Sender.Value,
                Test = new TestAggregateSnapshot.TestModel(State.Test.Id.GetGuid())
            };
        }

        private bool Handle(EmitTestSagaState testCommmand)
        {
            Emit(new TestAsyncSagaCompletedEvent(State));
            return true;
        }

        private class EmitTestSagaState
        {
        }
    }
}