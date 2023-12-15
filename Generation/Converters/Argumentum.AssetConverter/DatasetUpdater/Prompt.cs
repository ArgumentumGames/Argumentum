using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

	public PromptExample Example { get; set; }

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


	public async Task<string> Send(CancellationToken cancellationToken)
	{
		if (Tokenizer != null)
		{
			Tokenizer(SystemPrompt);
			if (this.Example != null)
			{
				Tokenizer(Example.UserPrompt);
				Tokenizer(Example.Answer);
			}
			
			Tokenizer(UserPrompt);
		}

		var messages = new List<ChatMessage>
		{
			ChatMessage.FromSystem(SystemPrompt)
		};
		if (Example != null)
		{
			messages.Add(ChatMessage.FromUser(Example.UserPrompt));
			messages.Add(ChatMessage.FromAssistant(Example.Answer));
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
			//chatCompletionCreateRequest.ToolChoice = ToolChoice.FunctionChoice();
		}


		var completionResult = await OpenAiService.ChatCompletion.CreateCompletion(chatCompletionCreateRequest, cancellationToken: cancellationToken);
		

		if (completionResult.Successful)
		{
			var chatMessage = completionResult.Choices.First().Message;


			if (chatMessage.FunctionCall != null)
			{
				var functionCall = chatMessage.FunctionCall;
				var result = FunctionCallingHelper.CallFunction<bool>(functionCall, Functions.First(tuple => tuple.functionDefinition.Name == functionCall.Name).targetObject);
				chatMessage.Content = result.ToString(CultureInfo.CurrentCulture);
			}

			var messageContent = chatMessage.Content;

			if (Tokenizer != null)
			{
				Tokenizer(messageContent);
				
			}
			return messageContent;
		}
		throw new ApplicationException(completionResult.Error?.Message ?? "Unsuccessful");
	}

}