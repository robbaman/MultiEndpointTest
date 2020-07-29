# MultiEndpointTest
Example of how multiple NServiceBus endpoints in a single process seems to fail when using the external Microsoft Dependency Injection container

## Usage
Make sure you have RabbitMQ running locally with the default guest/guest user and a vhost of `testvhost` (otherwise you'll have to change the hardcoded connectionstring). Then just start debugging and check the console.

Note that when running this project you should expect to get 2 notifications of the `SomeRegularEvent` occurring and 2 notifications of the `SomePriorityEvent` occurring. Instead you'll get too many regular events and no priority events.

After running the example you can see in the RabbitMQ management portal that the exchanges were created incorrectly. The `MultiEndpointTest.Messages:SomeRegularEvent` exchange will have bindings to both the `SomeService` and `SomeService.Priority` exchanges and the expected `MultiEndpointTest.Messages:SomePriorityEvent` exchange is not created at all.
