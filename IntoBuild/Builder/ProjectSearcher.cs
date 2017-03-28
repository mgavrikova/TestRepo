using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using IntoBuild.Core;
using IntoBuild.Utils;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Evaluation;
using Project = Microsoft.Build.Evaluation.Project;
using BuildResult = IntoBuild.Core.BuildResult;

namespace Intosoft.Builder
{
	internal static class ProjectSearcher
	{
		private const string BuildProjectReference = "IntoBuild";

		public static Project FindBuildProject(BuildContext buildContext)
		{
			Project foundProject = null;

			var solutionFilePath = (string)buildContext["SolutionFilePath"];
			foreach (var project in new ProjectEnumerable(solutionFilePath))
			{
				var refs = project.GetItems("Reference");
				if (refs.Any(r => r.EvaluatedInclude.Contains(BuildProjectReference)))
				{
					foundProject = project;
					break;
				}
			}
			
			if (foundProject == null)
			{
				throw new BuildException(BuildResult.BuildProjectNotFoundError);
			}

			return foundProject;
		}
	}
}
