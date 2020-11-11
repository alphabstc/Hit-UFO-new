using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneControllor{
	void LoadResources ();
}

public interface IUserAction{
	void Hit (Vector3 pos);
	void Restart ();
	int GetScore();
	int GetRound();
	void GameOver ();
	bool isCounting();
	int getEmitTime ();
	void modeSet (bool flag);
	void gameBegin ();
	bool getModeSetting ();
	void setting(float speed,GameObject explosion);
}
public interface IActionManager
{
	void UFOFly(GameObject disk, float angle, float power,bool isPhy);
}