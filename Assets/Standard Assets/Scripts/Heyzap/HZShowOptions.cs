using System;

namespace Heyzap
{
	public class HZShowOptions
	{
		public string Tag
		{
			get
			{
				return this.tag;
			}
			set
			{
				if (value != null)
				{
					this.tag = value;
				}
				else
				{
					this.tag = "default";
				}
			}
		}

		private string tag = "default";
	}
}
