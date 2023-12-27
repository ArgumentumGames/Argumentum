using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Playwright;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.Utilities.FunctionCalling;


namespace Argumentum.AssetConverter;

public class Prompt
{

	private OpenAIService openAiService ;

	public string Model { get; set; } = Models.Gpt_3_5_Turbo_1106;

	public string ApiKey { get; set; }

	public string SystemPrompt { get; set; }

	public List<PromptExample> DialogPrompts { get; set; } = new();

	public string UserPrompt { get; set; }


	public Action<string> Tokenizer { get; set; }

	public OpenAIService OpenAiService
	{
		get
		{
			if (openAiService == null)
			{
				openAiService = new OpenAIService(new OpenAiOptions()
				{
					ApiKey = ApiKey
				});
			}
			return openAiService;
		}
	}

	public List<(FunctionDefinition functionDefinition, object targetObject)> Functions { get; set; }

	public string FunctionName { get; set; }

	public async Task<string> Send(CancellationToken cancellationToken)
	{
		if (Tokenizer != null)
		{
			Tokenizer(SystemPrompt);
			if (this.DialogPrompts != null)
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
			ChatMessage.FromSystem(SystemPrompt)
		};
		if (this.DialogPrompts != null)
		{
			foreach (var dialogPrompt in DialogPrompts)
			{
				messages.Add(ChatMessage.FromUser(dialogPrompt.UserPrompt));
				messages.Add(ChatMessage.FromAssistant(dialogPrompt.AssistantAnswer));
			}
		}
			
		messages.Add(ChatMessage.FromUser(UserPrompt));

		var chatCompletionCreateRequest = new ChatCompletionCreateRequest
		{
			
			Messages = messages,
			Model = Model,
			//MaxTokens = 500//optional
		};
		if (Functions != null)
		{
			chatCompletionCreateRequest.Tools = Functions.Select(tuple =>  ToolDefinition.DefineFunction(tuple.functionDefinition)).ToList();
			//chatCompletionCreateRequest.ChatResponseFormat = ChatCompletionCreateRequest.ResponseFormats.Json;
			if (FunctionName != null)
			{
				chatCompletionCreateRequest.ToolChoice = ToolChoice.FunctionChoice(FunctionName);
			}
		}


		var completionResult = await OpenAiService.ChatCompletion.CreateCompletion(chatCompletionCreateRequest, cancellationToken: cancellationToken);
		

		if (completionResult.Successful)
		{
			var chatMessage = completionResult.Choices.First().Message;


			if (chatMessage.FunctionCall != null)
			{
				var functionCall = chatMessage.FunctionCall;
				var result = CallFunction(functionCall);
				//chatMessage.Content = result.ToString(CultureInfo.CurrentCulture);
			}

			if (chatMessage.ToolCalls != null && chatMessage.ToolCalls.Count > 0)
			{
				foreach (var chatMessageToolCall in chatMessage.ToolCalls)
				{
					var functionCall = chatMessageToolCall.FunctionCall;
					var result = CallFunction(functionCall);
				}
			}

			if (chatMessage.Content != null)
			{
				var messageContent = chatMessage.Content;

				if (Tokenizer != null)
				{
					Tokenizer(messageContent);

				}
				return messageContent;
			}
			return "";
		}
		throw new ApplicationException(completionResult.Error?.Message ?? "Unsuccessful");
	}

	private string CallFunction(FunctionCall functionCall)
	{
		var result = FunctionCallingHelper.CallFunction<string>(functionCall,
			Functions.First(tuple => tuple.functionDefinition.Name == functionCall.Name).targetObject);
		return result;
	}
}