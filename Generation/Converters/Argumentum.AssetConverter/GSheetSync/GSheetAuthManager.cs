using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;

namespace Argumentum.AssetConverter.GSheetSync
{
	public class GSheetAuthManager
	{
		private readonly string _credentialsPath;
		private readonly string _refreshTokenPath;
		private readonly string[] _scopes = { SheetsService.Scope.Spreadsheets };

		public GSheetAuthManager(string credentialsPath, string refreshTokenPath)
		{
			_credentialsPath = credentialsPath;
			_refreshTokenPath = refreshTokenPath;
		}

		public async Task<UserCredential> GetCredentialAsync()
		{
			if (!File.Exists(_credentialsPath))
			{
				throw new FileNotFoundException(
					$"OAuth credentials file not found: {_credentialsPath}\n" +
					$"Create a text file with line 1 = client_id, line 2 = client_secret.");
			}

			var lines = await File.ReadAllLinesAsync(_credentialsPath);
			if (lines.Length < 2)
			{
				throw new InvalidDataException(
					$"OAuth credentials file must contain 2 lines: client_id and client_secret.\n" +
					$"File: {_credentialsPath}");
			}

			var clientId = lines[0].Trim();
			var clientSecret = lines[1].Trim();

			var tokenDir = Path.GetDirectoryName(_refreshTokenPath) ?? ".";
			var tokenFile = Path.GetFileNameWithoutExtension(_refreshTokenPath);

			// Use a FileDataStore pointing to the token directory
			// The FileDataStore creates a folder named after the "user" and stores tokens there
			var tokenStoreDir = Path.Combine(tokenDir, "GSheetTokens");
			Directory.CreateDirectory(tokenStoreDir);
			var dataStore = new FileDataStore(tokenStoreDir, true);

			var secrets = new ClientSecrets
			{
				ClientId = clientId,
				ClientSecret = clientSecret,
			};

			var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
				secrets,
				_scopes,
				"user",
				CancellationToken.None,
				dataStore);

			if (credential == null)
			{
				throw new InvalidOperationException("Failed to obtain Google OAuth credential.");
			}

			return credential;
		}
	}
}
