using UnityEngine;
using System.Collections;

namespace InfiniteHopper
{
	/// <summary>
	/// This script executes a function on a target object. It needs to be attached to a UI Button.
	/// </summary>
	public class IPHButtonFunction:MonoBehaviour 
	{
		// The tag of the object in which the function will run
		public string targetTag = "GameController";

		//The object in which the function will run
		internal GameObject targetObject;
		
		//The name of the function that will be executed OnMouseDown
		public string mouseDownFunction = "StartJump";

		//The name of the function that will be executed OnMouseUp
		public string mouseUpFunction = "EndJump";

		/// <summary>
		/// Start this instance.
		/// </summary>
		void Start()
		{
			// If the target object is not assigned, assign it by the tag
			if ( targetObject == null )    targetObject = GameObject.FindGameObjectWithTag(targetTag);
		}

		/// <summary>
		/// Executes the function at the target object, OnMouseDown
		/// </summary>
		public void ExecuteMouseDown()
		{
			// Check if we have a function name
			if ( mouseDownFunction != string.Empty )
			{  
				// Check if there is a target object
				if ( targetObject )    
				{
					//Send the message to the target object, with a parameter
					targetObject.SendMessage(mouseDownFunction);
				}
			}
		}

		/// <summary>
		/// Executes the function at the target object, OnMouseUp
		/// </summary>
		public void ExecuteMouseUp()
		{
			// Check if we have a function name
			if ( mouseUpFunction != string.Empty )
			{  
				// Check if there is a target object
				if ( targetObject )    
				{
					//Send the message to the target object, with a parameter
					targetObject.SendMessage(mouseUpFunction);
				}
			}
		}
	}
}