using GithubInfo.Services.Github.Interface;
using Octokit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GithubInfo.Services.Github
{
	public class GithubClientFactoryService : IGithubClientFactoryService
	{
		// 成功=true
		public bool TryCreateGithubClient(string token, out IGitHubClient? gitHub)
		{
			gitHub = default;
			try
			{
				gitHub = CreateGithubClient(token);
				_ = gitHub.User.Current().Result;
			}
			catch
			{
				return false;
			}
			return true;
		}

		public bool IsEnableToken(string token)
		{
			try
			{
				var gitHub = CreateGithubClient(token);
				_ = gitHub.User.Current().Result;
			}
			catch
			{
				return false;
			}
			return true;
		}

		public async ValueTask<bool> IsEnableTokenAsync(string token)
		{
			try
			{
				var gitHub = CreateGithubClient(token);
				_ = await gitHub.User.Current();
			}
			catch
			{
				return false;
			}
			return true;
		}

		public IGitHubClient CreateGithubClient(string token)
		{
			var client = new GitHubClient(new ProductHeaderValue("my-apps"));
			var tokenAuth = new Credentials(token);
			client.Credentials = tokenAuth;
			return client;
		}
	}
}
