using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DTO
{
    public class Main
	{
		[Route("/player/checkbonus")]
		public class CheckBonusRequest
		{
			public string Brand { get; set; }
			public string BonusCode { get; set; }
		}
	}
}
