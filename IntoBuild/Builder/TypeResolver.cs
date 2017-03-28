using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IntoBuild.Core;
using BuildResult = IntoBuild.Core.BuildResult;

namespace Intosoft.Builder
{
	static class TypeResolver
	{
		public static Type FindType(Assembly assembly)
		{
			var types = assembly.GetTypes();
			var definedType = types
				.FirstOrDefault(x => x.GetInterfaces().Any(r => r == typeof(IEnumerable<ITaskElement>)));

			if (definedType == null)
			{
				throw new BuildException(BuildResult.BuildProjectIncorrectStructureError);
			}

			return definedType;
		}
	}
}
