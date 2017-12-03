//AIM - To check how much points should current ball Contains & set that Points to ScoreCount Variable & Redirect to ScoreUpdate() of GameController class.
//Added with Prefabs of all balls.
using UnityEngine;
using System.Collections;

public class BallManager : MonoBehaviour {
	
	private int scoreCount ;
	public bool scoreAdd; 
	public bool scoreDeduct;
	public bool bomb;
	public bool normal_ball;

	void Start()
	{//following if-else ladder is To Select how much Points will be given
		if (scoreAdd)
		{
			scoreCount = 5;
		}
		else if (scoreDeduct) 
		{
			scoreCount = -5;
		} 
		else if (bomb)
		{
			scoreCount = 0;
		}
		else if (normal_ball)
		{
			scoreCount = 1;
		}
	}

	public void OnCollisionEnter2D(Collision2D coll) //it will be triggered when Candy collides with any other collider.
	{
		if (coll.gameObject.tag == "Container") 
		{ //compare tag of Basket* here
			GameController.instance.ScoreUpdate (scoreCount); //this will jump to ScoreUpdate named method of gameController class.
			Destroy (gameObject); //gameObject is Ball here.
		} 
		else if (coll.gameObject.tag == "Die") 
		{ //chk for collider in grass here. DIE is tag given to collider in background named Sprite.
			if (!bomb && !scoreDeduct)//bomb should not be included for deadCount 
			{ //increment should be done in gameController because this current Script is associated with multiple objects so every time it initialize with 0 & no updation will be done.


				if (GameController.remainingCandy < 4) {
					Singleton.instance.sound.PlayOneShot(GameController.instance.colliderClip);//plays GameOver Sound.
				} else {
					Singleton.instance.sound.PlayOneShot (GameController.instance.lastCountDownClip);
				}

				Debug.Log ("" + GameController.isPause);
				if (GameController.isPause == false) {
					GameController.deadCount++;
					Debug.Log ("dead count " + GameController.deadCount);
				}
			}
			if (GameController.remainingCandy != 0) 
			{
				GameController.remainingCandy = GameController.instance.noOfCandytoDie - GameController.deadCount;

			}
			if (GameController.deadCount == GameController.instance.noOfCandytoDie)//|| bomb == true) 
			{
				Debug.Log ("dead done ");
				iTween.ShakePosition(Camera.main.gameObject,iTween.Hash("x",1.0f,"y",1.0f,"time",1.0f));
				GameController.instance.pauseButton.interactable = false;
				GameController.instance.gameisOver ();
				GameController.deadCount = 0;
				/*if (!bomb)
				{
					GameController.instance.remainingCandy = 0; //set 0 to remaining candy to die.
					iTween.ShakePosition(Camera.main.gameObject,iTween.Hash("x",1.0f,"y",1.0f,"time",1.0f));
				}*/
			}
			GameController.instance.remainBalls ();
			Destroy (gameObject);//gameObject is Ball here.
		} 
		else if (coll.gameObject.tag == "DestroyObj") 
		{ //vertical boundries at background
			Destroy (gameObject);//gameObject is Ball here.
		}
	}
}
