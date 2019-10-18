﻿// The MIT License (MIT)
//
// Copyright (c) 2018 - 2019 Lutando Ngqakaza
// https://github.com/Lutando/EventFly 
// 
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.ComponentModel;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using EventFly.Commands;
using EventFly.TestHelpers.Aggregates;
using EventFly.TestHelpers.Aggregates.Commands;
using EventFly.TestHelpers.Aggregates.Events;
using EventFly.TestHelpers.Subscribers;
using EventFly.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using EventFly.TestHelpers.Aggregates.Sagas.TestAsync;
using EventFly.Definitions;

namespace EventFly.Tests.UnitTests.Subscribers
{
    [Collection("SubsriberTests")]
    public class SubscriberTests : TestKit
    {
        private const string Category = "Subscribers";

        public SubscriberTests(ITestOutputHelper testOutputHelper)
            :base(TestHelpers.Akka.Configuration.Config, "subscriber-tests", testOutputHelper)
        {
            Sys.RegisterDependencyResolver(new ServiceCollection().AddEventFly(Sys, db => db.RegisterDomainDefinitions<TestDomain>()).AddScoped<TestAsyncSaga>().BuildServiceProvider());

        }

        [Fact]
        [Category(Category)]
        public void Subscriber_ReceivedEvent_FromAggregatesEmit()
        {
            var eventProbe = CreateTestProbe("event-probe");
            Sys.EventStream.Subscribe(eventProbe, typeof(TestSubscribedEventHandled<TestCreatedEvent>));
            Sys.ActorOf(Props.Create(() => new TestAggregateSubscriber()), "test-subscriber");        
            var bus = Sys.GetExtension<ServiceProviderHolder>().ServiceProvider.GetRequiredService<ICommandBus>();

            var aggregateId = TestAggregateId.New;
            var commandId = CommandId.New;
            var command = new CreateTestCommand(aggregateId, commandId);
            bus.Publish(command).GetAwaiter().GetResult();

            eventProbe.
                ExpectMsg<TestSubscribedEventHandled<TestCreatedEvent>>(x =>
                    x.AggregateEvent.TestAggregateId == command.AggregateId);
            
        }
        
        [Fact]
        [Category(Category)]
        public void Subscriber_ReceivedAsyncEvent_FromAggregatesEmit()
        {
            var eventProbe = CreateTestProbe("event-probe");
            Sys.EventStream.Subscribe(eventProbe, typeof(TestAsyncSubscribedEventHandled<TestCreatedEvent>));
            Sys.ActorOf(Props.Create(() => new TestAsyncAggregateSubscriber()), "test-subscriber");        
            var bus = Sys.GetExtension<ServiceProviderHolder>().ServiceProvider.GetRequiredService<ICommandBus>();

            var aggregateId = TestAggregateId.New;
            var commandId = CommandId.New;
            var command = new CreateTestCommand(aggregateId, commandId);
            bus.Publish(command).GetAwaiter().GetResult();

            eventProbe
                .ExpectMsg<TestAsyncSubscribedEventHandled<TestCreatedEvent>>(x =>
                    x.AggregateEvent.TestAggregateId == command.AggregateId);
        }
    }
}