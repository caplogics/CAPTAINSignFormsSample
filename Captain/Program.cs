using Captain.Common.Views.Forms;
using System;
using System.Threading.Tasks;
using Wisej.Web;

namespace Captain
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static async Task Main()
		{

			switch (await new LoginForm().ShowDialogAsync())
			{
				case DialogResult.OK:
					Application.MainPage = new MasterPage();
					break;


				case DialogResult.Yes:
					Application.MainPage = new MasterPage();
					break;
			}
		}

		//
		// You can use the entry method below
		// to receive the parameters from the URL in the args collection.
		//
		//static void Main(NameValueCollection args)
		//{
		//}
	}
}