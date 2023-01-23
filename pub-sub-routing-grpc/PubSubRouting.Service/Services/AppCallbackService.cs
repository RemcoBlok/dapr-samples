using Dapr.AppCallback.Autogen.Grpc.v1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MassTransit.Mediator;
using PubSubRouting.Framework;
using System.Text.Json;

namespace PubSubRouting.Service.Services
{
    public class AppCallbackService : AppCallback.AppCallbackBase
    {
        private readonly Dapr.Client.DaprClient _daprClient;
        private readonly IMediator _mediator;

        public AppCallbackService(Dapr.Client.DaprClient daprClient, IMediator mediator)
        {
            _daprClient = daprClient;
            _mediator = mediator;
        }

        public override Task<ListTopicSubscriptionsResponse> ListTopicSubscriptions(Empty request, ServerCallContext context)
        {
            // No need to hardcode subscriptions here as the subscriptions are picked up from the subscription.yaml.
            // However, you are still required to host this AppCallback service with the ListTopicSubscriptions method returning a non null but empty list of subscriptions.
            return Task.FromResult(new ListTopicSubscriptionsResponse());
        }

        public async override Task<TopicEventResponse> OnTopicEvent(TopicEventRequest request, ServerCallContext context)
        {
            // TODO: The TopicEventRequest.Path is filled from the matched routing rule in the subscription.yaml.
            // Can we use TopicEventRequest.Path here to route to a specific grpc endpoint?
            // In any case we need to use JsonSerializer to deserialize the json in the TopicEventRequest.Data as the ByteString cannot be sent as is to a grpc endpoint.
            // The ByteString still represents json. When invoking a grpc endpoint the data needs to be serialized with protobuf.
            // This is presumably also the reason why the dapr sidecar cannot directly invoke the grpc endpoint for the path of the matched routing rule as it can with a json http endpoint.
            // The MassTransit mediator used below will use polymorphic dispatch, which is not the same as the routing rules in the subscription.yaml.
            // For instance a Gadget will be consumed by both the Gadget consumer and the base Product consumer, whereas the pubsub routing rules will send the Gadget only to the Gadget endpoint.            

            System.Type dataType = TypeCache.GetType(request.Type);// must be AssemblyQualifiedName of Type, but may use simple Name of Assembly (without the Version, Culture and PublicKeyToken) instead of FullName of Assembly (with the Version, Culture and PublicKeyToken)
            object data = JsonSerializer.Deserialize(request.Data.Span, dataType, _daprClient.JsonSerializerOptions)!;

            await _mediator.Send(data, data.GetType());

            return new TopicEventResponse();
        }
    }
}
