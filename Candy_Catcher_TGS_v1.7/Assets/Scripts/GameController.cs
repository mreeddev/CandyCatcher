//AIM -rem Contains Logic of whole game.
//Added to canvas of Level-1 Scene
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEngine.Advertisements;

public class GameController : MonoBehaviour
{

	public static GameController instance;
	public Text scoreBoard;
	public Text high_Score;
	public Text last_Score;
	public Text remainingCandyToDie;
	public Slider slider;
	public Button pauseButton;
	public GameObject gameOverPanel;
	public GameObject RewardPanel;
	public GameObject pausePanel;
	public GameObject ContinueplayPanel;
	public GameObject ObjDie;
	//reference to bg image for disabling 2D collider.
	public GameObject ballSpawn;
	public GameObject pauseBtn;
	public GameObject[] arr;
	public GameObject[] btn;
	public GameObject[] clouds;
	public GameObject SunPosition;
	public GameObject intructionPanel;
	public GameObject shareBtn;
	public Transform GameOverPos;
	public Transform PauseMenuPos;
	public Transform rewardVideoPos;
	public Transform ContdPlayPos;
	public Transform CloudsPos;
	private int currentScore;
	public int _HighScore;
	private int counter;
	public static int remainingCandy;
	public int noOfCandytoDie;
	//public 
	public static int deadCount;
	//associated with BallManager class
	public static bool isPause;
	public bool isGameOver;
	private float time;
	private float timer;
	private float time_Slider;
	private float repeatRate;
	private Vector2 pos;
	public AudioClip scoreClip;
	public AudioClip btnClickClip;
	public AudioClip gameOverClip;
	public AudioClip colliderClip;
	public AudioClip gamePauseClip;
	public AudioClip lastCountDownClip;


	void Awake ()
	{
		if (PluginManager._insta.set60FPS) { // set framerate to 60 fps
			Application.targetFrameRate = 60;
		}

		instance = this; //reference to current class object
		time = 0;
		time_Slider = 5; //5 seconds countdown timer for slider
		deadCount = 0;
		//remainingCandy = 10; //to display remaining candy to die.initialzie with 10 and then decrease as deadCount increases.
		isGameOver = false;
		isPause = false;
		gameOverPanel.SetActive (false);//disable gameOver panel 
		pausePanel.SetActive (false); //disable pause panel
		RewardPanel.SetActive (false);
		ContinueplayPanel.SetActive (false);

		remainingCandyToDie.text = " " + noOfCandytoDie;
		remainingCandy = noOfCandytoDie;

		if (Singleton.instance.ex_Score != 0) { //chk for Score Update when Player Wants to Continue with current Score
			currentScore = Singleton.instance.ex_Score;
			scoreBoard.text = "" + currentScore;
			Singleton.instance.ex_Score = 0;
		}
		ItweenButtonEffects ();

		if (PlayerPrefs.GetInt ("instr") != 1) { // set intrucion panel
			intructionPanel.SetActive (true);
			Time.timeScale = 0;
		}

		if (!PluginManager._insta.enableSharingFeature) { // if sharing feature not enabled
			shareBtn.SetActive(false);
		}

		PluginManager._insta.loadInterstitial (); // load request ad
		//PluginManager._insta.LogScreen ("GamePlay"); // send screen to analytics
	}

	public void ItweenButtonEffects ()
	{ //apply effects to buttons
		iTween.MoveFrom (clouds [1], iTween.Hash ("x", CloudsPos.transform.position.x + 50, "LoopType", "loop", "time", 30f, "delay", 0.4f, "easetype", iTween.EaseType.linear));
		iTween.MoveFrom (clouds [0], iTween.Hash ("x", CloudsPos.transform.position.x, "LoopType", "loop", "time", 30f, "delay", 0.4f, "easetype", iTween.EaseType.linear));
		iTween.MoveFrom (clouds [2], iTween.Hash ("x", CloudsPos.transform.position.x + 150, "LoopType", "loop", "time", 30f, "delay", 2.0f, "easetype", iTween.EaseType.linear));
		iTween.RotateBy (SunPosition, iTween.Hash ("z", -1f, "loopType", "loop", "time", 10f, "easetype", iTween.EaseType.linear));
		iTween.PunchRotation (pauseBtn, iTween.Hash ("y", 180, "LoopType", "loop", "delay", 2.0f));
		foreach (GameObject obj in btn) { //it will apply following iTween effect to all components of an array named "btn" here
			iTween.PunchScale (obj, iTween.Hash ("x", 0.1f, "LoopType", "loop", "delay", 0.1f));
		}
	}

	void Start ()
	{
		repeatRate = 1.5f;//initialize repeatRate to instantiate balls* here.
		InvokeRepeating ("LaunchBalls", 0.0f, repeatRate);//launchBalls is name of method & 2.0f is the repeating  time of corresponding function.
	
		PluginManager._insta.LogScreen ("GamePlay"); // send screen to analytics
	}

	void LaunchBalls ()
	{
		Vector2 myPos = new Vector2 (ballSpawn.transform.position.x + Random.Range (-45, 96), ballSpawn.transform.position.y);//BallSpawn is gameObject to hold initial position of balls*.
		Instantiate (arr [Random.Range (0, 4)], myPos, ballSpawn.transform.rotation);//here arr[0 to 4] is an object to be instantiated & ballSpawn is gameObject whose position is being used for instantiation of a ball*.
		counter++; //counter increases aftr instantiation of Candy.
		SetRandomBalls (); //method call 
	}

	void SetRandomBalls ()
	{
		pos = new Vector2 (ballSpawn.transform.position.x + Random.Range (-45, 96), ballSpawn.transform.position.y); //set Position.
		if (counter > 20) {
			if (counter % 6 == 0) {
				Instantiate (arr [4], pos, ballSpawn.transform.rotation);//instantiates StarBall at index 4 in an array.
			} else if (counter % 4 == 0) {
				Instantiate (arr [5], pos, ballSpawn.transform.rotation);//instantiates DangerBall at index 5 in an array.
			} else if (counter % 5 == 0) {
				Instantiate (arr [6], pos, ballSpawn.transform.rotation);//instantiates bomb at index 6 in an array.
			}
		}
	}

	void FallingSpeed (float timer)
	{
		if (timer > 5.0f && timer < 10.0f) { //it will speedUp fallingRate of Balls After 5 seconds.
			repeatRate = 1.0f;
		} else if (timer > 10.0f && timer < 30.0f) { //it will speedUp fallingRate of Balls After 5 seconds.
			repeatRate = 0.5f;
		} else if (timer > 30.0f && timer < 45.0f) {
			repeatRate = 0.15f;
		} else if(timer > 45.0f) {
			repeatRate = 0.04f;
		}
	}

	public void closeIntruction ()
	{
		PlayerPrefs.SetInt ("instr", 1);
		Time.timeScale = 1;
		intructionPanel.SetActive (false);
	}

	public void ShowRewardedAd () //it will be called when player selects VideoPlay button from GameRewardPanel
	{
		isGameOver = false;
		Debug.Log ("button Clicked");
        ////if (Advertisement.IsReady("rewardedVideo"))
        ////{  //default code for Advertisement.
        ////    var options = new ShowOptions { resultCallback = HandleShowResult };
        ////    Advertisement.Show("rewardedVideo", options);
        ////}
        ////else
        ////{
        ////    RewardPanel.SetActive(false);
        ////    GameOver(); //executed if Advertisements is not ready.
        ////}
    }

    //public void HandleShowResult (ShowResult result)
    //{
    //	switch (result) {
    //	case ShowResult.Finished:
    //		Debug.Log ("ad finished");
    //		ContinueplayPanel.SetActive (true);
    //		iTween.MoveFrom (ContinueplayPanel, iTween.Hash ("x", ContdPlayPos.transform.position.x, "easetype", iTween.EaseType.easeOutBack, "time", 1.0f));
    //		RewardPanel.SetActive (false);
    //		break;
    //	case ShowResult.Skipped:
    //		RewardPanel.SetActive (false);
    //		Debug.Log ("Ad was Skipped");
    //		GameOver ();
    //		break;
    //	case ShowResult.Failed:
    //		RewardPanel.SetActive (false);
    //		Debug.LogError ("Ad Failed");
    //		GameOver ();
    //		break;
    //	}
    //}

    public void remainBalls ()
	{
		remainingCandyToDie.text = " " + remainingCandy;

	}

	void Update ()
	{
		time += Time.deltaTime;
		FallingSpeed (time);//function call

		if (isGameOver) {
			CancelInvoke ("LaunchBalls"); //stop instantiation of candy// cancel calling of function named LaunchBalls
			ObjDie.GetComponent<BoxCollider2D> ().enabled = false; //disables collider at down part of screen.
			BallCatcher.instance.Container.SetActive (false); //hides basket
			pauseButton.interactable = false;
			SliderEffect ();
		}

		//on backpress button pause enable
		if (Input.GetKeyDown (KeyCode.Escape) && !isGameOver && !gameOverPanel.activeSelf) {
			OnPause ();
		}
	}

	public void gameisOver ()
	{
		
		isGameOver = true;
		Debug.Log ("game is over reward");
		//if (Advertisement.IsReady ("rewardedVideo") && PluginManager._insta.enableUnityRewardAds && Application.internetReachability != NetworkReachability.NotReachable && Singleton.instance.rewardVideoTry < PluginManager._insta.numberTryWithVideo) {
		//	RewardPanel.SetActive (true); //shows reward panel
		//	iTween.MoveFrom (RewardPanel, iTween.Hash ("x", rewardVideoPos.transform.position.x, "easetype", iTween.EaseType.easeOutBack, "time", 1.0f));
		//} else {
			GameOver ();
		//}

	}

	public void SliderEffect ()
	{
		if (time_Slider >= 0) { //it will go in reverse direction from 5 seconds to 0.
			time_Slider -= Time.deltaTime;
			slider.value = time_Slider; 
			if (slider.value == 0) {
				StartCoroutine ("ScaleZero");
			}
		}
	}

	IEnumerator ScaleZero ()
	{
		iTween.MoveTo (RewardPanel, iTween.Hash ("x", rewardVideoPos.transform.position.x, "easetype", iTween.EaseType.easeOutBack, "time", 0.2f));
		yield return new WaitForSeconds (1.0f);
		RewardPanel.SetActive (false);
		GameOver ();
	}

	public void ScoreUpdate (int score) //its function is to update score.it will be called in ballManager Script.
	{
		currentScore += score; 
		scoreBoard.text = " " + currentScore;
		if (score == 0) { //when bomb will collide with basket
			iTween.ShakePosition (Camera.main.gameObject, iTween.Hash ("x", 1.0f, "y", 1.0f, "time", 1.0f));
			gameisOver ();
		}

		Singleton.instance.sound.PlayOneShot (scoreClip);

	}

	public void GameOver ()
	{
		Time.timeScale = 1;

		if (pausePanel.activeSelf) { //check pause panel isn't active
			pausePanel.SetActive (false);
		}

		CancelInvoke ();
		BallCatcher.instance.Container.SetActive (false); //hides basket
		pauseButton.interactable = false;

		isPause = true;
		remainingCandy = 0;
		iTween.ShakePosition (Camera.main.gameObject, iTween.Hash ("x", 1.0f, "y", 1.0f, "time", 1.0f));

		Singleton.instance.sound.PlayOneShot (gameOverClip);//plays GameOver Sound.


		if (currentScore > PlayerPrefs.GetInt ("HighScore")) { //it will Compare HighScore from main Screen & change its value if this condition becomes true.
			_HighScore = currentScore; //Assign Current Score to HighScore.
			PlayerPrefs.SetInt ("HighScore", _HighScore); //set / update HighScore.
			PluginManager._insta.ReportScore (currentScore); // set for leaderbaord
		}
		StartCoroutine (TweenPanelEffect ()); //calls TweenPanelEffect function.
		BallCatcher.instance.Container.SetActive (false);//disable basket*.
		gameOverPanel.SetActive (true);//set gameOver panel Active.
		high_Score.text = "" + PlayerPrefs.GetInt ("HighScore"); //Prints HighScore 
		last_Score.text = "" + currentScore; //prints Current Score
		isGameOver = false;

		Singleton.instance.rewardVideoTry = 0; // reset counter

	}

	IEnumerator TweenPanelEffect ()
	{
		iTween.MoveFrom (gameOverPanel, iTween.Hash ("y", GameOverPos.transform.position.y, "easetype", iTween.EaseType.easeOutBack, "time", 1.0f));
		yield return new WaitForSeconds (1.03f);
		//ItweenButtonEffects ();
		//yield return new WaitForSeconds (1.1f);
		//Time.timeScale = 0; 

		PluginManager._insta.showInterstitial (); // show ad
	}

	public void OnPause () //called OnClick event of Pause Button
	{
		if (!pausePanel.activeSelf && !RewardPanel.activeSelf && !gameOverPanel.activeSelf && !ContinueplayPanel.activeSelf) {
		
			Singleton.instance.sound.PlayOneShot (gamePauseClip);
			isPause = true;
			pausePanel.SetActive (true);
			StartCoroutine (TweenEffect ());
			pauseButton.interactable = false;
		}

	
	}

	IEnumerator TweenEffect ()
	{
		iTween.MoveFrom (pausePanel, iTween.Hash ("y", PauseMenuPos.transform.position.y, "easetype", iTween.EaseType.easeOutBack, "time", 1.0f));
		yield return new WaitForSeconds (1.1f);
		//ItweenButtonEffects ();
		//yield return new WaitForSeconds (1.1f);
		Time.timeScale = 0; //pause Scene

		//PluginManager._insta.showInterstitial (); // show ad
	}

	public void OnPlay () //called On OnClick event of Play Button
	{
		Time.timeScale = 1;//Play Scene
		Singleton.instance.sound.PlayOneShot (btnClickClip);
		pausePanel.SetActive (false);
		isPause = false;
		pauseButton.interactable = true;
	}

	public void OnContinueplay () //called On OnClick event of Play Button
	{
		Singleton.instance.sound.PlayOneShot (btnClickClip);
		Singleton.instance.rewardVideoTry++;

		iTween.MoveTo (ContinueplayPanel, iTween.Hash ("x", ContdPlayPos.transform.position.x, "easetype", iTween.EaseType.easeInBack, "time", 1.0f));
		ContinueplayPanel.SetActive (false);
		Time.timeScale = 1;//Play Scene
		Singleton.instance.ex_Score = currentScore;
		remainingCandyToDie.text = "" + 10;
		SceneManager.LoadScene (1);//jump to scene with index 1.
	}

	public void OnRestart () //Called on Restart Button
	{
		Singleton.instance.sound.PlayOneShot (btnClickClip);
		Singleton.instance.rewardVideoTry = 0; // reset counter
		Time.timeScale = 1;
		SceneManager.LoadScene (1);//jump to scene with index 0.
	}

	public void gameContinue () //Called on Restart Button of Game Over Panel
	{

		Singleton.instance.sound.PlayOneShot (btnClickClip);
	
		Time.timeScale = 1;
		Singleton.instance.ex_Score = currentScore;
		SceneManager.LoadScene (1);//jump to scene with index 1.
	}

	public void OnHome () //Called on Home Button
	{

		Singleton.instance.sound.PlayOneShot (btnClickClip);
		Time.timeScale = 1;
		SceneManager.LoadScene (0);//jump to scene with index 1.
	}

	public void shareMe ()
	{
		#if UNITY_ANDROID
		PluginManager._insta.ShareScreenshotWithText ("Hey,\n I'm playing " + Application.productName.ToString() + ".\nMy high score is "+ PlayerPrefs.GetInt ("HighScore") + ".\n Download : "+ PluginManager.AndroidLink);
		#else
		PluginManager._insta.ShareScreenshotWithText ("Hey,\n I'm playing " + Application.productName.ToString () + ".\nMy high score is " + PlayerPrefs.GetInt ("HighScore") + ".\n Download : " + PluginManager._insta.iOSLink);
		#endif
	}



	bool isPaused = false;

	void OnApplicationPause (bool pauseStatus)
	{

		isPaused = pauseStatus;
		if (!isPaused) {
			//Time.timeScale = 1;

			OnPause ();


		}
		Debug.Log ("pauseStatus" + pauseStatus);
	}
}
	