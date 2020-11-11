using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundControllor : MonoBehaviour {
	private IUserAction action;
	private float speed;
	private GameObject explosion;
	void Start(){
		action = GameDirector.GetInstance().CurrentSceneControllor as IUserAction;
		speed = 2;
	}
	public void loadRoundData(int round)
	{
		
		switch (round)
		{
		case 1:    

			break;
		case 2:    
			
			speed = 1.5f;
			explosion = Instantiate (Resources.Load<GameObject> ("Prefabs/ParticleSystemY"), new Vector3(0, -100, 0), Quaternion.identity);
			action.setting (speed,explosion);
			break;
		case 3:
			
			speed = 1;
			explosion = Instantiate (Resources.Load<GameObject> ("Prefabs/ParticleSystemR"), new Vector3(0, -100, 0), Quaternion.identity);
			action.setting (speed,explosion);
			break;
		}
	}
}
