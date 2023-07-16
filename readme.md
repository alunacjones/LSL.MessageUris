[![Build status](https://img.shields.io/appveyor/ci/alunacjones/lsl-messageuris.svg)](https://ci.appveyor.com/project/alunacjones/lsl-messageuris)
[![Coveralls branch](https://img.shields.io/coverallsCoverage/github/alunacjones/LSL.MessageUris)](https://coveralls.io/github/alunacjones/LSL.MessageUris)
[![NuGet](https://img.shields.io/nuget/v/LSL.MessageUris.svg)](https://www.nuget.org/packages/LSL.MessageUris/)

# LSL.MessageUris

A simple library to provide message uris that can be used to encapsulate intent for data.

## LSL.MessageUris.SendMessageUri

### Creation

Create an instance of the `SendMessageUri`:

```csharp
var uri = new SendMessageUri("my queue");

// uri.DestinationAddress will be set to "my queue"

// QueryParameters allows for user defined options that your system may interpret for further context
uri.QueryParameters.Add("single", "a-val");
uri.QueryParameters.Add("double", "first");
uri.QueryParameters.Add("double", "second");

var realUri = uri.ToUri();

// realUri.ToString() will be "send-message:my queue?single=a-val&double=first&double=second"
```

### Parsing

For a potentially exception-throwing parse (if the format is wrong) see the following example:

```csharp
var uri = SendMessageUri.Parse("send-message:my-queue");
// uri.DestinationQueue is set to "my-queue"
// uri.DestinationExchange is set to ""
// uri.DestinationQueueAndExchange is set to "my-queue"

var uri2 = SendMessageUri.Parse("send-message:my-queue@exchange");
// uri2.DestinationQueue is set to "my-queue"
// uri2.DestinationExchange is set to "exchange"
// uri2.DestinationQueueAndExchange is set to "my-queue@exchange"

// Parsing from an existing Uri instance
var uri3 = SendMessageUri.Parse(new Uri("send-message:my-queue@exchange"));
// uri3.DestinationQueue is set to "my-queue"
// uri3.DestinationExchange is set to "exchange"
// uri3.DestinationQueueAndExchange is set to "my-queue@exchange"

```

For a safe attempt at parsing:

```csharp
if (SendMessageUri.TryParse("send-message:my-queue", out var result))
{
    // do something with result now
};
```

## LSL.MessageUris.PublishMessageUri

### Creation

Create an instance of the `PublishMessageUri`:

```csharp
var uri = new PublishMessageUri("my exchange", "my topic");

// uri.Exchange will be set to "my exchange"
// uri.Topic will be set to "my topic"
// uri.TopicAndExchange will be set to "my topic@my exchange"

// QueryParameters allows for user defined options that your system may interpret for further context
uri.QueryParameters.Add("single", "a-val");
uri.QueryParameters.Add("double", "first");
uri.QueryParameters.Add("double", "second");

var realUri = uri.ToUri();

// realUri.ToString() will be "publish-message:my topic@my exchange?single=a-val&double=first&double=second"
```

### Parsing

For a potentially exception-throwing parse (if the format is wrong) see the following example:

```csharp
var uri = PublishMessageUri.Parse("publish-message:my-topic");
// uri2.Topic will be set to "my-topic"
// uri2.Exchange will be set to ""
// uri2.TopicAndExchange will be set to "my-topic"
// uri2.QueryParameters will be empty

var uri2 = PublishMessageUri.Parse("publish-message:my-topic@my-ex");

// uri2.Topic will be set to "my-topic"
// uri2.Exchange will be set to "my-ex"
// uri2.TopicAndExchange will be set to "my-topic@my-ex"
// uri2.QueryParameters will be empty
```

For a safe attempt at parsing:

```csharp
if (SendMessageUri.TryParse("publish-message:my-topic@my-ex", out var result))
{
    // do something with result now
};
```

