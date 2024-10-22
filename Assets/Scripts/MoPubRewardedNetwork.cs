using System;

public class MoPubRewardedNetwork
{
	private MoPubRewardedNetwork(string name)
	{
		this.name = name;
	}

	public override string ToString()
	{
		return this.name;
	}

	private readonly string name;

	public static readonly MoPubRewardedNetwork AdColony = new MoPubRewardedNetwork("com.mopub.mobileads.AdColonyRewardedVideo");

	public static readonly MoPubRewardedNetwork AdMob = new MoPubRewardedNetwork("com.mopub.mobileads.GooglePlayServicesRewardedVideo");

	public static readonly MoPubRewardedNetwork Chartboost = new MoPubRewardedNetwork("com.mopub.mobileads.ChartboostRewardedVideo");

	public static readonly MoPubRewardedNetwork Facebook = new MoPubRewardedNetwork("com.mopub.mobileads.FacebookRewardedVideo");

	public static readonly MoPubRewardedNetwork Tapjoy = new MoPubRewardedNetwork("com.mopub.mobileads.TapjoyRewardedVideo");

	public static readonly MoPubRewardedNetwork Unity = new MoPubRewardedNetwork("com.mopub.mobileads.UnityRewardedVideo");

	public static readonly MoPubRewardedNetwork Vungle = new MoPubRewardedNetwork("com.mopub.mobileads.VungleRewardedVideo");
}
