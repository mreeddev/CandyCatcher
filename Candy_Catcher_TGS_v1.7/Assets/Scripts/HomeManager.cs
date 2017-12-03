//AIM - used to Communicate with Another Scene.
//Added to Canvas Of Main Scene
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeManager: MonoBehaviour
{

	public Text highScore;
	public Text forBucket2;
	public Text forBucket3;
	public Text ControllerSelected;
	public GameObject SettingPanel;
	public GameObject RemoveAdPanel;
	public GameObject BucketPanel;
	public GameObject selectContrlPanel;
	public GameObject ControllerSelectionPanel;
	public GameObject sound_On;
	public GameObject sound_Off;
	public GameObject music_On;
	public GameObject music_Off;

	public AudioClip btnClickClip;
	public GameObject Playbtn;
	public GameObject SettingBtn;
	public GameObject RemoveAdBtn;
	//public GameObject AdRemovedBtn;
	public GameObject RatingBtn;
	public GameObject LeaderboardBtn;
	public GameObject bucketBtn;
	public GameObject bucketBtn1;
	public GameObject bucketBtn2;
	public GameObject bucketBtn3;
	public GameObject cnInputBtn;
	public GameObject[] AdRemoveBtns;
	public GameObject AccBtn;
	public GameObject SunPosition;
	public GameObject Cloud1;
	public GameObject Cloud2;
	public GameObject Cloud3;
	public Transform PlayPoint;
	public Transform SettingPoint;
	public Transform PanelPoint;
	public Transform PanelBackPoint;
	public Transform PanelUpPoint;
	public Transform cloud2Point;
	public Transform cloud1Point;
	private bool selection;

	public string myMsg; //panel msg

	public static HomeManager _insta;

	void Awake ()
	{

		_insta = this;

		//PlayerPrefs.DeleteAll ();

		SettingPanel.SetActive (false);
		RemoveAdPanel.SetActive (false);
//		AdRemovedBtn.SetActive (false);
		BucketPanel.SetActive (false);
		highScore.text = "High Score  " + PlayerPrefs.GetInt ("HighScore");//display high score from gameController 

		if (PlayerPrefs.GetInt ("Controller") == 1) {
			AccBtn.SetActive (true);
			iTween.PunchScale (AccBtn, iTween.Hash ("y", 0.1f, "LoopType", "loop", "delay", 0.6f));
			cnInputBtn.SetActive (false);
		} else if (PlayerPrefs.GetInt ("Controller") == 2) {
			AccBtn.SetActive (false);
			cnInputBtn.SetActive (true);
			iTween.PunchScale (cnInputBtn, iTween.Hash ("y", 0.1f, "LoopType", "loop", "delay", 0.6f));
		}
		BtnActivate (); //chk for high Score and set btn active or Inactive.
		ITweenEffectOnBtn ();
		initCheck ();

		#if UNITY_ANDROID
		//disable restore button in adroid
		AdRemoveBtns[1].SetActive(false);
		#endif

	
	}

	void Start(){
		PluginManager._insta.LogScreen ("MainMenu"); // send screen to analytics
	}


	void Update(){
		//on backpress button quit game
		if (Input.GetKeyDown (KeyCode.Escape)) {
			PluginManager._insta.AppQuit (); 
			//DestroyImmediate( GetComponent<GameObject> ().transform.Find ("Singleton"));
			//Application.Quit ();
		}
	}

	public void ITweenEffectOnBtn ()
	{
		iTween.MoveFrom (Cloud2, iTween.Hash ("x", cloud2Point.transform.position.x + 50, "LoopType", "loop", "time", 30f, "delay", 0.5f, "easetype", iTween.EaseType.linear));
		iTween.MoveFrom (Cloud1, iTween.Hash ("x", cloud2Point.transform.position.x, "LoopType", "loop", "time", 30f, "delay", 0.2f, "easetype", iTween.EaseType.linear));
		iTween.MoveFrom (Cloud3, iTween.Hash ("x", cloud1Point.transform.position.x, "LoopType", "loop", "time", 30f, "delay", 0.9f, "easetype", iTween.EaseType.linear));
		iTween.RotateBy (SunPosition, iTween.Hash ("z", -1f, "loopType", "loop", "time", 10f, "easetype", iTween.EaseType.linear));
		iTween.MoveFrom (Playbtn, iTween.Hash ("x", PlayPoint.transform.position.x, "easetype", iTween.EaseType.easeInBack));
		iTween.MoveFrom (SettingBtn, iTween.Hash ("x", SettingPoint.transform.position.x, "easetype", iTween.EaseType.easeInBack));
		iTween.PunchScale (SettingBtn, iTween.Hash ("y", 0.1f, "LoopType", "loop", "delay", 0.2f));
		iTween.PunchScale (Playbtn, iTween.Hash ("x", 0.2f, "LoopType", "loop", "delay", 0.5f));
		iTween.PunchScale (RemoveAdBtn, iTween.Hash ("y", 0.1f, "LoopType", "loop", "delay", 1.0f)); //,"easetype",iTween.EaseType.punch 
		iTween.PunchScale (bucketBtn, iTween.Hash ("y", 0.1f, "LoopType", "loop", "delay", 1.3f));
		iTween.PunchScale (RatingBtn, iTween.Hash ("y", 0.1f, "LoopType", "loop", "delay", 1.6f));
		iTween.PunchScale (LeaderboardBtn, iTween.Hash ("y", 0.1f, "LoopType", "loop", "delay", 1.5f));
		iTween.PunchScale (AdRemoveBtns [0], iTween.Hash ("x", 0.1f, "LoopType", "loop", "delay", 0.6f));
		iTween.PunchScale (AdRemoveBtns [1], iTween.Hash ("x", 0.1f, "LoopType", "loop", "delay", 0.6f));
	}

	private void initCheck(){ // to enale disable buttons on starts

		if (PlayerPrefs.GetInt ("sound") == 1) { // check sound button and set  
			sound_On.SetActive (false);
			sound_Off.SetActive (true);
		} else if (PlayerPrefs.GetInt ("sound") == 2) {
			sound_On.SetActive (true);
			sound_Off.SetActive (false);
		}


		if (PlayerPrefs.GetInt ("music") == 1) { // check music button and set  
			music_On.SetActive (false);
			music_Off.SetActive (true);
		} else if (PlayerPrefs.GetInt ("music") == 2) {
			music_On.SetActive (true);
			music_Off.SetActive (false);
		}

		if (!PluginManager._insta.enableRemoveAds || PlayerPrefs.GetInt ("removeAd")==1) { // remove ad option
			RemoveAdBtn.SetActive (false);
		}

		if (!PluginManager._insta.enableRateUs) { // remove ad option
			RatingBtn.SetActive (false);
		}

		if (!PluginManager._insta.enableLeaderBoard) { // leaderbaord option
			LeaderboardBtn.SetActive (false);
		}

		//PluginManager._insta.LogScreen ("MainMenu"); // send screen to analytics
//		GoogleAnalytics.instance.LogScreen ("MainMenu"); // send screen to analytics

		if (PlayerPrefs.GetInt ("contrlSelect") != 1)  //select panel
		{
			selectContrlPanel.SetActive (true);
			PlayerPrefs.SetInt("contrlSelect", 1);
		}
	}

	public void ButtonClicked ()
	{
		Singleton.instance.sound.PlayOneShot (btnClickClip);
	}

	public void selectControl(int check){
		if (check == 1) {
			PlayerPrefs.SetInt ("Controller",1);//Input button selected by default
		} else {
			PlayerPrefs.SetInt ("Controller",2);//Input motion selected by default
		}
		selectContrlPanel.SetActive (false);
	}

	public void rateUS(){
		ButtonClicked ();
		Debug.Log ("rateUs");
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			myMsg = "No Internet Connection";
			StartCoroutine (ControllerSelection ());
		} else {
			PluginManager._insta.openRateUs ();
		}


	}

	public void leaderboardShow(){
		ButtonClicked ();
		Debug.Log ("leaderboardShow");
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			myMsg = "No Internet Connection";
			StartCoroutine (ControllerSelection ());
		} else {
			PluginManager._insta.showLeaderboard ();
		}
	}

	public void OnRemoveAd ()
	{
		ButtonClicked ();

		if (Application.internetReachability == NetworkReachability.NotReachable) {
			myMsg = "No Internet Connection";
			StartCoroutine (ControllerSelection ());
		} else {
			iTween.MoveTo (RemoveAdPanel, iTween.Hash ("y", PanelBackPoint.transform.position.y, "easetype", iTween.EaseType.easeOutBack));
			RemoveAdPanel.SetActive (true);
		}
		//RemoveAdBtn.SetActive (false);
	}

	public void exitApp(){
		//PluginManager._insta.impDestroy (); 
		//DestroyImmediate( GetComponent<GameObject> ().transform.Find ("Singleton"));
		PluginManager._insta.AppQuit (); 
	}

	public void OnExit ()
	{
		ButtonClicked ();
		StartCoroutine (ExitAnimation ());
	}

	IEnumerator ExitAnimation () //method is used to wait for some seconds before Deactivate setting panel
	{
		iTween.MoveTo (RemoveAdPanel, iTween.Hash ("y", PanelUpPoint.transform.position.y, "easetype", iTween.EaseType.spring, "time", 1f));
		yield return new WaitForSeconds (1f);
		RemoveAdPanel.SetActive (false);
		RemoveAdBtn.SetActive (true);
	}

	public void OnBuy ()
	{
		RemoveAdPanel.SetActive (false);
		ButtonClicked ();
		InAppManager.instance.PurchaseProduct (PluginManager._insta.inAppRemoveAdID);
		Debug.Log ("Purchase Done");
	}

	public void OnRestore()
	{
		RemoveAdPanel.SetActive (false);
		ButtonClicked ();
		InAppManager.instance.restorePurchase ();
		Debug.Log ("Restore Done");
	} 
	public void OnClickPlay () //called when Play button is pressed.
	{
		ButtonClicked ();
		SceneManager.LoadScene (1); //jump to Scene with index 1
	}

	public void OnSettings () //called when Settings button is Pressed.
	{
		ButtonClicked ();
		SettingPanel.SetActive (true);
		iTween.MoveTo (SettingPanel, iTween.Hash ("y", PanelBackPoint.transform.position.y, "easetype", iTween.EaseType.easeOutBack));
	}

	public void OnSave () //called when Save button is pressed.
	{
		ButtonClicked ();
		StartCoroutine (CloseAnimation ()); //calls CloseAnimation().
	}

	IEnumerator CloseAnimation () //method is used to wait for some seconds before Deactivate setting panel
	{
		iTween.MoveTo (SettingPanel, iTween.Hash ("y", PanelPoint.transform.position.y, "easetype", iTween.EaseType.spring, "time", 1f));
		yield return new WaitForSeconds (1f);
		SettingPanel.SetActive (false);
	}

	public void MusicSetting (int check) //called when Music ON/OFF button is pressed.
	{
		if (check == 1) { //for music on
			ButtonClicked ();
			PlayerPrefs.SetInt ("music", 1);
			music_On.SetActive (false);
			music_Off.SetActive (true);
			Singleton.instance.music.mute = false;
			Debug.Log ("music ON");
		} else {
			ButtonClicked ();
			PlayerPrefs.SetInt ("music", 2);
			music_On.SetActive (true);
			music_Off.SetActive (false);
			Singleton.instance.music.mute = true;
			Debug.Log ("music OFF");
		}
	}

	public void SoundSetting (int check) //called when Sound ON/OFF button is pressed.
	{
		if (check == 1) { //for music on
			ButtonClicked ();
			PlayerPrefs.SetInt ("sound", 1);
			sound_On.SetActive (false);
			sound_Off.SetActive (true);
			Singleton.instance.sound.mute = false;
			Debug.Log ("sound ON");
		} else {
			ButtonClicked ();
			PlayerPrefs.SetInt ("sound", 2);
			sound_On.SetActive (true);
			sound_Off.SetActive (false);
			Singleton.instance.sound.mute = true;
			Debug.Log ("sound OFF");
		}
	}


	public void OnBucketBtn ()
	{
		ButtonClicked ();
		BucketPanel.SetActive (true);
		iTween.MoveTo (BucketPanel, iTween.Hash ("y", PanelBackPoint.transform.position.y, "easetype", iTween.EaseType.easeOutBack));
		bucketBtn.SetActive (false);
	}

	public void Onbucket1 ()
	{
		ButtonClicked ();
		PlayerPrefs.SetInt ("bucket", 1);
		StartCoroutine (BtnPressEvent1 ());
		Debug.Log ("Bucket 1 selected");
	}

	IEnumerator BtnPressEvent1 () //method is used to wait for some seconds before Deactivate setting panel
	{
		iTween.PunchScale (bucketBtn1, iTween.Hash ("x", -1, "time", 1.0f, "easetype", iTween.EaseType.punch));
		yield return new WaitForSeconds (1f);
		OnBucketExit ();
	}

	public void Onbucket2 ()
	{
		ButtonClicked ();
		PlayerPrefs.SetInt ("bucket", 2);
		StartCoroutine (BtnPressEvent2 ());
		Debug.Log ("Bucket 2 selected");
	}

	IEnumerator BtnPressEvent2 () //method is used to wait for some seconds before Deactivate setting panel
	{
		iTween.PunchScale (bucketBtn2, iTween.Hash ("x", -1, "time", 1.0f, "easetype", iTween.EaseType.punch));
		yield return new WaitForSeconds (1f);
		OnBucketExit ();
	}

	public void Onbucket3 ()
	{
		ButtonClicked ();
		PlayerPrefs.SetInt ("bucket", 3);
		StartCoroutine (BtnPressEvent3 ());
		Debug.Log ("Bucket 3 selected");
	}

	IEnumerator BtnPressEvent3 () //method is used to wait for some seconds before Deactivate setting panel
	{
		iTween.PunchScale (bucketBtn3, iTween.Hash ("x", -1, "time", 1.0f, "easetype", iTween.EaseType.punch));
		yield return new WaitForSeconds (1f);
		OnBucketExit ();
	}

	public void OnBucketExit ()
	{
		ButtonClicked ();
		StartCoroutine (Exit ());
	}

	IEnumerator Exit () //method is used to wait for some seconds before Deactivate setting panel
	{
		iTween.MoveTo (BucketPanel, iTween.Hash ("y", PanelUpPoint.transform.position.y, "easetype", iTween.EaseType.spring, "time", 1f));
		yield return new WaitForSeconds (1f);
		BucketPanel.SetActive (false);
		bucketBtn.SetActive (true);
	}

	void BtnActivate ()
	{
		if (PlayerPrefs.GetInt ("HighScore") > 150) {
			bucketBtn2.GetComponent<Button> ().interactable = true;
			forBucket2.color = Color.black;
		}
		if (PlayerPrefs.GetInt ("HighScore") > 500) {
			bucketBtn3.GetComponent<Button> ().interactable = true;
			forBucket3.color = Color.black;
		}
	}

	public void OnCNinputBtn () //button 
	{
		ButtonClicked ();
		PlayerPrefs.SetInt ("Controller", 1);
		StartCoroutine (CloseAnimation ()); 

		myMsg = "Input Keys selected";
		StartCoroutine (ControllerSelection ());
		cnInputBtn.SetActive (false);
		AccBtn.SetActive (true);
	}

	public void OnAccBtn () //sensor
	{
		ButtonClicked ();
		PlayerPrefs.SetInt ("Controller", 2);
		StartCoroutine (CloseAnimation ()); 
		myMsg = "Acceleration selected";
		StartCoroutine (ControllerSelection ());
		cnInputBtn.SetActive (true);
		AccBtn.SetActive (false);
	}

	public IEnumerator ControllerSelection ()
	{
		ControllerSelected.text = myMsg;
		yield return new WaitForSeconds (0.2f);
		ControllerSelectionPanel.SetActive (true);
		iTween.MoveTo (ControllerSelectionPanel, iTween.Hash ("y", PanelBackPoint.transform.position.y, "easetype", iTween.EaseType.easeOutBack));
	}



	public void OnControllerSelectionExit ()
	{
		ButtonClicked ();
		StartCoroutine (SelectionExit ());
	}

	IEnumerator SelectionExit () //method is used to wait for some seconds before Deactivate setting panel
	{
		iTween.MoveTo (ControllerSelectionPanel, iTween.Hash ("y", PanelPoint.transform.position.y, "easetype", iTween.EaseType.spring, "time", 1f));
		yield return new WaitForSeconds (1f);
		ControllerSelectionPanel.SetActive (false);
	}
}
