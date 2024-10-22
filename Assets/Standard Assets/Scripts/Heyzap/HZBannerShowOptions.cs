using System;

namespace Heyzap
{
	public class HZBannerShowOptions : HZShowOptions
	{
		public string Position
		{
			get
			{
				return this.position;
			}
			set
			{
				if (value == "top" || value == "bottom")
				{
					this.position = value;
				}
			}
		}

		public const string POSITION_TOP = "top";

		public const string POSITION_BOTTOM = "bottom";

		private const string DEFAULT_POSITION = "bottom";

		private string position = "bottom";
	}
}
