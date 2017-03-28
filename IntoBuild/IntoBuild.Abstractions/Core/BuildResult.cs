namespace IntoBuild.Core
{
	public enum BuildResult
	{
		SuccessCode = 0,
		ExecutionCancelled = 1,
		UnknownError = 2,
		InvalidParametersError = 301,
		BuildProjectNotFoundError = 302,
		CompilationError = 303,
		BuildProjectIncorrectStructureError = 304,
		TaskElementError = 305
	}
}