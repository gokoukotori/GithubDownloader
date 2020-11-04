using Octokit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GithubInfo.Services.Github.Interface
{
	public interface IGithubClientFactoryService
	{
		public bool TryCreateGithubClient(string token, out IGitHubClient gitHub);
		public bool IsEnableToken(string token);
		public ValueTask<bool> IsEnableTokenAsync(string token);
		public IGitHubClient CreateGithubClient(string token);
	}
}
