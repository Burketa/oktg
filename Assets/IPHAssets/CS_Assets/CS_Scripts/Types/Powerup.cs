using UnityEngine;
using System;

namespace InfiniteHopper.Types
{
	[Serializable]
	public class Powerup
	{
		// The name of the function 
		public string startFunction = "SetScoreMultiplier";
		public float startParamater = 2;

		// The duration of this powerup. After it reaches 0, the end functions run
		public float duration = 10;
		internal float durationMax;

		// The name of the function 
		public string endFunction = "SetScoreMultiplier";
		public float endParamater = 1;

		// The icon of this powerup
		public Transform icon;
	}
}
