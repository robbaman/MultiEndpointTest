using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MultiEndpointTest.Messages;
using MultiEndpointTest.PriorityHandlers;
using MultiEndpointTest.RegularHandlers;
using NServiceBus;
using NServiceBus.Logging;

namespace MultiEndpointTest
{
    class Program
    {
        static async Task Main(string[] args) {
			var defaultFactory = LogManager.Use<DefaultFactory>();
			defaultFactory.Level(LogLevel.Debug);

			var services = new ServiceCollection();

			// Create 'regular' endpoint ignoring all handlers in the priority assembly
			var regularEndpointConfiguration = new EndpointConfiguration("SomeService");
			regularEndpointConfiguration.EnableInstallers();
			regularEndpointConfiguration.UseTransport<RabbitMQTransport>()
				.UseConventionalRoutingTopology()
				.ConnectionString("host=localhost;username=guest;password=guest;virtualhost=testvhost;RequestedHeartbeat=600");
			regularEndpointConfiguration.AssemblyScanner().ExcludeAssemblies(typeof(SomePriorityEventHandler).Assembly.GetName().Name);
			var regularStarableEndpoint = EndpointWithExternallyManagedServiceProvider.Create(regularEndpointConfiguration, services);


			// Create 'priority' endpoint ignoring all handlers in the regular assembly
			var priorityEndpointConfiguration = new EndpointConfiguration("SomeService.Priority");
			priorityEndpointConfiguration.EnableInstallers();
			priorityEndpointConfiguration.UseTransport<RabbitMQTransport>()
				.UseConventionalRoutingTopology()
				.ConnectionString("host=localhost;username=guest;password=guest;virtualhost=testvhost;RequestedHeartbeat=600");
			priorityEndpointConfiguration.AssemblyScanner().ExcludeAssemblies(typeof(SomeRegularEventHandler).Assembly.GetName().Name);
			var priorityStartableEndpoint = EndpointWithExternallyManagedServiceProvider.Create(priorityEndpointConfiguration, services);

			var provider = services.BuildServiceProvider();

			var regularEndpoint = await regularStarableEndpoint.Start(provider);
			var priorityEndpoint = await priorityStartableEndpoint.Start(provider);

			ConsoleWriter.WriteRedLine("Hit enter to send two events via each endpoint!");
			Console.ReadLine();


			ConsoleWriter.WriteGreenLine("Publishing two events from Regular Endpoint");
			await regularEndpoint.Publish(new SomeRegularEvent());
			await Task.Delay(TimeSpan.FromSeconds(5));
			await regularEndpoint.Publish(new SomePriorityEvent());
			await Task.Delay(TimeSpan.FromSeconds(5));

			ConsoleWriter.WriteGreenLine("Publishing two events from Priority Endpoint");
			await priorityEndpoint.Publish(new SomeRegularEvent());
			await Task.Delay(TimeSpan.FromSeconds(5));
			await priorityEndpoint.Publish(new SomePriorityEvent());


			// Wait a sec to process the messages
			await Task.Delay(TimeSpan.FromSeconds(1));

			await regularEndpoint.Stop();
			await priorityEndpoint.Stop();

			ConsoleWriter.WriteRedLine("Well done!");
			Console.ReadLine();
		}
    }
}
