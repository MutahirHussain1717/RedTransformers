using System;
using System.Collections.Generic;
using GoogleMobileAds.Api.Mediation;

namespace GoogleMobileAds.Api
{
	public class AdRequest
	{
		private AdRequest(AdRequest.Builder builder)
		{
			this.TestDevices = new List<string>(builder.TestDevices);
			this.Keywords = new HashSet<string>(builder.Keywords);
			this.Birthday = builder.Birthday;
			this.Gender = builder.Gender;
			this.TagForChildDirectedTreatment = builder.ChildDirectedTreatmentTag;
			this.Extras = new Dictionary<string, string>(builder.Extras);
			this.MediationExtras = builder.MediationExtras;
		}

		public List<string> TestDevices { get; private set; }

		public HashSet<string> Keywords { get; private set; }

		public DateTime? Birthday { get; private set; }

		public Gender? Gender { get; private set; }

		public bool? TagForChildDirectedTreatment { get; private set; }

		public Dictionary<string, string> Extras { get; private set; }

		public List<MediationExtras> MediationExtras { get; private set; }

		public const string Version = "3.6.3";

		public const string TestDeviceSimulator = "SIMULATOR";

		public class Builder
		{
			public Builder()
			{
				this.TestDevices = new List<string>();
				this.Keywords = new HashSet<string>();
				this.Birthday = null;
				this.Gender = null;
				this.ChildDirectedTreatmentTag = null;
				this.Extras = new Dictionary<string, string>();
				this.MediationExtras = new List<MediationExtras>();
			}

			internal List<string> TestDevices { get; private set; }

			internal HashSet<string> Keywords { get; private set; }

			internal DateTime? Birthday { get; private set; }

			internal Gender? Gender { get; private set; }

			internal bool? ChildDirectedTreatmentTag { get; private set; }

			internal Dictionary<string, string> Extras { get; private set; }

			internal List<MediationExtras> MediationExtras { get; private set; }

			public AdRequest.Builder AddKeyword(string keyword)
			{
				this.Keywords.Add(keyword);
				return this;
			}

			public AdRequest.Builder AddTestDevice(string deviceId)
			{
				this.TestDevices.Add(deviceId);
				return this;
			}

			public AdRequest Build()
			{
				return new AdRequest(this);
			}

			public AdRequest.Builder SetBirthday(DateTime birthday)
			{
				this.Birthday = new DateTime?(birthday);
				return this;
			}

			public AdRequest.Builder SetGender(Gender gender)
			{
				this.Gender = new Gender?(gender);
				return this;
			}

			public AdRequest.Builder AddMediationExtras(MediationExtras extras)
			{
				this.MediationExtras.Add(extras);
				return this;
			}

			public AdRequest.Builder TagForChildDirectedTreatment(bool tagForChildDirectedTreatment)
			{
				this.ChildDirectedTreatmentTag = new bool?(tagForChildDirectedTreatment);
				return this;
			}

			public AdRequest.Builder AddExtra(string key, string value)
			{
				this.Extras.Add(key, value);
				return this;
			}
		}
	}
}
