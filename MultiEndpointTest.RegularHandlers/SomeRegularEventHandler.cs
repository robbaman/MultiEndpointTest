using System;
using System.Threading.Tasks;
using MultiEndpointTest.Messages;
using NServiceBus;

namespace MultiEndpointTest.RegularHandlers
{
	public class SomeRegularEventHandler : IHandleMessages<SomeRegularEvent>
	{
		public Task Handle(SomeRegularEvent message, IMessageHandlerContext context) {
			ConsoleWriter.WriteRedLine("Well, you've done it now... a Regular Message was received!");
			return Task.CompletedTask;
		}
	}
}
