# Argumentation ChatGPT plugin

This project is the submission to a [semantic-kernel hackathon](https://github.com/microsoft/semantic-kernel-plugins) to create a ChatGPT plugin. It includes the following components:
- An endpoint that serves up an ai-plugin.json file for ChatGPT to discover the plugin. It exposes a series of native functions and automatically builds a series of semantic ones. 
- A generator that automatically converts prompts into semantic function endpoints
- The ability to add additional native functions as endpoints to the plugin.

Plugin is getting tested in one of the modes available with the AssetConverter project, several tools of which are used by this plugin for informal analysis.

To learn more about using this starter, see the Semantic Kernel documentation that describes how to [create a ChatGPT plugin](https://learn.microsoft.com/en-us/semantic-kernel/ai-orchestration/chatgpt-plugins).

## Argumentation Features

- The plugin offers several [semantic](/azure-function/Prompts) and [native](/azure-function/ArgumentationPlugin.cs) function for  argumentation analysis of a text, typically a transcript.
- A set of semantic and search-augmented semi-semantic functions are doing informal analysis based on matching text content to branches of the large taxonomy of Fallacies that this whole repository is about.
- A set of native and semantic methods offer formal reasoning over the text powered by [Tweety project](http://tweetyproject.org/). A pipeline for belief set translation (semantic), query crafting (semantic), query execution (native - Tweety), and results intepretation (semantic) is available in 3 declinations for propositional, first order and modal logics.  


## Prerequisites

- [.NET 6](https://dotnet.microsoft.com/download/dotnet/6.0) is required to run this starter.
- [Azure Functions Core Tools](https://www.npmjs.com/package/azure-functions-core-tools) is required to run this starter.
- Install the recommended extensions
  - [C#](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
  - [Semantic Kernel Tools](https://marketplace.visualstudio.com/items?itemName=ms-semantic-kernel.semantic-kernel)

## Configuring the plugin

To configure the starter, you need to provide the following information:

- Define the properties of the plugin in the [appsettings.json](./azure-function/appsettings.json) file.
- Enter the API key for your AI endpoint in the [local.settings.json](./azure-function/local.settings.json) file.

For Debugging the console application alone, we suggest using .NET [Secret Manager](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) to avoid the risk of leaking secrets into the repository, branches and pull requests.


### Using appsettings.json

Configure an OpenAI endpoint

1. Copy [settings.json.openai-example](./config/appsettings.json.openai-example) to `./appsettings.json`
1. Edit the `kernel` object to add your OpenAI endpoint configuration
1. Edit the `aiPlugin` object to define the properties that get exposed in the ai-plugin.json file

Configure an Azure OpenAI endpoint

1. Copy [settings.json.azure-example](./config/appsettings.json.azure-example) to `./appsettings.json`
1. Edit the `kernel` object to add your Azure OpenAI endpoint configuration
1. Edit the `aiPlugin` object to define the properties that get exposed in the ai-plugin.json file

### Using local.settings.json

1. Copy [local.settings.json.example](./azure-function/local.settings.json.example) to `./azure-function/local.settings.json`
1. Edit the `Values` object to add your OpenAI endpoint configuration in the `apiKey` property

## Running the plugin

To run the Azure Functions application just hit `F5`.

To build and run the Azure Functions application from a terminal use the following commands:

```powershell
cd azure-function
dotnet build
func start --csharp
```
