//AIM - To set bounds for movement of a basket* here.
//Added to GameObject Basket* here
using UnityEngine;
using CnControls;
using System.Collections;

public class BallCatcher : MonoBehaviour
{
	public static BallCatcher instance;
	public float moveSpeed;
	public float movement;
	public GameObject[] ballContainer;
	public GameObject Container;
	public GameObject cnInput;
	public int controller;

	void Awake()
	{
		instance = this;
		//Container = ballContainer [0];
		if (PlayerPrefs.GetInt ("Controller") == 1)  //to use CNinput control for movement of bucket
		{
			controller = 1;
		}
		else if(PlayerPrefs.GetInt ("Controller") == 2) //to use Acceleration control for movement of bucket
		{
			controller =2;
			cnInput.SetActive (false);
		}
		if (PlayerPrefs.GetInt ("bucket") == 1) 
		{ //bucket 1 is selected
			Container = ballContainer [0];
			ballContainer [1].SetActive (false);
			ballContainer [2].SetActive (false);
		}
		else if (PlayerPrefs.GetInt ("bucket") == 2) 
		{ //Bucket 2 is selected
			Container = ballContainer [1];
			ballContainer [2].SetActive (false);
			ballContainer [0].SetActive (false);
		}
		else if (PlayerPrefs.GetInt ("bucket") == 3) 
		{ //Bucket 3 is selected
			Container = ballContainer [2];
			ballContainer [1].SetActive (false);
			ballContainer [0].SetActive (false);
		}
	}
		
	void Update ()
	{
		if (controller== 1)
		{
			if (CnInputManager.GetAxis ("Horizontal") > 0 && Container.transform.position.x < 186) 
			{  //for right side movement
				movement = CnInputManager.GetAxis ("Horizontal") * moveSpeed;//CnInput is used for Cn Control buttons.[left n right arrow here]
				Container.transform.position = new Vector2 (Container.transform.position.x + movement, Container.transform.position.y);
			}
			else if (CnInputManager.GetAxis ("Horizontal") < 0 && Container.transform.position.x > 6) 
			{ //for left side movement
				movement = CnInputManager.GetAxis ("Horizontal") * moveSpeed; 
				Container.transform.position = new Vector2 (Container.transform.position.x + movement, Container.transform.position.y);
			}
		}
		if(controller==2)
		{
			if (Input.acceleration.x >0 && Container.transform.position.x < 186)  //for right side movement
			{
			movement = Input.acceleration.x * 17;//used with sensor of mobile
			Container.transform.position = new Vector2 (Container.transform.position.x + movement, Container.transform.position.y);
			} 
			else if(Input.acceleration.x < 0 && Container.transform.position.x > 6) //for left side movement
			{
			movement = Input.acceleration.x * 17;
			Container.transform.position = new Vector2 (Container.transform.position.x + movement, Container.transform.position.y);
			}
		}
	}
}