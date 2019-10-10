using UnityEngine;
using System;

namespace InfiniteHopper.Types
{
	[Serializable]
	public class PlayerUnlock 
	{
		//The icon/avatar of the player
		public Transform playerIcon;
			
		//How many tokens are needed to unlock this player
		public int tokensToUnlock = 5;
	}
}


