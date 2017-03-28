using System;
using System.IO;
using IntoBuild.Core;

namespace Intosoft.Builder
{
	internal static class ParametersParser
	{
		private const int ValidParametersCount = 1;

		public static BuildContext Parse(string[] args)
		{
			BuildContext result = null;
			if (args.Length == ValidParametersCount)
			{
				var solutionPath = args[0];
				var file = new FileInfo(solutionPath);
				if (file.Exists && file.Extension.Equals(".sln", StringComparison.OrdinalIgnoreCase))
				{
					result = new BuildContext {["SolutionFilePath"] = solutionPath};
				}
			}

			if (result == null)
			{
				throw new BuildException(BuildResult.InvalidParametersError);
			}

			return result;
		}
	}
}