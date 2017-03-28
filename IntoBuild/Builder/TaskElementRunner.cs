using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IntoBuild.Core;
using BuildResult = IntoBuild.Core.BuildResult;

namespace Intosoft.Builder
{
	public class TaskElementRunner
	{
		internal static void Run(Assembly assembly, Type type, BuildContext buildContext)
		{
			try
			{
				var taskElementProvider = (IEnumerable<ITaskElement>)assembly.CreateInstance(type.FullName);
				var task = RunTaskElements(taskElementProvider, buildContext);
				var cancellationToken = new CancellationTokenSource(Settings.Default.Timeout);
				task.Wait(cancellationToken.Token);
			}
			catch (AggregateException ex)
			{
				if (ex.InnerException != null)
				{
					if (ex.InnerException is BuildException)
					{
						throw ex.InnerException;
					}
					throw new BuildException(BuildResult.TaskElementError, ex.InnerException.Message);
				}
				throw;
			}
			catch (OperationCanceledException)
			{
				throw new BuildException(BuildResult.ExecutionCancelled); 
			}
		}

		public static async Task RunTaskElements(IEnumerable<ITaskElement> taskElementProvider, BuildContext context)
		{
			foreach (var taskElement in taskElementProvider)
			{
				await taskElement.Execute(context);
			}
		}
	}
}
