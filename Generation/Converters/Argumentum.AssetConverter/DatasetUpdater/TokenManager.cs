using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using SharpToken;

namespace Argumentum.AssetConverter;

public class TokenManager
{
	private readonly int maxTokensPerMinute;
	private readonly ConcurrentQueue<(int tokenCount, DateTime timestamp)> tokenTimestamps;
	private readonly GptEncoding encoding;

	public TokenManager(int maxTokensPerMinute, string model)
	{
		this.maxTokensPerMinute = maxTokensPerMinute;
		this.encoding = GptEncoding.GetEncodingForModel(model);
		tokenTimestamps = new ConcurrentQueue<(int, DateTime)>();
	}

	public Action<string> TokenizerAction => Tokenize;

	private void Tokenize(string text)
	{
		var tokenCount = encoding.Encode(text).Count;
		tokenTimestamps.Enqueue((tokenCount, DateTime.UtcNow));
		CleanupTokens();
	}

	public void WaitForTokenAvailability()
	{
		while (GetTotalTokenCount() >= maxTokensPerMinute)
		{
			CleanupTokens();
			Logger.Log($"Waiting for token availability. Current token count: {GetTotalTokenCount()}");
			Task.Delay(5000).Wait(); // Attendre 1 seconde avant de réessayer
		}
		Logger.Log($"Tokens over last minute: {GetTotalTokenCount()}");
	}

	private void CleanupTokens()
	{
		while (tokenTimestamps.TryPeek(out var item) && (DateTime.UtcNow - item.timestamp).TotalMinutes >= 1)
		{
			tokenTimestamps.TryDequeue(out _);
		}
	}

	private int GetTotalTokenCount()
	{
		CleanupTokens();
		return tokenTimestamps.Sum(item => item.tokenCount);
	}
}