using UnityEngine;
using System.Collections;

namespace InfiniteHopper
{
	/// <summary>
	/// This script gives a sprite a random color when it's spawned
	/// </summary>
	public class IPHRandomSpriteColor : MonoBehaviour 
	{
		//The list of colors to choose from 
		public Color[] colors;

		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start() 
		{
			//Choose a random color from the list and assign it to the sprite
			if ( colors.Length > 0 )    gameObject.GetComponent<SpriteRenderer>().color = colors[Mathf.FloorToInt(Random.value * colors.Length)];
		}
	}
}