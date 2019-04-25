# Bot Framework V3 to V4 Migration Body of Knowledge
The following repository stores documentation and samples, recommended practices and other advise for migrating Bot Framework Bots from SDK V3 to SDK V4.

Some of the areas to be covered include:
* Netcore vs. Net Framework

    Bot Builder V3 is targeting netframework 4.6.  Bot Builder dotnet V4 is targeting .NET Standard 2.0.  This means it can be used in projects targeting netstandard2.0, netcoreapp2.0, netframework 4.6.1, etc.  Refer to https://github.com/dotnet/standard/blob/master/docs/versions.md for more version campatibility information.

    The Bot Framework dotnet sample projects are mostly demonstrating how to build bots targeting netcoreapp2.1: https://github.com/Microsoft/BotBuilder-Samples/tree/master/samples/csharp_dotnetcore  However, there is also an example targeting netframework 4.6.1: https://github.com/Microsoft/BotBuilder-Samples/tree/master/samples/csharp_webapi  

    We recommend upgrading your bot code to target netcore.

* Store Conversation

    The interface for persisting activity transcripts has changed.

    V3: IActivityLogger https://docs.microsoft.com/en-us/dotnet/api/microsoft.bot.builder.history.iactivitylogger
        method: LogAsync(IActivity)
        usage: builder.RegisterType<ActivityLoggerImplementation>().AsImplementedInterfaces().InstancePerDependency();


    V4: ITranscriptLogger https://docs.microsoft.com/en-us/dotnet/api/microsoft.bot.builder.itranscriptlogger
        method: LogActivityAsync(IActivity)
        usage:  var transcriptMiddleware = new TranscriptLoggerMiddleware(new MyTranscriptLoggerImplementation(Configuration.GetSection("StorageConnectionString").Value));
                adapter.Use(transcriptMiddleware);

* Cosmos Specific
* Blob specific
* SQL specific
* Form Flow: Get from community
* Dialog Type XYZ…
* Adaptive Cards 1.0, 1.1
* QnA Maker
* LUIS
* Move in/out of .bots, ARM templates, new cli's
