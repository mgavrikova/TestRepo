using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using GitSharp;
using GitSharp.Commands;
using IntoBuild.Core;
using Intosoft.Builder;
using NUnit.Framework;

namespace IntoBuild.Tests
{
	[TestFixture]
	public class TaskRunnerTests
	{
		private BuildContext _context;

		[TestFixtureSetUp]
		public void Setup()
		{
			var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
			var pathToCurrentSolution = fileInfo.Directory.Parent.Parent.Parent.FullName;
			var solutionFilePath = Path.Combine(pathToCurrentSolution, "IntoBuild.sln");
			var ctx = new BuildContext();
			ctx["SolutionFilePath"] = solutionFilePath;
			_context = ctx;
		}

		[Test]
		public void FailedGitCommit()
		{
			var gitCommit = new GitCommitTaskElement();
			//Nothing to commit, throw exception
			var exception =
				Assert.Throws<BuildException>(async () => await gitCommit.Execute(_context)); 
			Assert.IsTrue(exception.Code == BuildResult.TaskElementError);
		}
		
		[Test, Ignore("Run the test with broken Internet connection")]
		public void FailedGitPush()
		{
			var runTasks = TaskElementRunner.RunTaskElements(new List<ITaskElement>()
				{
					new IncreaseVersionTaskElement(),
					new GitCommitTaskElement(),
					new GitPushTaskElement()
				}, _context);

			//No connection to remote branch, throw exception
			var exception =
				Assert.Throws<BuildException>(async () => await runTasks);
			Assert.IsTrue(exception.Code == BuildResult.TaskElementError);
		}
	}
}
