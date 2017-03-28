using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using IntoBuild.Core;
using Microsoft.Build.Execution;
using BuildResult = IntoBuild.Core.BuildResult;

namespace Intosoft.Builder
{
	static class ProjectCompiler
	{
		private const string BuildTargets = "Build";

		public static string Compile(string projectPath)
		{
			BuildManager manager = BuildManager.DefaultBuildManager;

			ProjectInstance projectInstance = new ProjectInstance(projectPath);
			var buildParameters = new BuildParameters()
			{
				DetailedSummary = true
			};
			var buildRequestData = new BuildRequestData(projectInstance, new[] { BuildTargets });

			string outputPath = null;
			var result = manager.Build(buildParameters, buildRequestData);
			if (result.OverallResult == BuildResultCode.Success)
			{
				var buildResult = result.ResultsByTarget[BuildTargets];
				var buildResultItems = buildResult.Items[0];
				outputPath = buildResultItems.ItemSpec;
			}

			if (String.IsNullOrEmpty(outputPath))
			{
				throw new BuildException(BuildResult.CompilationError);
			}

			return outputPath;
		}
	}
}
