using System.IO;

namespace Argumentum.AssetConverter;

public class PromptExample
{
	private string _userPrompt;
	public string UserPromptPath { get; set; }

	public string AssistantAnswerPath { get; set; }

	public string UserPrompt
	{
		get
		{
			if (_userPrompt == null)
			{
				_userPrompt = File.ReadAllText(UserPromptPath);
			}
			return _userPrompt;
		}
	}

	private string _assistantAnswer;

	public string AssistantAnswer
	{
		get
		{
			if (_assistantAnswer == null)
			{
				_assistantAnswer = File.ReadAllText(AssistantAnswerPath);
			}
			return _assistantAnswer;
		}
	}

}