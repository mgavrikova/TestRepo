using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntoBuild.Core;

namespace IntoBuild.Tests
{
	class TaskElementProviderMock : IEnumerable<ITaskElement>
	{
		public IEnumerator<ITaskElement> GetEnumerator()
		{
			for (int i = 0; i < 5; i++)
			{
				yield return new TaskElementMock(i);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
