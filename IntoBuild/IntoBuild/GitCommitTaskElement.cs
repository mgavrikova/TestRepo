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
using IntoBuild.Core;
using IntoBuild.Utils;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Evaluation;
using BuildResult = IntoBuild.Core.BuildResult;

namespace IntoBuild
{
	public class GitCommitTaskElement : ITaskElement
	{
		private const string CommitMessage = "DS-32 Git commit task test";
		private Guid _taskId = Guid.NewGuid();

		public Task Execute(BuildContext context)
		{
			var taskCompletionSource = new TaskCompletionSource<int>();
			var solutionFilePath = (string)context["SolutionFilePath"];
			var solutionFolder = Path.GetDirectoryName(solutionFilePath);

			try
			{
				var modifiedFiles = (List<string>)context["ModifiedFiles"];
				if (modifiedFiles == null || !modifiedFiles.Any())
				{
					taskCompletionSource.TrySetException(new BuildException(BuildResult.TaskElementError));
				}
				else
				{
					using (var repository = new Repository(solutionFolder))
					{
						foreach (var file in modifiedFiles)
						{
							repository.Index.Add(file);
						}
						var commit = repository.Commit(CommitMessage);
						if (!commit.IsValid)
						{
							taskCompletionSource.TrySetException(new BuildException(BuildResult.TaskElementError));
						}
						context[$"Commit{_taskId}"] = commit;
						taskCompletionSource.TrySetResult(0);
					}
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
