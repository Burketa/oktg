using UnityEngine;
using System;

namespace InfiniteHopper.Types
{
	//This class defines the background elements which are animated to loop

	[Serializable]
	public class LoopingBackground 
	{
		//The object that will loop. This object should have a looping animation
		public Transform backgroundObject;
		
		//The speed of the animation loop
		public float animationSpeed = 1;
		
		//The offset of the time of the animation
		public float animationOffset = 0;
	}
}
