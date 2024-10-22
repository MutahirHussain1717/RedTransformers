using System;
using System.Collections.Generic;

public class MoPubMediationSetting : Dictionary<string, object>
{
	public MoPubMediationSetting(string adVendor)
	{
		base.Add("adVendor", adVendor);
	}
}
