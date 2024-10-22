using System;
using UnityEngine;

[Serializable]
public class StopCAM : MonoBehaviour
{
	public StopCAM()
	{
		this.myCheck = true;
	}

	public virtual bool OnMouseDown()
	{
		bool result;
		if (this.myCheck)
		{
			this.camAnim.GetComponent<Animation>().Stop();
			this.myCheck = false;
			result = this.myCheck;
		}
		else if (!this.myCheck)
		{
			this.camAnim.GetComponent<Animation>().Play();
			this.myCheck = true;
			result = this.myCheck;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public virtual void Main()
	{
	}

	public bool myCheck;

	public Animation camAnim;
}
