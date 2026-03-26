using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using SharpToken;

namespace Argumentum.AssetConverter;

public class TokenManager
{
	
	private readonly ConcurrentQueue<(int tokenCount, DateTime timestamp)> tokenTimestamps;
	private readonly GptEncoding encoding;
	public int MillisecondsDelay { get; set; } = 5000;
	public int MaxTokensPerMinute { get; set; }

	public int CurrentMinuteTokenCount
	{
		get
		{
			CleanupTokens();
			return tokenTimestamps.Sum(item => item.tokenCount);
		}
	}

	public int TotalMaxTokens { get; set; } = 0;

	public TokenManager(int maxTokensPerMinute, string model)
	{
		this.MaxTokensPerMinute = maxTokensPerMinute;
		this.encoding = GptEncoding.GetEncodingForModel(model);
		tokenTimestamps = new ConcurrentQueue<(int, DateTime)>();
	}

	public Action<string> TokenizerAction => Tokenize;

	private void Tokenize(string text)
	{
		var tokenCount = encoding.Encode(text).Count;
		TotalMaxTokens += tokenCount;
		tokenTimestamps.Enqueue((tokenCount, DateTime.UtcNow));
		CleanupTokens();
	}

	public void WaitForTokenAvailability()
	{
		while (CurrentMinuteTokenCount >= MaxTokensPerMinute)
		{
			CleanupTokens();
			Logger.Log($"Waiting for token availability. Current token count: {CurrentMinuteTokenCount}");
			Task.Delay(MillisecondsDelay).Wait();
		}
		Logger.Log($"Tokens over last minute: {CurrentMinuteTokenCount}");
	}

	private void CleanupTokens()
	{
		while (tokenTimestamps.TryPeek(out var item) && (DateTime.UtcNow - item.timestamp).TotalMinutes >= 1)
		{
			tokenTimestamps.TryDequeue(out _);
		}
	}

	
}