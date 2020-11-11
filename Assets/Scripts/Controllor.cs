using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controllor : MonoBehaviour,ISceneControllor,IUserAction
{
	public IActionManager iam;
	public DiskFactory df;
	public UserGUI ug;
	public ScoreRecorder sr;
	public RoundControllor rc;

	private Queue<GameObject> dq = new Queue<GameObject> ();
	private List<GameObject> dfree = new List<GameObject> ();
	private GameObject explosion;
	private float emit_time = 3;
	private int round = 1;
	private float t = 1;
	private float speed = 2;
	private bool game_over = false;  
	private bool counting = true;
	public bool modeSetting = true;
	public bool isCounting(){return counting;}
	public int getEmitTime(){return (int)emit_time+1;}
	public void Restart (){SceneManager.LoadScene(0);}
	public int GetScore (){return sr.score;}
	public int GetRound (){return round;}
	public void modeSet (bool flag){mode = flag;}
	public void gameBegin (){modeSetting = false;}
	public bool getModeSetting (){return modeSetting;}
	public void GameOver (){game_over = true;}
	public bool mode = false;
	void Start(){
		GameDirector director = GameDirector.GetInstance();     
		director.CurrentSceneControllor = this;             
		df = Singleton<DiskFactory>.Instance;
		sr = gameObject.AddComponent<ScoreRecorder> () as ScoreRecorder;
		iam = gameObject.AddComponent<ActionManagerAdapter>() as IActionManager;
		ug = gameObject.AddComponent<UserGUI>() as UserGUI;
		rc = gameObject.AddComponent<RoundControllor> () as RoundControllor;
		explosion = Instantiate (Resources.Load<GameObject> ("Prefabs/ParticleSystemG"), new Vector3(0, -100, 0), Quaternion.identity);
		t = speed;
	}
	void Update ()
	{	if (modeSetting == false) {
			if (emit_time > 0) {
				counting = true;
				emit_time -= Time.deltaTime;
			} else {
				counting = false;
				t-=Time.deltaTime;
				if (t < 0) {
					if (game_over == false) {
						LoadResources ();
						SendDisk ();
						t = speed;	
					}
				}
				if ((sr.score >= 10 && round == 1) || (sr.score >= 30 && round == 2)) {
					round++;
					rc.loadRoundData (round);
				}
			}	
		}
	}
	public void setting(float speed_,GameObject explosion_)
	{
		speed = speed_;	
		explosion = explosion_;
	}
	public void LoadResources()
	{
		dq.Enqueue(df.GetDisk(round)); 
	}

	private void SendDisk()
	{
		float position_x = mode ? 16 : 11;                       
		if (dq.Count != 0)
		{
			GameObject disk = dq.Dequeue();
			dfree.Add(disk);
			disk.SetActive(true);
			float ran_y = mode ? Random.Range(1f, 4f) : Random.Range(3f, 4f);
			float ran_x = Random.Range(-1f, 1f) < 0 ? -1 : 1;
			disk.GetComponent<DiskData>().direction = new Vector3(ran_x, ran_y, 0);
			Vector3 position = new Vector3(-disk.GetComponent<DiskData>().direction.x * position_x, ran_y, 0);
			disk.transform.position = position;
			float power = mode ? Random.Range(20f, 30f) : Random.Range(3.5f, 5f);
			float angle = mode ? Random.Range(5f, 13f) : Random.Range(20f, 35f);
			iam.UFOFly(disk,angle,power,mode);
		}

		for (int i = 0; i < dfree.Count; i++)
		{
			GameObject temp = dfree[i];
			if (temp.transform.position.y < -10 && temp.gameObject.activeSelf == true)
			{
				df.FreeDisk(dfree[i]);
				dfree.Remove(dfree[i]);
				ug.ReduceBlood();
			}
		}
	}
	public void Hit (Vector3 pos){
		Ray ray = Camera.main.ScreenPointToRay(pos);
		RaycastHit[] hits;
		hits = Physics.RaycastAll(ray);
		bool not_hit = false;
		for (int i = 0; i < hits.Length; i++)
		{
			RaycastHit hit = hits[i];
			if (hit.collider.gameObject.GetComponent<DiskData>() != null)
			{
				for (int j = 0; j < dfree.Count; j++)
				{
					if (hit.collider.gameObject.GetInstanceID() == dfree[j].gameObject.GetInstanceID())
					{
						not_hit = true;
					}
				}
				if(!not_hit)
				{
					return;
				}
				dfree.Remove(hit.collider.gameObject);
				sr.Record(hit.collider.gameObject);

				explosion.transform.position = hit.collider.gameObject.transform.position;
				explosion.GetComponent<ParticleSystem>().Play();
				hit.collider.gameObject.transform.position = new Vector3(0, -100, 0);
				df.FreeDisk(hit.collider.gameObject);
				break;
			}
		}
	}
}