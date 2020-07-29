using System;
using System.Threading.Tasks;
using MultiEndpointTest.Messages;
using NServiceBus;

namespace MultiEndpointTest.PriorityHandlers
{
	public class SomePriorityEventHandler : IHandleMessages<SomePriorityEvent>
	{
		public Task Handle(SomePriorityEvent message, IMessageHandlerContext context) {
			ConsoleWriter.WriteRedLine("Well, you've done it now... a Priority Message was received!");
			return Task.CompletedTask;
		}
	}
}
