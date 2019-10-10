#if UNITY_5_3
using UnityEngine.SceneManagement;
#endif

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using InfiniteHopper.Types;
using UnityEngine.Advertisements;

namespace InfiniteHopper
{
	/// <summary>
	/// This script controls the game, starting it, following game progress, and finishing it with game over.
	/// It also creates columns as the player moves forward, and calculates the bonus the player gets when it lands.
	/// </summary>
	public class IPHGameController:MonoBehaviour 
	{
		//The player object, and the current player
		public Transform[] playerObjects;
		public int currentPlayer = 0;

		//The camera that follows the player, and its speed
		public Transform cameraObject;
		public float cameraSpeed = 10;
		
		//A list of background elements that loop
		public LoopingBackground[] loopingBackground;
		
		//A list of columns that that randomly appear as the player moves forward
		public Transform[] columns;

		// A list of moving columns that randomly appear as the player moves forward
		public Transform[] movingColumns;
		
		// The chance for a moving column to appear
		public float movingColumnChance = 0;
		
		//The range of the horizontal and vertical gap between each two columns
		public Vector2 columnGapRange = new Vector2(3,7);
		public Vector2 columnHeightRange = new Vector2(-4,0);
		
		//How many columns to create before starting the game
		public int precreateColumns = 20;
		public Vector2 nextColumnPosition = new Vector2( 0, -2);
		
		//A list of items that can appear on columns
		public Transform[] items;
		
		//After how many columns does an item appear?
		public int itemRate = 8;
		internal int itemRateCount = 0;

		// An array of powerups that can be activated
		public Powerup[] powerups;

		//The jump button. Hold this button to charge the jump power, then release to launch the player up and forward
		public string jumpButton = "Jump";

		//Should the player auto jump when reaching the maximum jump power?
		public bool playerAutoJump = true;

		//A list of scores the player gets based on how far from the center of the column he lands
		public LandingBonus[] landingBonuses;

		//The streak value is multiplied by the bonus you get when you land closest to the center of a platform.
		//Each consecutive perfect landing adds 1x to the streak. The streak is broken when you don't land closest to the center.
		internal int currentStreak = 0;
		
		//The text object that shows the bonus we got when landing
		public Transform bonusText;
		
		//The score and score text of the player
		public int score = 0;
		public Transform scoreText;
		internal int highScore = 0;
		internal int scoreMultiplier = 1;

		//The overall game speed
		public float gameSpeed = 1;
		
		//How many points the player needs to collect before leveling up
		public int levelUpEveryScore = 1000;
		internal int levelProgress = 0;

		// The change in column height when leveling up
		public Vector2 columnHeightIncrease = new Vector2(-0.2f, 0.2f);
		public Vector2 columnHeightMax = new Vector2(-4, 1);
		
		// Increases the moving column chance when leveling up
		public float increaseMovingColumnChance = 0.05f;
		
		//This is vertical (Y) position of the death line. When the player falls below this line, he dies
		public float deathLineHeight = -2;

		// The number of continues the player has
		public int continues = 1;
		
		// The last position the player landed on succesfully. This is used to reset the player when continuing after GameOver
		internal Transform lastLandedObject;
		
		//Various canvases for the UI
		public Transform gameCanvas;
		public Transform pauseCanvas;
		public Transform gameOverCanvas;
		public Transform bonusCanvas;
		
		//Is the game over?
		internal bool  isGameOver = false;
		
		//The level of the main menu that can be loaded after the game ends
		public string mainMenuLevelName = "MainMenu";
		
		//Various sounds and their source
		public AudioClip soundLevelUp;
		public AudioClip soundGameOver;
		public string soundSourceTag = "GameController";
		internal GameObject soundSource;
		
		//The button that will restart the game after game over
		public string confirmButton = "Submit";
		
		//The button that pauses the game. Clicking on the pause button in the UI also pauses the game
		public string pauseButton = "Cancel";
		internal bool  isPaused = false;
		
		internal int index = 0;

		// A list of records that keep track of various game stats
		internal int currentDistance = 0;
		internal int longestDistance = 0;
		internal int longestStreak = 0;
		internal int totalPowerups = 0;
		internal int currentPowerUpStreak = 0;
		internal int longestPowerUpStreak = 0;


		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start()
		{

			//Initialize Unity Ads
			#if UNITY_ANDROID
			Advertisement.Initialize ("44169");
			#elif UNITY_IPHONE
			Advertisement.Initialize ("44172");
			#endif

			

				//Update the score
				UpdateScore ();
			
				//Hide the game over canvas
				if (gameOverCanvas)
					gameOverCanvas.gameObject.SetActive (false);

                //Get the highscore for the player

                highScore = 0;                               
                
                totalPowerups = 0;
			
				//Set the current player object
				SetPlayer (currentPlayer);
			
				//If the player object is not already assigned, Assign it from the "Player" tag
				if (cameraObject == null)
					cameraObject = GameObject.FindGameObjectWithTag ("MainCamera").transform;
			
				for (index = 0; index < loopingBackground.Length; index++) {
					//Choose a random time from the animation
					//loopingBackground[index].backgroundObject.animation[loopingBackground[index].backgroundObject.animation.clip.name].time = Random.Range(0, loopingBackground[index].backgroundObject.animation.clip.length);
					loopingBackground [index].backgroundObject.GetComponent<Animation> () [loopingBackground [index].backgroundObject.GetComponent<Animation> ().clip.name].time = loopingBackground [index].animationOffset;
				
					//Enable the animation
					loopingBackground [index].backgroundObject.GetComponent<Animation> () [loopingBackground [index].backgroundObject.GetComponent<Animation> ().clip.name].enabled = true;
				
					//Sample the animation at the current time
					loopingBackground [index].backgroundObject.GetComponent<Animation> ().Sample ();
				
					//Disable the animation
					loopingBackground [index].backgroundObject.GetComponent<Animation> () [loopingBackground [index].backgroundObject.GetComponent<Animation> ().clip.name].enabled = false;
				
					//Play the animation from the new time we sampled
					loopingBackground [index].backgroundObject.GetComponent<Animation> ().Play ();
				}
			
				//Create a few columns at the start of the game
				createColumn (1, false);
				createColumn (precreateColumns, true);

				//Go through all the powerups and reset their timers
				for (index = 0; index < powerups.Length; index++) {
					//Set the maximum duration of the powerup
					powerups [index].durationMax = powerups [index].duration;
				
					//Reset the duration counter
					powerups [index].duration = 0;
				
					//Deactivate the icon of the powerup
					powerups [index].icon.gameObject.SetActive (false);
				}
			
				//Assign the sound source for easier access
				if (GameObject.FindGameObjectWithTag (soundSourceTag))
					soundSource = GameObject.FindGameObjectWithTag (soundSourceTag);

				//Pause the game at the start
				Pause ();

			
		}

		/// <summary>
		/// Update is called every frame, if the MonoBehaviour is enabled.
		/// </summary>
		void  Update()
		{
			//If the game is over, listen for the Restart and MainMenu buttons
			if ( isGameOver == true )
			{
				//The jump button restarts the game
				if ( Input.GetButtonDown(confirmButton) )
				{
					Restart();
				}
				
				//The pause button goes to the main menu
				if ( Input.GetButtonDown(pauseButton) )
				{
					MainMenu();
				}
			}
			else
			{
				//Toggle pause/unpause in the game
				if ( Input.GetButtonDown(pauseButton) )
				{
					if ( isPaused == true )    Unpause();
					else    Pause();
				}
				
				//If there is a player object, you can make it jump, the background moves in a loop.
				if ( playerObjects[currentPlayer] )
				{
					if ( cameraObject )
					{
						//Make the camera chase the player in all directions
						cameraObject.GetComponent<Rigidbody2D>().velocity = new Vector2((playerObjects[currentPlayer].position.x - cameraObject.position.x) * cameraSpeed, cameraObject.GetComponent<Rigidbody2D>().velocity.y);
					}
					
					//If we press the jump buttons, start the jump sequence, charging up the jump power
					if ( Input.GetButtonDown(jumpButton) )    StartJump();
					
					//If we release the jump buttons, end the jump sequence, and make the player jump
					if ( Input.GetButtonUp(jumpButton) )    EndJump();
					
					//If the player object moves below the death line, kill it.
					if ( playerObjects[currentPlayer].position.y < deathLineHeight )     playerObjects[currentPlayer].SendMessage("Die");
				}
			}
			
			//Set the speed of the looping background based on the horizontal speed of the player
			for ( index = 0 ; index < loopingBackground.Length ; index++ )
			{
				loopingBackground[index].backgroundObject.GetComponent<Animation>()[loopingBackground[index].backgroundObject.GetComponent<Animation>().clip.name].speed = loopingBackground[index].animationSpeed * cameraObject.GetComponent<Rigidbody2D>().velocity.x;
			}
			
			//If the camera moved forward enough, create another column
			if ( nextColumnPosition.x - cameraObject.position.x < precreateColumns * 5 )
			{ 
				createColumn(1, true);
			}
		}
		
		//This function creates a column
		void  createColumn ( int columnCount, bool giveBonus)
		{
			//Create a few columns at the start of the game
			while ( columnCount > 0 )
			{
				columnCount--;
				
				//Choose a random column from the list
				int randomColumn = 0;
				
				//Create a random column from the list of available columns
				Transform newColumn;
				
				// There's a chance for a moving column to appear
				if ( Random.value < movingColumnChance )
				{
					//Choose a random column from the list
					randomColumn = Mathf.FloorToInt(Random.Range(0, movingColumns.Length));
					
					//Create a random column from the list of available moving columns
					newColumn = Instantiate( movingColumns[randomColumn], nextColumnPosition, Quaternion.identity) as Transform;
				}
				else
				{
					//Choose a random column from the list
					randomColumn = Mathf.FloorToInt(Random.Range(0, columns.Length));
					
					//Create a random column from the list of available columns
					newColumn = Instantiate( columns[randomColumn], nextColumnPosition, Quaternion.identity) as Transform;
				}

				// Record the first column we land on
				if ( giveBonus == false )    lastLandedObject = newColumn;

				//Go to the next column position, based on the gap of the current column
				nextColumnPosition.x += Random.Range(columnGapRange.x, columnGapRange.y);
				nextColumnPosition.y = Random.Range(columnHeightRange.x, columnHeightRange.y);

				// If the column is moving, give it a vertical range
				newColumn.SendMessage("SetMoveRange", new Vector2( columnHeightRange.x, columnHeightRange.y));

				//Should this column give bonus when landed upon?
				if ( giveBonus == false )    newColumn.SendMessage("NoBonus");
				
				//Count the rate for an item to appear on a column
				itemRateCount++;
				
				//Create a new item on the column
				int startingPowerUp = 0;
                startingPowerUp = 0;

				if (startingPowerUp == 1) {
					Instantiate( items[Mathf.FloorToInt(Random.Range(0, items.Length))], newColumn.position, Quaternion.identity);					
				}
				else if ( itemRateCount >= itemRate  )
				{
					//Create a new random item from the list of items
					Instantiate( items[Mathf.FloorToInt(Random.Range(0, items.Length))], newColumn.position, Quaternion.identity);
					
					//Reset the item rate counter
					itemRateCount = 0;
				}
			}
		}
		
		//This function changes the score of the player
		void  ChangeScore( Transform landedObject )
		{
			// Record the last landed object, so we can reset the player position when continuing after game over
			lastLandedObject = landedObject;

			//Calculate the distance of the player from the center of the column when it landed on it
			float landingDistance = Mathf.Abs(landedObject.position.x - playerObjects[currentPlayer].position.x);
			
			//Has bonus been given yet? If so, don't give any more bonus
			bool  bonusGiven = false;
			
			//Go through all landing bonuses, and check which one should be given to the player
			for ( index = 0 ; index < landingBonuses.Length ; index++ )
			{
				//If no bonus has been given, check if the player is within the correct distance to get a bonus
				if ( bonusGiven == false && landingDistance <= landingBonuses[index].landDistance )    
				{
					//Increase the streak if we are closest to the center, or reset it if we're not
					if ( index == 0 )    
					{
						currentStreak++;
						
						//Add the bonus to the score
						score += landingBonuses[index].bonusValue * currentStreak * scoreMultiplier;

						// Increase level progress
						levelProgress += landingBonuses[index].bonusValue * currentStreak * scoreMultiplier;

						//Call the perfect landing function, which plays a sound and particle effect based on the player's streak
						playerObjects[currentPlayer].gameObject.SendMessage("PerfectLanding", currentStreak);
					}
					else    
					{
                        longestStreak = 0;

						currentStreak = 0;
						
						//Add the bonus to the score
						score += landingBonuses[index].bonusValue * scoreMultiplier;

						// Increase level progress
						levelProgress += landingBonuses[index].bonusValue * scoreMultiplier;
					}
					
					//Update the bonus text
					if ( bonusText )    
					{
						//Set the position of the bonus text to the player
						bonusText.position = playerObjects[currentPlayer].position;
						
						//Play the bonus animation
						if ( bonusText.GetComponent<Animation>() )    bonusText.GetComponent<Animation>().Play();
						
						//Update the text of the bonus object. If we have a streak, display 2X 3X etc
						if ( currentStreak > 1 )    bonusText.Find("Text").GetComponent<Text>().text = "+" + (landingBonuses[index].bonusValue * currentStreak * scoreMultiplier).ToString() + " " + currentStreak.ToString() + "X";  
						else    bonusText.Find("Text").GetComponent<Text>().text = "+" + (landingBonuses[index].bonusValue * scoreMultiplier).ToString();
					}
					
					//The score has been given, no need to give any more bonus
					bonusGiven = true;
				}
			}
			
			//Update the score
			UpdateScore();

			// Add to the longest distance statistic
			currentDistance++;
		}
		
		//This function updates the player's score, ( and in a later update checks if we reached the required score to level up )
		void UpdateScore()
		{
			//Update the score text
			if ( scoreText )    scoreText.GetComponent<Text>().text = score.ToString();
			
			//If we reached the required number of points, level up!
			if ( levelProgress >= levelUpEveryScore )
			{
				levelProgress -= levelUpEveryScore;
				
				LevelUp();
			}
		}
		
		//This function levels up, and increases the difficulty of the game
		void LevelUp()
		{
			// Increase the height range of the columns
			columnHeightRange += columnHeightIncrease;
			
			// Limit the height range of columns
			if ( columnHeightRange.x < columnHeightMax.x )    columnHeightRange.x = columnHeightMax.x;
			if ( columnHeightRange.y > columnHeightMax.y )    columnHeightRange.y = columnHeightMax.y;
			
			// Increase the chance of a moving column appearing
			movingColumnChance += increaseMovingColumnChance;
			
			// If there is a source and a sound, play it from the source
			if ( soundSource && soundLevelUp )    soundSource.GetComponent<AudioSource>().PlayOneShot(soundLevelUp);
		}


		//This function sets the score multiplier ( When the player picks up coins he gets 1X,2X,etc score )
		void SetScoreMultiplier( int setValue )
		{			
			// Set the score multiplier
			scoreMultiplier = setValue;
		}
		
		//This function pauses the game
		void  Pause()
		{
			isPaused = true;
			
			//Set timescale to 0, preventing anything from moving
			Time.timeScale = 0;
			
			//Show the pause screen and hide the game screen
			if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(true);
			if ( gameCanvas )    gameCanvas.gameObject.SetActive(false);
		}
		
		//This function resume the game
		void  Unpause()
		{
			isPaused = false;
			
			//Set timescale back to the current game speed
			Time.timeScale = gameSpeed;
			
			//Hide the pause screen and show the game screen
			if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(false);
			if ( gameCanvas )    gameCanvas.gameObject.SetActive(true);


		}
		//This function handles the game over event
		IEnumerator GameOver(float delay)
		{
			//Go through all the powerups and nullify their timers, making them end
			for ( index = 0 ; index < powerups.Length ; index++ )
			{
				//Set the duration of the powerup to 0
				powerups[index].duration = 0;
			}			

			yield return new WaitForSeconds(delay);

			//If there is a source and a sound, play it from the source
			if ( soundSource && soundGameOver )    soundSource.GetComponent<AudioSource>().PlayOneShot(soundGameOver);
			
			isGameOver = true;
			
			//Hide the pause screen and the game screen
			if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(false);
			if ( gameCanvas )    gameCanvas.gameObject.SetActive(false);
							//Show the game over screen
			if ( gameOverCanvas )    
			{
				
				//Show the game over screen
				gameOverCanvas.gameObject.SetActive(true);
				
				//Write the score text
				gameOverCanvas.Find("TextScore").GetComponent<Text>().text = "SCORE " + score.ToString();
				
				//Check if we got a high score
				if ( score > highScore )    
				{
					highScore = score;									
				}
				
				//Write the high sscore text
				gameOverCanvas.Find("TextHighScore").GetComponent<Text>().text = "HIGH SCORE " + highScore.ToString();


				// Show bonus button

				bonusCanvas.gameObject.SetActive(Advertisement.isReady());

			}
		}
		
		//This function restarts the current level
		void  Restart ()
		{
			#if UNITY_5_3
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			#else
			Application.LoadLevel(Application.loadedLevelName);
			#endif
		}

		//This function returns to the Main Menu
		void  MainMenu()
		{
			#if UNITY_5_3
			SceneManager.LoadScene(mainMenuLevelName);
			#else
			Application.LoadLevel(mainMenuLevelName);
			#endif
		}

	//	This function continues the game from your last point
	public void Continue()
		{
	if ( continues > 0 )
	{
		// Reset the player to its last position
		playerObjects[currentPlayer].position = lastLandedObject.position;

//		 Reset the player's dead status
		playerObjects[currentPlayer].SendMessage("NotDead");

//		 Continue the game
		isGameOver = false;
		
		// Show the game screen and hide the game over screen
		if ( gameCanvas )    gameCanvas.gameObject.SetActive(true);
		if ( gameOverCanvas )    gameOverCanvas.gameObject.SetActive(false);

		ChangeContinues(-1);
	}
		}
	

		// This function changes the number of continues we have
		public void ChangeContinues( int changeValue )
		{
			continues += changeValue;

			// Limit the minimum number of continues to 0
			if ( continues > 0 ) 
			{
				// Deactivate the continues object if we have no more continues
				if ( gameOverCanvas )    gameOverCanvas.Find("ButtonContinue").gameObject.SetActive(true);
			}
			else
			{
				// Activate the continues object if we have no more continues
				if ( gameOverCanvas )    gameOverCanvas.Find("ButtonContinue").gameObject.SetActive(false);
			}
		}
		
		//This function activates the selected player, while deactivating all the others
		void  SetPlayer( int playerNumber )
		{
			//Go through all the players, and hide each one except the current player
			for(index = 0; index < playerObjects.Length; index++)
			{
				if (index != playerNumber)
					playerObjects [index].gameObject.SetActive (false);
				else {
					playerObjects [index].gameObject.SetActive (true);					
				}
			}
		}
		
		//This function sends a start jump command to the current player
		public void StartJump()
		{
			if ( playerObjects[currentPlayer] )    playerObjects[currentPlayer].SendMessage("StartJump", playerAutoJump);
		}
		
		//This function sends an end jump command to the current player
		void EndJump()
		{
			if ( playerObjects[currentPlayer] )    playerObjects[currentPlayer].SendMessage("EndJump");
		}

		/// <summary>
		/// Sends a rescale command to the player
		/// </summary>
		void RescalePlayer( float targetScale )
		{
			if (playerObjects [currentPlayer]) {				
				playerObjects [currentPlayer].SendMessage ("Rescale", targetScale);
			}
		}

		/// <summary>
		/// Activates a power up from a list of available power ups
		/// </summary>
		/// <param name="setValue">The index numebr of the powerup to activate</param>
		IEnumerator ActivatePowerup( int powerupIndex )
		{
			//If there is already a similar powerup running, refill its duration timer
			if ( powerups[powerupIndex].duration > 0 )
			{
				//Refil the duration of the powerup to maximum
				powerups[powerupIndex].duration = powerups[powerupIndex].durationMax;

				// Add to the powerup count
				totalPowerups++;

				// Add to the current power up streak count
				currentPowerUpStreak++;
			}
			else //Otherwise, activate the power up functions
			{
				//Activate the powerup icon
				if ( powerups[powerupIndex].icon )    powerups[powerupIndex].icon.gameObject.SetActive(true);
				
				//Run up to two start functions from the gamecontroller
				if ( powerups[powerupIndex].startFunction != string.Empty )    SendMessage(powerups[powerupIndex].startFunction, powerups[powerupIndex].startParamater);

				//Fill the duration timer to maximum
				powerups[powerupIndex].duration = powerups[powerupIndex].durationMax;
				
				//Count down the duration of the powerup
				while ( powerups[powerupIndex].duration > 0 )
				{
					yield return new WaitForSeconds(Time.deltaTime);
					
					powerups[powerupIndex].duration -= Time.deltaTime;
					
					//Animate the powerup timer graphic using fill amount
					if ( powerups[powerupIndex].icon )    powerups[powerupIndex].icon.Find("FillAmount").GetComponent<Image>().fillAmount = powerups[powerupIndex].duration/powerups[powerupIndex].durationMax;
				}
				
				//Run up to two end functions from the gamecontroller
				if ( powerups[powerupIndex].endFunction != string.Empty )    SendMessage(powerups[powerupIndex].endFunction, powerups[powerupIndex].endParamater);

				//Deactivate the powerup icon
				if ( powerups[powerupIndex].icon )    powerups[powerupIndex].icon.gameObject.SetActive(false);

				// Add to the powerup count
				totalPowerups++;				

				// Reset the current power up streak
				currentPowerUpStreak = 0;
			}
		}		
		
		void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			
			//Draw the position of the next column
			Gizmos.DrawSphere( nextColumnPosition,0.2f);

			//Draw the death line. Falling off it will kill the player
			Gizmos.DrawLine( new Vector3(-10,deathLineHeight,0), new Vector3(10,deathLineHeight,0));
		}
	}
}