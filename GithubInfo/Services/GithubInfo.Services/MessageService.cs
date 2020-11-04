using GithubInfo.Services.Interfaces;

namespace GithubInfo.Services
{
	public class MessageService : IMessageService
	{
		public string GetMessage()
		{
			return "Hello from the Message Service";
		}
	}
}
