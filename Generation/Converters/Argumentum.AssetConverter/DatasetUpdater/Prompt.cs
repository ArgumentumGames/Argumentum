#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using OpenAI.Chat;

namespace Argumentum.AssetConverter;

public class Prompt
{
    private OpenAI.OpenAIClient? _openAIClient;
    private ChatClient? _chatClient;

    public string Model { get; set; } = "gpt-4.1-mini";

    public string ApiKey { get; set; } = "";

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
                _openAIClient = new OpenAI.OpenAIClient(ApiKey);
                _chatClient = _openAIClient.GetChatClient(Model);
            }
            return _chatClient;
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
