using System;
using UnityEngine;

[Serializable]
public class VisibleBG : MonoBehaviour
{
	public VisibleBG()
	{
		this.myCheck = true;
	}

	public virtual bool OnMouseDown()
	{
		bool result;
		if (this.myCheck)
		{
			this.BG.SetActiveRecursively(false);
			this.myCheck = false;
			result = this.myCheck;
		}
		else
		{
			if (!this.myCheck)
			{
				this.BG.SetActiveRecursively(true);
				this.myCheck = true;
			}
			result = this.myCheck;
		}
		return result;
	}

	public virtual void Main()
	{
	}

	public bool myCheck;

	public GameObject BG;
}
