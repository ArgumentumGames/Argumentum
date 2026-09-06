using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Utility
{
	/// <summary>
	/// #1296 — transport hardening of GetDocumentPayload / GetCardSetDocument.
	/// Before the fix: no HttpClient timeout (100 s .NET default), no retry on transient
	/// failures, unbounded WaitAsync on the per-host semaphore, and a null payload from a
	/// failed download dereferenced as a NullReferenceException in CardSetInfo. A WAN blip
	/// of a few minutes cost 56 CardSets on 2026-09-06. These tests pin the new contract:
	/// transient failures are retried through, permanent ones fail fast without retry, and
	/// a failed CardSet download throws an error naming the URL and the DataSet.
	/// </summary>
	public class TransportHardeningTests
	{
		private static readonly TimeSpan[] FastDelays = { TimeSpan.FromMilliseconds(20), TimeSpan.FromMilliseconds(20), TimeSpan.FromMilliseconds(20) };

		[Fact]
		public async Task GetDocumentPayload_RetriesTransient503_AndSucceeds()
		{
			using var server = new FlakyServer(n => n < 3 ? (503, "unavailable") : (200, "{\"name\":\"ok\"}"));

			var payload = await server.Url.GetDocumentPayload(FastDelays);

			payload.Should().NotBeNull("the third attempt succeeds, so the transient 503s must be retried through");
			Encoding.UTF8.GetString(payload.Content).Should().Contain("\"ok\"");
			server.RequestCount.Should().Be(3);
		}

		[Fact]
		public async Task GetDocumentPayload_ReturnsNull_WhenTransientFailurePersistsAfterAllRetries()
		{
			using var server = new FlakyServer(_ => (503, "unavailable"));
			var delays = new[] { TimeSpan.FromMilliseconds(20), TimeSpan.FromMilliseconds(20) };

			var payload = await server.Url.GetDocumentPayload(delays);

			payload.Should().BeNull("a persistent failure must surface as null, not as an exception");
			server.RequestCount.Should().Be(3, "initial attempt + 2 retries, then give up");
		}

		[Fact]
		public async Task GetDocumentPayload_DoesNotRetry_Permanent404()
		{
			using var server = new FlakyServer(_ => (404, "not found"));

			var payload = await server.Url.GetDocumentPayload(new[] { TimeSpan.FromSeconds(30) });

			payload.Should().BeNull();
			server.RequestCount.Should().Be(1, "404 is permanent — a retry would only add a 30 s delay before the same verdict");
		}

		[Fact]
		public async Task GetCardSetDocument_FailedDownload_ThrowsNamingCardSetAndUrl()
		{
			using var server = new FlakyServer(_ => (404, "not found"));
			var cardSetInfo = new CardSetInfo
			{
				DataSet = KnownCardSets.Scenarii,
				JsonFilePathDebug = server.Url + "template.json",
				JsonFilePathRelease = server.Url + "template.json"
			};
			var config = new AssetConverterConfig
			{
				ForceDebugParams = true,
				WebBasedGeneratorConfig = new WebBasedGeneratorConfig()
			};

			var act = async () => await cardSetInfo.GetCardSetDocument(config);

			(await act.Should().ThrowAsync<InvalidOperationException>())
				.WithMessage($"*{KnownCardSets.Scenarii}*{server.Url}template.json*");
		}


		/// <summary>
		/// Minimal local HTTP server answering from a per-request responder, so the retry
		/// logic is exercised against a real HttpClient round-trip instead of a mock.
		/// </summary>
		private sealed class FlakyServer : IDisposable
		{
			private readonly HttpListener _listener;
			private readonly Task _loop;
			private int _requestCount;

			public string Url { get; }

			public int RequestCount => Volatile.Read(ref _requestCount);

			public FlakyServer(Func<int, (int Status, string Body)> responder)
			{
				var port = GetFreeTcpPort();
				Url = $"http://localhost:{port}/";
				_listener = new HttpListener();
				_listener.Prefixes.Add(Url);
				_listener.Start();
				_loop = Task.Run(async () =>
				{
					while (_listener.IsListening)
					{
						try
						{
							var context = await _listener.GetContextAsync();
							var requestNumber = Interlocked.Increment(ref _requestCount);
							var (statusCode, body) = responder(requestNumber);
							var buffer = Encoding.UTF8.GetBytes(body);
							context.Response.StatusCode = statusCode;
							context.Response.ContentType = "application/json";
							context.Response.ContentLength64 = buffer.Length;
							await context.Response.OutputStream.WriteAsync(buffer);
							context.Response.Close();
						}
						catch (Exception)
						{
							// listener stopped — exit the loop
						}
					}
				});
			}

			private static int GetFreeTcpPort()
			{
				var probe = new TcpListener(IPAddress.Loopback, 0);
				probe.Start();
				var port = ((IPEndPoint)probe.LocalEndpoint).Port;
				probe.Stop();
				return port;
			}

			public void Dispose()
			{
				try
				{
					_listener.Stop();
					_listener.Close();
				}
				catch (Exception)
				{
					// already closed
				}
				try
				{
					_loop.Wait(TimeSpan.FromSeconds(2));
				}
				catch (Exception)
				{
					// the loop task never outlives the listener by design
				}
			}
		}
	}
}
