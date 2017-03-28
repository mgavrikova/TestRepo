using System.Threading.Tasks;

namespace IntoBuild.Core
{
	public interface ITaskElement
	{
		Task Execute(BuildContext context);
	}
}
