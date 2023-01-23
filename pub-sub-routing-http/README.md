# pub-sub-routing-http

Shows pub sub routing using the dapr .NET SDK with declarative subscriptions to http endpoints.  

Uses a hierarchy of products (Gadget, Widget, Thingamajig inheriting from abstract Product), requiring polymorphic serialisation and deserialisation in STJ in .NET 7.0 of the CloudEvent data.   

Also shows a NotAProduct that goes to the dead letter topic, but subscription of dead letter topic is not currently working with declarative subscriptions (it does work with programmatic subscriptions). See https://github.com/dapr/dotnet-sdk/issues/998