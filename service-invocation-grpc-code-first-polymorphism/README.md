# service-invocation-grpc-code-first-polymorphism

Shows service invocation using the dapr .NET SDK and grpc code-first using protobuf-net.Grpc.   

Uses a hierarchy of the request and response types requiring polymorphic serialisation and deserialisation in the grpc service call and the http api call.   

Polymorphic serialisation and deserialisation in the http api call requires STJ in .NET 7.0. The discriminator value requires the FullName due to the type hierarchy having the same class names in different namespaces.   

The manager service uses a reflection-based convention-over-configuration strategy to call the use case in the namespace corresponding to the namespace of the request type. Also, this strategy effectively gives you covariance of `Task<TResult>` without introducing some covariant interface `ITask<out TResult>`.
