using System.Threading.Tasks;

namespace Sample.Main
{
	public class SampleMain
	{
		public async Task<string> Get(DTO.Main.CheckBonusRequest request)
		{
			return "BonusCode not specified.";
		}
	}
}