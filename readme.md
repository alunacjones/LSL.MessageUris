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

// This works and you end up with an instance with DesctinationAddress set to "my-queue" but with no query parameters
```

For a safe attempt at parsing:

```csharp
if (SendMessageUri.TryParse(uri, out var result))
{
    // do something with result now
};
```

