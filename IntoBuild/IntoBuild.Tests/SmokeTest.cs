using System;
using System.IO;
using System.Reflection;
using IntoBuild.Core;
using Intosoft.Builder;
using NUnit.Framework;

namespace IntoBuild.Tests
{
	public class SmokeTest
	{
		[Test]
		public void IncorrectParamsLaunch()
		{
			var strings = new string[0];
			var actualCode = Program.Main(strings);
			Assert.AreEqual(301, actualCode, ((BuildResult)actualCode).ToString());

			var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
			var pathToCurrentSolution = fileInfo.Directory.Parent.Parent.Parent.FullName;
			strings = new[]
			{
				Path.Combine(pathToCurrentSolution, "IntoBuild.txt")
			};
			actualCode = Program.Main(strings);
			Assert.AreEqual(301, actualCode, ((BuildResult)actualCode).ToString());
		}

		[Test]
		public void Launch()
		{
			var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
			var pathToCurrentSolution = fileInfo.Directory.Parent.Parent.Parent.FullName;
			var strings = new[]
			{
				Path.Combine(pathToCurrentSolution, "IntoBuild.sln")
			};
			Settings.Default.Timeout = TimeSpan.FromDays(1);
			var actualCode = Program.Main(strings);
			Assert.AreEqual(0, actualCode, ((BuildResult)actualCode).ToString());
		}

		[Test]
		public void CancelByTimeout()
		{
			var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
			var pathToCurrentSolution = fileInfo.Directory.Parent.Parent.Parent.FullName;
			var strings = new[]
			{
				Path.Combine(pathToCurrentSolution, "IntoBuild.sln")
			};
			Settings.Default.Timeout = TimeSpan.FromSeconds(3);
			var actualCode = Program.Main(strings);
			Assert.AreEqual(1, actualCode, ((BuildResult)actualCode).ToString());
		}
	}
}
