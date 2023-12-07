using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;


namespace Argumentum.AssetConverter;

public class Prompt
{

	private OpenAIService openAiService ;

	public string Model { get; set; } = Models.Gpt_3_5_Turbo;

	public string ApiKey { get; set; }

	public string SystemPrompt { get; set; }

	public PromptExample Example { get; set; }

	public string UserPrompt { get; set; }

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


	public async Task<string> Send()
	{
		Model = Models.Gpt_3_5_Turbo;
		var completionResult = await OpenAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
		{
			Messages = new List<ChatMessage>
			{
				ChatMessage.FromSystem(SystemPrompt),
				ChatMessage.FromUser(Example.UserPrompt),
				ChatMessage.FromAssistant(Example.Answer),
				ChatMessage.FromUser(UserPrompt)
			},
			Model = Model,
			//MaxTokens = 500//optional
		});
		if (completionResult.Successful)
		{
			return (completionResult.Choices.First().Message.Content);
		}
		throw new ApplicationException(completionResult.Error?.Message ?? "Unsuccessful");
	}

}