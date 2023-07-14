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

// This works and you end up with an instance with DestinationAddress set to "my-queue" but with no query parameters
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

// QueryParameters allows for user defined options that your system may interpret for further context
uri.QueryParameters.Add("single", "a-val");
uri.QueryParameters.Add("double", "first");
uri.QueryParameters.Add("double", "second");

var realUri = uri.ToUri();

// realUri.ToString() will be "publish-message:my exchange/my topic?single=a-val&double=first&double=second"
```

### Parsing

For a potentially exception-throwing parse (if the format is wrong) see the following example:

```csharp
var uri = PublishMessageUri.Parse("publish-message:my-ex/my-topic");

// This works and you end up with an instance with Exchange set to "my-queue" 
// and Topic set to "my-topic" 
// but with no query parameters
```

For a safe attempt at parsing:

```csharp
if (SendMessageUri.TryParse("publish-message:my-ex/my-topic", out var result))
{
    // do something with result now
};
```

