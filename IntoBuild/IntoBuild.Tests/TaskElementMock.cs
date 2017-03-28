using System;
using System.Threading.Tasks;
using IntoBuild.Core;

namespace IntoBuild.Tests
{
	class TaskElementMock : ITaskElement
	{
		private int _index;

		public TaskElementMock(int index)
		{
			_index = index;
		}

		public async Task Execute(BuildContext context)
		{
			Console.WriteLine($"Start task {_index}");
			await Task.Delay(1000);
			Console.WriteLine($"End task {_index}");
		}
	}
}