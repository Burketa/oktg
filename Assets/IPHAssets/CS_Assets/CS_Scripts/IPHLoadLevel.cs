#if UNITY_5_3
using UnityEngine.SceneManagement;
#endif

using UnityEngine;

namespace InfiniteHopper
{
	/// <summary>
	/// Includes functions for loading levels and URLs. It's intended for use with UI Buttons
	/// </summary>
	public class IPHLoadLevel:MonoBehaviour
	{
		/// <summary>
		/// Loads the URL.
		/// </summary>
		/// <param name="urlName">URL/URI</param>
		public void LoadURL(string urlName)
		{
			Application.OpenURL(urlName);
		}
	
		/// <summary>
		/// Loads the level.
		/// </summary>
		/// <param name="levelName">Level name.</param>
		public void LoadLevel(string levelName)
		{
			#if UNITY_5_3
			SceneManager.LoadScene(levelName);
			#else
			Application.LoadLevel(levelName);
			#endif
		}

		/// <summary>
		/// Restarts the current level.
		/// </summary>
		public void RestartLevel()
		{
			#if UNITY_5_3
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			#else
			Application.LoadLevel(Application.loadedLevelName);
			#endif
		}
			
		public void RestartLevelWithBonus()
		{					
			#if UNITY_5_3
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			#else
			Application.LoadLevel(Application.loadedLevelName);
			#endif
		}

	}
}