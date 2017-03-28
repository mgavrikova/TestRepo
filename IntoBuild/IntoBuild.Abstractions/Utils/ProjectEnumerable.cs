using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using IntoBuild.Core;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Evaluation;
using Project = Microsoft.Build.Evaluation.Project;

namespace IntoBuild.Utils
{
	public class ProjectEnumerable : IEnumerable<Project>
	{
		private readonly string _solutionFilePath;

		public ProjectEnumerable(string solutionFilePath)
		{
			this._solutionFilePath = solutionFilePath;
		}

		public IEnumerator<Project> GetEnumerator()
		{
			string wrapperContent = SolutionWrapperProject.Generate(
				_solutionFilePath, toolsVersionOverride: null, projectBuildEventContext: null);
			byte[] rawWrapperContent = Encoding.Unicode.GetBytes(wrapperContent.ToCharArray());
			using (MemoryStream memStream = new MemoryStream(rawWrapperContent))
			{
				using (XmlTextReader xmlReader = new XmlTextReader(memStream))
				{
					var proj = ProjectCollection.GlobalProjectCollection.LoadProject(xmlReader);
					var solutionFolder = Path.GetDirectoryName(_solutionFilePath);
					foreach (ProjectItem projectItem in GetOrderedProjectReferencesFromWrapper(proj))
					{
						string projectPath = Path.Combine(solutionFolder, projectItem.EvaluatedInclude);
						var currentProject = ProjectCollection.GlobalProjectCollection.LoadProject(projectPath);
						yield return currentProject;
					}
					ProjectCollection.GlobalProjectCollection.UnloadAllProjects();
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private static IEnumerable<ProjectItem> GetOrderedProjectReferencesFromWrapper(Project solutionWrapperProject)
		{
			int buildLevel = 0;
			while (true)
			{
				var items = solutionWrapperProject.GetItems(String.Format("BuildLevel{0}", buildLevel));
				if (items.Count == 0)
				{
					yield break;
				}
				foreach (var item in items)
				{
					yield return item;
				}
				buildLevel++;
			}
		}
	}
}
