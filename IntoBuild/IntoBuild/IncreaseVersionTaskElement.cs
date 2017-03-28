using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using IntoBuild.Core;
using IntoBuild.Utils;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Evaluation;
using BuildResult = IntoBuild.Core.BuildResult;

namespace IntoBuild
{
	public class IncreaseVersionTaskElement : ITaskElement
	{
		private const string AssemblyInfoFile = "AssemblyInfo.cs";
		private const string AttributeGroup = "attribute";
		private const string VersionGroup = "version";
		private const string EndGroup = "end";
		private Regex _regex =
			new Regex(@"(?<attribute>Assembly(File){0,1}Version\(\x22)(?<version>([\d]{1,3}.){4})(?<end>\x22\))", RegexOptions.Compiled);

		public Task Execute(BuildContext context)
		{
			var taskCompletionSource = new TaskCompletionSource<int>();
			var solutionFilePath = (string)context["SolutionFilePath"];
			var modifiedFiles = (List<string>)context["ModifiedFiles"];
			if (modifiedFiles == null)
			{
				modifiedFiles = new List<string>();
			}
			try
			{
				foreach (var project in new ProjectEnumerable(solutionFilePath))
				{
					foreach (var assemblyInfo in project
						.GetItems("Compile")
						.Where(x => x.EvaluatedInclude.Contains(AssemblyInfoFile)))
					{
						var assemblyInfoPath = Path.Combine(project.DirectoryPath, assemblyInfo.EvaluatedInclude);
						var content = File.ReadAllText(assemblyInfoPath);
						var updatedContent = _regex.Replace(content, OnProcessVersionMatch);
						File.WriteAllText(assemblyInfoPath, updatedContent);
						modifiedFiles.Add(assemblyInfoPath);
					}
				}

				context["ModifiedFiles"] = modifiedFiles;
				taskCompletionSource.TrySetResult(0);
			}
			catch (Exception ex)
			{
				taskCompletionSource.TrySetException(ex);
			}
			return taskCompletionSource.Task;
		}

		private string OnProcessVersionMatch(Match match)
		{
			var versionString = match.Groups[VersionGroup].Value;
			Version version = Version.Parse(versionString);
			Version newVersion = new Version(version.Major, version.Minor, version.Build, version.Revision + 1);
			var result = match.Groups[AttributeGroup].Value + newVersion + match.Groups[EndGroup];
			return result;
		}
	}
}
