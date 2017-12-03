// This plugin manager and game is made by Thunder Game Studio - TGS = info@thundergamestudio.com
// Thanks for using game -fno-objc-arc Hello World....!!!!

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

// google ad import
using GoogleMobileAds.Api;

// leaderboard import
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
//unity ad manager
using UnityEngine.SocialPlatforms;

public class PluginManager : MonoBehaviour
{

	[SerializeField]
	public static PluginManager _insta;

	public bool keepAwakeScreen;
	public bool set60FPS;

	public bool enableUnityRewardAds;
	public int numberTryWithVideo = 1;


	public bool enableGoogleAds;
	public string bannerIdIOS, interstitialIdIOS, bannerIdAndroid, interstitialIdAndroid;


	public bool enableGoogleAnalytics;
	public string GoogleAnalyticsIdIOS, GoogleAnalyticsIdAndroid;

	public bool enableRemoveAds;
	public string inAppRemoveAdID;
	//android test id "android.test.purchased" com.game.removeads
	public string publicKeyAndroid;


	public bool enableLeaderBoard;
	public string leaderboardIdIOS, leaderboardIdAndroid;


	public bool enableSharingFeature;
	public static string AndroidLink;
	public string iOSLink = "https://itunes.apple.com/app/id";
	public bool enableRateUs;


	void Awake ()
	{

		//PlayerPrefs.DeleteAll ();
		if (_insta == null) {
			_insta = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}

		if (keepAwakeScreen) {
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
		}

		if (set60FPS) { // set framerate to 60 fps
			Application.targetFrameRate = 60;
		}


		//set link for android
		if (enableRateUs) {
			AndroidLink = "https://play.google.com/store/apps/details?id=" + Application.identifier.ToString ();
		}

		/// google ads
		loadBanner ();
		loadInterstitial ();

	}

	void Start ()
	{
		//leaderboard
		authenticateUser ();
	}

	void OnEnable ()
	{
		Debug.Log ("on enable");
	}

	/// Application Close without FC
	public void AppQuit ()
	{
		#if UNITY_ANDROID
		if (enableLeaderBoard) {
			//((GooglePlayGames.PlayGamesPlatform)Social.Active).SignOut ();
			AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer"); 
			AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject> ("currentActivity"); 
			activity.Call<bool> ("moveTaskToBack", true);
			Application.Quit ();
		}
		#endif
		Application.Quit ();
	}


	///////////// LEADERBOARD STUFF ///////////////////
	private void authenticateUser ()
	{
		if (enableLeaderBoard) {
			

			#if UNITY_ANDROID
			PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder ()
				.Build ();

			PlayGamesPlatform.InitializeInstance (config);
			// recommended for debugging:
			PlayGamesPlatform.DebugLogEnabled = true;
			// Activate the Google Play Games platform
			PlayGamesPlatform.Activate ();
			#endif

			Social.localUser.Authenticate (success => {
				if (success) {
					Debug.Log ("Authentication successful");

					string userInfo = "Username: " + Social.localUser.userName +
					                  "\nUser ID: " + Social.localUser.id +
					                  "\nIsUnderage: " + Social.localUser.underage;

					Debug.Log (userInfo);

					Loadleaderboard ();

				} else
					Debug.Log ("Authentication failed");
			});
		}

	}

	private void Loadleaderboard ()
	{
		#if UNITY_ANDROID
		Social.LoadScores (leaderboardIdAndroid, success => {
			ProcessLoadedScores (success);
		});
		#else
		Social.LoadScores (leaderboardIdIOS, success => {
			ProcessLoadedScores (success);
		});
		#endif

	}

	private void ProcessLoadedScores (IScore[] scores)
	{
		Debug.Log ("load scores");
	}

	//report score
	public void ReportScore (int score)
	{
		if (enableLeaderBoard) {
			#if UNITY_ANDROID
			Debug.Log ("Reporting score " + (long)score + " on leaderboard " + leaderboardIdAndroid);
			Social.ReportScore ((long)score, leaderboardIdAndroid, success => {
				Debug.Log (success ? "Reported score successfully" : "Failed to report score");
			});
			#else
			Debug.Log ("Reporting score " + score + " on leaderboard " + leaderboardIdIOS);
			Social.ReportScore (score, leaderboardIdIOS, success => {
				Debug.Log (success ? "Reported score successfully" : "Failed to report score");
			});
			#endif
			

		}
	}

	// show leaderboard ui
	public void showLeaderboard ()
	{
		#if UNITY_ANDROID
		// show leaderboard UI
		PlayGamesPlatform.Instance.ShowLeaderboardUI (leaderboardIdAndroid);
		#else
		Social.ShowLeaderboardUI ();
		#endif



	}


	///////////// RATE US LINK ///////////////////
	public void openRateUs ()
	{
		#if UNITY_ANDROID
		Application.OpenURL (AndroidLink);
		#else
		Application.OpenURL (iOSLink);
		#endif

	}

	///////////// GOOGLE ANALYTICS  ///////////////////
	public void LogScreen (string title)
	{
		if (enableGoogleAnalytics) {
			string screenRes = Screen.width + "x" + Screen.height;

			string clientID = SystemInfo.deviceUniqueIdentifier;

			title = WWW.EscapeURL (title);
			#if UNITY_ANDROID
			var url = "https://www.google-analytics.com/collect?v=1&ul=en-us&t=appview&sr=" + screenRes + "&an=" + WWW.EscapeURL (Application.productName.ToString ()) + "&a=448166238&tid=" + GoogleAnalyticsIdAndroid + "&aid=" + Application.identifier.ToString () + "&cid=" + WWW.EscapeURL (clientID) + "&_u=.sB&av=" + Application.version.ToString () + "&_v=ma1b3&cd=" + title + "&qt=600&z=185";
			WWW request = new WWW (url);
			#else
			var url = "https://www.google-analytics.com/collect?v=1&ul=en-us&t=appview&sr=" + screenRes + "&an=" + WWW.EscapeURL (Application.productName.ToString ()) + "&a=448166238&tid=" + GoogleAnalyticsIdIOS + "&aid=" + Application.identifier.ToString () + "&cid=" + WWW.EscapeURL (clientID) + "&_u=.sB&av=" + Application.version.ToString () + "&_v=ma1b3&cd=" + title + "&qt=600&z=185";
			WWW request = new WWW (url);
			//Debug.Log (url);
			#endif

			Debug.Log (url + " and " + request);

		}

	}


	///////////// GOOGLE ADS ///////////////////
	private BannerView bannerView;
	private InterstitialAd interstitial;

	public void showInterstitial ()
	{
		if (enableGoogleAds && PlayerPrefs.GetInt ("removeAd") != 1) {
			if (interstitial.IsLoaded ()) {
				interstitial.Show ();
			}
		}
	}

	public void loadInterstitial ()
	{
		if (enableGoogleAds && PlayerPrefs.GetInt ("removeAd") != 1) {

			RequestInterstitial ();

		}
	}

	public void loadBanner ()
	{
		if (enableGoogleAds && PlayerPrefs.GetInt ("removeAd") != 1) {
			RequestBanner ();
		}
	}

	//destroy ads
	public void destroyAds ()
	{
		if (enableGoogleAds) {
			bannerView.Hide ();
			bannerView.Destroy ();
			interstitial.Destroy ();
		}
	}

	private void RequestBanner ()
	{
		#if UNITY_ANDROID
		string adUnitId = bannerIdAndroid;
		#elif UNITY_IPHONE
		string adUnitId = bannerIdIOS;
		#else
		string adUnitId = "unexpected_platform";
		#endif

		// Create a 320x50 banner at the top of the screen.
		bannerView = new BannerView (adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder ().Build ();
		// Load the banner with the request.
		bannerView.LoadAd (request);

		bannerView.Show ();
	}

	private void RequestInterstitial ()
	{
		#if UNITY_ANDROID
		string adUnitId = interstitialIdAndroid;
		#elif UNITY_IPHONE
		string adUnitId = interstitialIdIOS;
		#else
		string adUnitId = "unexpected_platform";
		#endif

		// Initialize an InterstitialAd.
		interstitial = new InterstitialAd (adUnitId);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder ().Build ();
		// Load the interstitial with the request.
		interstitial.LoadAd (request);
	}


	///////// SHARING FEATURE //////////////
	private string ScreenshotName = "screenshot.png";

	public void ShareScreenshotWithText (string text)
	{
		string screenShotPath = Application.persistentDataPath + "/" + ScreenshotName;
		ScreenCapture.CaptureScreenshot (ScreenshotName);

		Share (text, screenShotPath, "");
	}

	public void Share (string shareText, string imagePath, string url, string subject = "")
	{
		#if UNITY_ANDROID
		AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");
		AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");

		intentObject.Call<AndroidJavaObject> ("setAction", intentClass.GetStatic<string> ("ACTION_SEND"));
		AndroidJavaClass uriClass = new AndroidJavaClass ("android.net.Uri");
		AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject> ("parse", "file://" + imagePath);
		intentObject.Call<AndroidJavaObject> ("putExtra", intentClass.GetStatic<string> ("EXTRA_STREAM"), uriObject);
		intentObject.Call<AndroidJavaObject> ("setType", "image/png");

		intentObject.Call<AndroidJavaObject> ("putExtra", intentClass.GetStatic<string> ("EXTRA_TEXT"), shareText);

		AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject> ("currentActivity");

		AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject> ("createChooser", intentObject, subject);
		currentActivity.Call ("startActivity", jChooser);
		#elif UNITY_IOS
		CallSocialShareAdvanced (shareText, subject, url, imagePath);
		#else
		Debug.Log("No sharing set up for this platform.");
		#endif
	}

	#if UNITY_IOS
	public struct ConfigStruct
	{
		public string title;
		public string message;
	}

	[DllImport ("__Internal")] private static extern void showAlertMessage (ref ConfigStruct conf);

	public struct SocialSharingStruct
	{
		public string text;
		public string url;
		public string image;
		public string subject;
	}

	[DllImport ("__Internal")] private static extern void showSocialSharing (ref SocialSharingStruct conf);

	public static void CallSocialShare (string title, string message)
	{
		ConfigStruct conf = new ConfigStruct ();
		conf.title = title;
		conf.message = message;
		showAlertMessage (ref conf);
	}


	public static void CallSocialShareAdvanced (string defaultTxt, string subject, string url, string img)
	{
		SocialSharingStruct conf = new SocialSharingStruct ();
		conf.text = defaultTxt;
		conf.url = url;
		conf.image = img;
		conf.subject = subject;


		showSocialSharing (ref conf);
	}
	#endif
}
