# Bot Framework V3 to V4 Migration Body of Knowledge
The following repository stores documentation and samples, recommended practices and other advise for migrating Bot Framework Bots from SDK V3 to SDK V4.

Some of the areas to be covered include:
## Netcore vs. Net Framework

Bot Builder V3 is targeting netframework 4.6.  Bot Builder dotnet V4 is targeting .NET Standard 2.0.  This means it can be used in projects targeting netstandard2.0, netcoreapp2.0, netframework 4.6.1, and above etc.  Refer to https://github.com/dotnet/standard/blob/master/docs/versions.md for more version campatibility information.

The Bot Framework dotnet sample projects are mostly demonstrating how to build bots targeting netcoreapp2.1: https://github.com/Microsoft/BotBuilder-Samples/tree/master/samples/csharp_dotnetcore  We recommend upgrading your bot code to target netcore.  However, we also provide an example targeting netframework 4.6.1: https://github.com/Microsoft/BotBuilder-Samples/tree/master/samples/csharp_webapi  


## Store Conversation

The interface for persisting activity transcripts has changed.

### V3: 
`IActivityLogger` https://docs.microsoft.com/en-us/dotnet/api/microsoft.bot.builder.history.iactivitylogger
    
Method: `LogAsync(IActivity)`
        
Usage:

```csharp
builder.RegisterType<ActivityLoggerImplementation>().AsImplementedInterfaces().InstancePerDependency(); 
```

### V4: 
`ITranscriptLogger` https://docs.microsoft.com/en-us/dotnet/api/microsoft.bot.builder.itranscriptlogger
        
Method: `LogActivityAsync(IActivity)`
        
Usage:

```csharp
var transcriptMiddleware = new TranscriptLoggerMiddleware(new MyTranscriptLoggerImplementation(Configuration.GetSection("StorageConnectionString").Value));
adapter.Use(transcriptMiddleware);
```
## State Storage


### V3: 
The interface for storing state (UserData, ConversationData, PrivateConversationData) has changed.  In V3, state was implemented using an IBotDataStore implementation, and injecting it into the dialog state system of the sdk using autofac.  Microsoft provided MemoryStorage, DocumentDbBotDataStore, TableBotDataStore and SqlBotDataStore for V3 in Microsoft.Bot.Builder.Azure https://github.com/Microsoft/BotBuilder-Azure/  

`IBotDataStore<BotData>`

```csharp
Task<bool> FlushAsync(IAddress key, CancellationToken cancellationToken);
Task<T> LoadAsync(IAddress key, BotStoreType botStoreType, CancellationToken cancellationToken);
Task SaveAsync(IAddress key, BotStoreType botStoreType, T data, CancellationToken cancellationToken);
```

Usage: 

```csharp   
builder.Register(c => new DocumentDbBotDataStore(docDbEmulatorUri, docDbEmulatorKey))
                .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                .AsSelf()
                .SingleInstance();
```
    
### V4: 
`IStorage`

The V4 sdk uses an IStorage interface, the implementation of which is provided to a BotState object (UserState, ConversationState, PrivateConversationState).  The BotState provides keys to the underlying IStorage, and acts as a property manager (IPropertyManager.CreateProperty<T>(string name)).  

```csharp
Task DeleteAsync(string[] keys, CancellationToken cancellationToken = default(CancellationToken));
Task<IDictionary<string, object>> ReadAsync(string[] keys, CancellationToken cancellationToken = default(CancellationToken));
Task WriteAsync(IDictionary<string, object> changes, CancellationToken cancellationToken = default(CancellationToken));
```

Usage: 
```charp
var storageOptions = new CosmosDbStorageOptions()
{
    AuthKey = "YourAuthKey",
    CollectionId = "v4Container",
    CosmosDBEndpoint = new Uri("https://YourCosmosDb.documents.azure.com:443/"),
    DatabaseId = "botdbv4"
};
IStorage dataStore = new CosmosDbStorage(storageOptions);
var conversationState = new ConversationState(dataStore);
services.AddSingleton(conversationState);

```

* Cosmos Specific

    V3: Microsoft.Bot.Builder.Azure.DocumentDbBotDataStore

    V4: Microsoft.Bot.Builder.Azure.CosmosDbStorage


* Table/Blob specific
    V3: Microsoft.Bot.Builder.Azure.TableBotDataStore

    V4: Microsoft.Bot.Builder.Azure.AzureBlobStorage

* SQL specific
    V3: Microsoft.Bot.Builder.Azure.SqlBotDataStore

    V4: Bot.Builder.Community.Storage.EntityFramework

* Form Flow
    V3: Microsoft.Bot.Builder.FormFlow

    V4: Bot.Builder.Community.Dialogs.FormFlow

* Dialog Type XYZ…


* Adaptive Cards 1.0, 1.1


* QnA Maker

* LUIS

* Move in/out of .bots, ARM templates, new cli's

