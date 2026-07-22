#nullable enable
// The OpenAI.Responses namespace is [Experimental("OPENAI001")] in the SDK. We adopt it
// deliberately here (the Responses API is the supported path for reasoning models); this file is
// the only consumer, so the suppression is scoped to it rather than promoted to the csproj.
#pragma warning disable OPENAI001
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using OpenAI.Chat;
using OpenAI.Responses;

namespace Argumentum.AssetConverter;

public class Prompt
{
    private OpenAI.OpenAIClient? _openAIClient;
    private ChatClient? _chatClient;

    public string Model { get; set; } = "gpt-4.1-mini";

    public string ApiKey { get; set; } = "";

    public string? BaseUrl { get; set; }

    public int? MaxOutputTokens { get; set; }

    /// <summary>
    /// When true, route the call through the OpenAI Responses API (/v1/responses) instead of
    /// Chat Completions. Required for reasoning models (gpt-5.x): on Chat Completions they burn
    /// the token budget on hidden reasoning and return an empty Content, whereas /v1/responses
    /// honours <see cref="ReasoningEffort"/> (=low) and returns usable output. Default false keeps
    /// the legacy gpt-4.1 path unchanged.
    /// </summary>
    public bool UseResponsesApi { get; set; }

    /// <summary>
    /// Reasoning effort for the Responses API ("minimal"|"low"|"medium"|"high"). Null/empty = omit
    /// (server default). Use "low" for gpt-5.x translation tasks to cap reasoning-token spend.
    /// Only effective when <see cref="UseResponsesApi"/> is true.
    /// </summary>
    public string? ReasoningEffort { get; set; }

    public string SystemPrompt { get; set; } = "";

    public List<PromptExample> DialogPrompts { get; set; } = new();

    public string UserPrompt { get; set; } = "";

    public Action<string>? Tokenizer { get; set; }

    public ChatClient ChatClient
    {
        get
        {
            if (_chatClient == null)
            {
                if (!string.IsNullOrEmpty(BaseUrl))
                {
                    var options = new OpenAI.OpenAIClientOptions { Endpoint = new Uri(BaseUrl) };
                    _openAIClient = new OpenAI.OpenAIClient(new System.ClientModel.ApiKeyCredential(ApiKey), options);
                }
                else
                {
                    _openAIClient = new OpenAI.OpenAIClient(ApiKey);
                }
                _chatClient = _openAIClient.GetChatClient(Model);
            }
            return _chatClient;
        }
    }

    private ResponsesClient? _responsesClient;

    /// <summary>
    /// Lazy Responses-API client. Shares the underlying <see cref="_openAIClient"/> with
    /// <see cref="ChatClient"/> (creating it on first use when only the Responses path is taken).
    /// Unlike ChatClient, the Responses client is model-agnostic: the model is supplied per call
    /// via <see cref="CreateResponseOptions.Model"/>.
    /// </summary>
    public ResponsesClient ResponsesClient
    {
        get
        {
            if (_responsesClient == null)
            {
                _openAIClient ??= !string.IsNullOrEmpty(BaseUrl)
                    ? new OpenAI.OpenAIClient(new System.ClientModel.ApiKeyCredential(ApiKey),
                        new OpenAI.OpenAIClientOptions { Endpoint = new Uri(BaseUrl) })
                    : new OpenAI.OpenAIClient(ApiKey);
                _responsesClient = _openAIClient.GetResponsesClient();
            }
            return _responsesClient;
        }
    }

    public List<FunctionToolDef> Functions { get; set; } = new();

    public string? FunctionName { get; set; }

    public async Task<string> Send(CancellationToken cancellationToken, Action<string> log)
    {
        if (Tokenizer != null)
        {
            Tokenizer(SystemPrompt);
            if (DialogPrompts != null)
            {
                foreach (var dialogPrompt in DialogPrompts)
                {
                    Tokenizer(dialogPrompt.UserPrompt);
                    Tokenizer(dialogPrompt.AssistantAnswer);
                }
            }
            Tokenizer(UserPrompt);
        }

        if (UseResponsesApi)
        {
            return await SendViaResponses(cancellationToken, log);
        }

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(SystemPrompt)
        };

        if (DialogPrompts != null)
        {
            foreach (var dialogPrompt in DialogPrompts)
            {
                messages.Add(new UserChatMessage(dialogPrompt.UserPrompt));
                messages.Add(new AssistantChatMessage(dialogPrompt.AssistantAnswer));
            }
        }

        messages.Add(new UserChatMessage(UserPrompt));

        var options = new ChatCompletionOptions();

        if (MaxOutputTokens.HasValue)
        {
            options.MaxOutputTokenCount = MaxOutputTokens.Value;
        }

        if (Functions != null && Functions.Count > 0)
        {
            foreach (var func in Functions)
            {
                options.Tools.Add(func.ToChatTool());
            }

            if (FunctionName != null)
            {
                options.ToolChoice = ChatToolChoice.CreateFunctionChoice(FunctionName);
            }
        }

        var completion = await ChatClient.CompleteChatAsync(messages, options, cancellationToken);

        var chatMessage = completion.Value.Content.FirstOrDefault()?.Text;

        if (completion.Value.ToolCalls.Count > 0)
        {
            foreach (var toolCall in completion.Value.ToolCalls)
            {
                var result = CallFunction(toolCall.FunctionName, toolCall.FunctionArguments.ToString());
                log($"Function call {toolCall.FunctionName} with arguments {toolCall.FunctionArguments}");
            }
        }

        if (chatMessage != null)
        {
            if (Tokenizer != null)
            {
                Tokenizer(chatMessage);
            }
            return chatMessage;
        }

        return "";
    }

    /// <summary>
    /// Responses-API (/v1/responses) call path — mirrors <see cref="Send"/> but routes through
    /// <see cref="ResponsesClient"/> and, crucially, can set <see cref="ResponseReasoningOptions"/>
    /// (reasoning effort). This is the supported path for reasoning models (gpt-5.x); the Chat
    /// Completions path returns empty Content for them because it cannot cap reasoning-token spend.
    /// Reversible: only taken when <see cref="UseResponsesApi"/> is true (default false).
    /// </summary>
    private async Task<string> SendViaResponses(CancellationToken cancellationToken, Action<string> log)
    {
        var options = new CreateResponseOptions
        {
            Model = Model,
        };

        if (!string.IsNullOrEmpty(SystemPrompt))
        {
            options.InputItems.Add(ResponseItem.CreateSystemMessageItem(SystemPrompt));
        }

        if (DialogPrompts != null)
        {
            foreach (var dialogPrompt in DialogPrompts)
            {
                options.InputItems.Add(ResponseItem.CreateUserMessageItem(dialogPrompt.UserPrompt));
                options.InputItems.Add(ResponseItem.CreateAssistantMessageItem(dialogPrompt.AssistantAnswer));
            }
        }

        options.InputItems.Add(ResponseItem.CreateUserMessageItem(UserPrompt));

        if (MaxOutputTokens.HasValue)
        {
            options.MaxOutputTokenCount = MaxOutputTokens.Value;
        }

        if (!string.IsNullOrEmpty(ReasoningEffort))
        {
            options.ReasoningOptions = new ResponseReasoningOptions
            {
                ReasoningEffortLevel = ParseReasoningEffort(ReasoningEffort),
            };
        }

        if (Functions != null && Functions.Count > 0)
        {
            foreach (var func in Functions)
            {
                options.Tools.Add(new FunctionTool(
                    func.Name,
                    BinaryData.FromString(func.ParametersJson),
                    strictModeEnabled: null)
                {
                    FunctionDescription = func.Description,
                });
            }

            if (FunctionName != null)
            {
                options.ToolChoice = ResponseToolChoice.CreateFunctionChoice(FunctionName);
            }
        }

        var response = await ResponsesClient.CreateResponseAsync(options, cancellationToken);
        var result = response.Value;

        // Function-calling path: execute the requested tools (mirrors the Chat Completions branch
        // in Send). The Responses API returns each call as a FunctionCallResponseItem.
        if (result.OutputItems != null)
        {
            foreach (var item in result.OutputItems)
            {
                if (item is FunctionCallResponseItem functionCall)
                {
                    var callResult = CallFunction(functionCall.FunctionName, functionCall.FunctionArguments.ToString());
                    log($"Function call {functionCall.FunctionName} with arguments {functionCall.FunctionArguments}");
                }
            }
        }

        var outputText = result.GetOutputText();
        if (!string.IsNullOrEmpty(outputText))
        {
            if (Tokenizer != null)
            {
                Tokenizer(outputText);
            }
            return outputText;
        }

        return "";
    }

    /// <summary>
    /// Maps a config string to a <see cref="ResponseReasoningEffortLevel"/>. Unknown values fall
    /// back to Low (the intended default for cost-bounded translation runs) rather than throwing —
    /// a bad config value should not abort an entire translation campaign.
    /// </summary>
    internal static ResponseReasoningEffortLevel ParseReasoningEffort(string effort)
    {
        return effort.Trim().ToLowerInvariant() switch
        {
            "minimal" => ResponseReasoningEffortLevel.Minimal,
            "low" => ResponseReasoningEffortLevel.Low,
            "medium" => ResponseReasoningEffortLevel.Medium,
            "high" => ResponseReasoningEffortLevel.High,
            _ => ResponseReasoningEffortLevel.Low,
        };
    }

    private string CallFunction(string functionName, string argumentsJson)
    {
        var funcDef = Functions.FirstOrDefault(f => f.Name == functionName);
        if (funcDef == null || funcDef.TargetObject == null)
            return "function not found";

        var method = funcDef.TargetObject.GetType().GetMethod(funcDef.MethodName);
        if (method == null)
            return "method not found";

        using var doc = JsonDocument.Parse(argumentsJson);
        var args = new List<object?>();
        foreach (var param in method.GetParameters())
        {
            if (param.Name != null && doc.RootElement.TryGetProperty(param.Name, out var elem))
            {
                args.Add(elem.ValueKind == JsonValueKind.String
                    ? elem.GetString()
                    : elem.GetRawText());
            }
            else
            {
                args.Add(null);
            }
        }

        var result = method.Invoke(funcDef.TargetObject, args.ToArray());
        return result?.ToString() ?? "";
    }
}

public class FunctionToolDef
{
    public string Name { get; }
    public string Description { get; }
    public string ParametersJson { get; }
    public string MethodName { get; }
    public object TargetObject { get; set; }

    public FunctionToolDef(string name, string description, string methodName, string parametersJson)
    {
        Name = name;
        Description = description;
        MethodName = methodName;
        ParametersJson = parametersJson;
        TargetObject = null!;
    }

    public ChatTool ToChatTool()
    {
        return ChatTool.CreateFunctionTool(Name, Description, BinaryData.FromString(ParametersJson));
    }
}
