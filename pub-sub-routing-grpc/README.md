# pub-sub-routing-grpc   

Shows pub sub routing using the dapr .NET SDK with declarative subscriptions to the dapr grpc AppCallback service.   

Uses a hierarchy of products (Gadget, Widget, Thingamajig inheriting from abstract Product), requiring polymorphic serialisation and deserialisation in .NET 7.0 STJ of the CloudEvent data.   

The TopicEventRequest.Path in the call to AppCallback OnTopicEvent does contain the route from the matched rule in the subscription.yaml, however I have not found a way to use this to route the event to a corresponding endpoint. Ideally the dapr sidecar invokes the corresponding grpc endpoint, without going through the AppCallback service.   

Instead I use the MassTransit Mediator. This works using polymorphic dispatch, which is different from how the pub sub routing rules work. For instance a Gadget will be consumed by both the Gadget consumer and the base Product consumer, whereas the pub sub routing rules would send the Gadget only to the Gadget endpoint.   

In any case the TopicEventRequest.Data needs to be deserialised using the TopicEventRequest.Type / CloudEvent.Type which now needs to be an AssemblyQualifiedName. We cannot use polymorphic deserialisation by STJ in .NET 7.0 as we don't want to hardcode here that it can only be a Product.   

Also shows a NotAProduct that goes to the dead letter topic, but need to think of a way to make the dead letter topic subscription work with MassTransit Mediator.