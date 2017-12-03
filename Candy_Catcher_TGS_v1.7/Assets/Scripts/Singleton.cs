//AIM -TO make gameObject Persistent Throughout the scenes.
using UnityEngine;
using System.Collections;

public class Singleton : MonoBehaviour {

	public static Singleton instance;
	public AudioSource sound;
	public AudioSource music;
	public int ex_Score= 0; //to store continous Score

	public int rewardVideoTry = 0;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
		else
		{
			Destroy(gameObject);
		}

	//	PlayerPrefs.DeleteAll ();
		if (PlayerPrefs.GetInt ("initialization") != 1) 
		{
			PlayerPrefs.SetInt("music",1);//background music ON
			PlayerPrefs.SetInt("sound",1);//Sound ON
			PlayerPrefs.SetInt("bucket",1);//Basket 1 selected by default
			PlayerPrefs.SetInt ("Controller",2);//Input arrows selected by default
			PlayerPrefs.SetInt("initialization", 1);
		}


		// check sound and set  
		if (PlayerPrefs.GetInt ("sound") == 1) {
			sound.mute = false;
		} else if (PlayerPrefs.GetInt ("sound") == 2) {
			sound.mute = true;
		}

		// check music and set  
		if (PlayerPrefs.GetInt ("music") == 1) {
			music.mute = false;
		} else if (PlayerPrefs.GetInt ("music") == 2) {
			music.mute = true;
		}
	}

}
