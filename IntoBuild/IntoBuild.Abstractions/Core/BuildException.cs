using System;

namespace IntoBuild.Core
{
	public class BuildException : Exception
	{
		public BuildException(BuildResult code) 
		{
			Code = code;
		}

		public BuildException(BuildResult code, string message) : base(message)
		{
			Code = code;
		}

		public BuildResult Code { get; set; }
	}
}
