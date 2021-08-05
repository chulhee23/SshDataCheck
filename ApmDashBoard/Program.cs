using System;
using System.Collections.Generic;
using System.Threading;
using Renci.SshNet;

namespace ApmDashBoard
{
	class MainClass
	{
		static Renci.SshNet.SshClient ssh;

		private static SshClient Connect_SSH(ConnectionInfo con)
		{
			try
			{
				SshClient cSSH = new SshClient(con);

				cSSH.ConnectionInfo.Timeout = TimeSpan.FromSeconds(120);
				cSSH.Connect();
				return cSSH;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
				return null;
			}
		}

		private static void recvCommSSHData(SshClient cli, string operation)
		{
			try
			{

				var output = cli.CreateCommand(operation).Execute();
				Console.WriteLine($"Output : {output}");

			}
			catch (Exception ex)
			{
				Console.WriteLine($"Err: {ex.Message}");
				Console.WriteLine($"Stack Trace : {ex.StackTrace}");
			}

			Thread.Sleep(200);
		}


		public static void Main(string[] args)
		{
			var pk = new PrivateKeyFile("res/my_pem_key.pem");
			var keyFiles = new[] { pk };
			var methods = new List<AuthenticationMethod>();
			methods.Add(new PrivateKeyAuthenticationMethod("chulhee", keyFiles));

			var con = new ConnectionInfo("13.125.52.86", 22, "chulhee", methods.ToArray());

			var cli = Connect_SSH(con);
			recvCommSSHData(cli, "ps -eo user,pid,ppid,rss,size,vsize,pmem,pcpu,time,cmd --sort -rss | head -n 11");
		}
	}
}
