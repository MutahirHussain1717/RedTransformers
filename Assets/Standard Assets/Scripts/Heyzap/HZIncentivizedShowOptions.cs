using System;

namespace Heyzap
{
	public class HZIncentivizedShowOptions : HZShowOptions
	{
		public string IncentivizedInfo
		{
			get
			{
				return this.incentivizedInfo;
			}
			set
			{
				if (value != null)
				{
					this.incentivizedInfo = value;
				}
				else
				{
					this.incentivizedInfo = string.Empty;
				}
			}
		}

		private const string DEFAULT_INCENTIVIZED_INFO = "";

		private string incentivizedInfo = string.Empty;
	}
}
