using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using GitSharp;
using GitSharp.Commands;
using GitSharp.Core.Transport;
using IntoBuild.Core;
using IntoBuild.Utils;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Evaluation;
using BuildResult = IntoBuild.Core.BuildResult;

namespace IntoBuild
{
	public class GitPushTaskElement : ITaskElement
	{
		public Task Execute(BuildContext context)
		{
			var taskCompletionSource = new TaskCompletionSource<int>();
			var solutionFilePath = (string)context["SolutionFilePath"];
			var solutionFolder = Path.GetDirectoryName(solutionFilePath);

			try
			{
				using (var repository = new Repository(solutionFolder))
				{
					PushCommand pushCommand = new PushCommand
					{
						
						Repository = repository
					};
					pushCommand.Execute();
					taskCompletionSource.TrySetResult(0);
				}
			}
			catch (Exception ex)
			{
				taskCompletionSource.TrySetException(ex);
			}
			return taskCompletionSource.Task;
		}
	}
}
