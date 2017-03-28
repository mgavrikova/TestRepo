using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using IntoBuild.Core;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using BuildResult = IntoBuild.Core.BuildResult;

namespace Intosoft.Builder
{
	internal static class Program
	{
		internal static int Main(string[] args)
		{
			BuildResult code = BuildResult.SuccessCode;

			try
			{
				var buildContext = ParametersParser.Parse(args);
				var project = ProjectSearcher.FindBuildProject(buildContext);
				var assemblyPath = ProjectCompiler.Compile(project.FullPath);
				var assembly = Assembly.LoadFile(assemblyPath);
				var definedType = TypeResolver.FindType(assembly);
				TaskElementRunner.Run(assembly, definedType, buildContext);
			}
			catch (BuildException ex)
			{
				if (!String.IsNullOrEmpty(ex.Message))
				{
					Console.Error.WriteLine(ex.Message);
				}

				code = ex.Code;
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine(ex);
				code = BuildResult.UnknownError;
			}

			return (int)code;
		}
	}
}
