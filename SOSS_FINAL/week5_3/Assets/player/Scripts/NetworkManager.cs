using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using SocketIO;
using UnityStandardAssets.Characters.ThirdPerson;

public class NetworkManager : MonoBehaviour {

	public static NetworkManager instance;
	public SocketIOComponent socket;
    public GameObject player;
    public GameObject map;
    public Canvas canvas;
    public GameObject Winner;
    public GameObject Loser;
    

    private Hashtable connectPlayer = new Hashtable();
    private List<String> color = new List<String>();
    private GameObject localPlayer;

    void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		//DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
		socket.On("other player connected", OnOtherPlayerConnected);
		socket.On("play", OnPlay);
		socket.On("player action", OnPlayerAction);
		socket.On("other player disconnected", OnOtherPlayerDisconnect);
        socket.On("time", OnTime);
        socket.On("gameRestart", OnGameRestart);
        JoinGame();
    }

    public void JoinGame()
	{
		StartCoroutine(ConnectToServer());
	}

	#region Commands
    void restart(float time)
    {
        fixMap();
        respawnAll();
        Count.timer = time;
        Count.doOnce = false;
        Count.canCount = true;
        
    }

    void respawnAll()
    {
        System.Random r = new System.Random();
        int min = 0;
        int max = 2;
        List<SpawnPoint> playerSpawnPoints = GetComponent<PlayerSpawner>().playerSpawnPoints;
        ICollection allPlayer = connectPlayer.Values;
        foreach(GameObject p in allPlayer)
        {
            int sp = r.Next(min, max);
            p.transform.position = playerSpawnPoints[sp].transform.position;
        }
    }
    void fixMap()
    {
        string envName = "Environment";
        if (GameObject.Find(envName) == null)
        {
            envName = envName + "(Clone)";
        }
        GameObject oldmap = GameObject.Find(envName);
        Destroy(oldmap);
        GameObject p = Instantiate(map, new Vector3(34.76214f, 59.97409f, 120.415f), Quaternion.Euler(0,0,0)) as GameObject;
    }
    public void end()
    {
        Position position = localPlayer.GetComponent<Position>();
        string envName = "Environment";
        if (GameObject.Find(envName) == null)
        {
            envName = envName + "(Clone)";
        }
        GameObject redBoat = GameObject.Find(envName).transform.FindChild("RedBoat").gameObject;
        GameObject blueBoat = GameObject.Find(envName).transform.FindChild("BlueBoat").gameObject;
        GameObject bridge = GameObject.Find(envName).transform.FindChild("Bridge").gameObject;
        Rigidbody redRigid = redBoat.GetComponent<Rigidbody>();
        Rigidbody blueRigid = blueBoat.GetComponent<Rigidbody>();
        Rigidbody bridegeRigid = bridge.GetComponent<Rigidbody>();
        bridegeRigid.isKinematic = false;
        if (number1.num1 > number2.num2)
        {
            blueRigid.isKinematic = false;
            if (position.getColor() == -1)
            {
                Loser.SetActive(true);
            }
            else
            {
                Winner.SetActive(true);
            }
        }
        else if(number1.num1 < number2.num2)
        {
            redRigid.isKinematic = false;
            if (position.getColor() == 1)
            {
                Loser.SetActive(true);
            }
            else
            {
                Winner.SetActive(true);
            }
        }
        else
        {
            blueRigid.isKinematic = false;
            redRigid.isKinematic = false;
            Loser.SetActive(true);
        }
    }
    public void respawn(string rename)
    {
        GameObject p = connectPlayer[rename] as GameObject;
        System.Random r = new System.Random();
        int min = 0;
        int max = 2;
        int sp = r.Next(min, max);
        List<SpawnPoint> playerSpawnPoints = GetComponent<PlayerSpawner>().playerSpawnPoints;
        Debug.Log(sp);
        p.transform.position = playerSpawnPoints[sp].transform.position;
        Rigidbody pRigid = p.GetComponent<Rigidbody>();
        pRigid.velocity = new Vector3(0, 0, 0);
    }

	IEnumerator ConnectToServer()
	{
		yield return new WaitForSeconds(1f);
		List<SpawnPoint> playerSpawnPoints = GetComponent<PlayerSpawner>().playerSpawnPoints;
		PlayerJSON playerJSON = new PlayerJSON(playerSpawnPoints);
		string data = JsonUtility.ToJson(playerJSON);
        socket.Emit("player connect");
        socket.Emit("play", new JSONObject(data));
		//canvas.gameObject.SetActive(false);
    }

    public void CommandAcction(Vector3 position, Quaternion rotation, Vector3 move, bool jump, bool crouch, bool kick)
	{
		string data = JsonUtility.ToJson(new ActionJSON(position, rotation, move, jump, crouch, kick));
		socket.Emit("playerAction", new JSONObject(data));
	}
    public void CommandRestart()
    {
        socket.Emit("gameRestart");
    }
    #endregion

    #region Listening
    void OnOtherPlayerConnected(SocketIOEvent socketIOEvent)
	{
		print("Someone else joined");
		string data = socketIOEvent.data.ToString();
		UserJSON userJSON = UserJSON.CreateFromJSON(data);
		Vector3 position = new Vector3(userJSON.position[0], userJSON.position[1], userJSON.position[2]);
		Quaternion rotation = Quaternion.Euler(userJSON.rotation[0], userJSON.rotation[1], userJSON.rotation[2]);
		if (connectPlayer.ContainsKey(userJSON.name))
		{
			return;
		}
        GameObject p = Instantiate(player, position, rotation) as GameObject;
        p.name = userJSON.name;
        connectPlayer.Add(userJSON.name, p);
        ThirdPersonUserControl pc = p.GetComponent<ThirdPersonUserControl>();
        pc.isLocal = false;
    }
	void OnPlay(SocketIOEvent socketIOEvent)
	{
		print("you joined");
		string data = socketIOEvent.data.ToString();
		UserJSON currentUserJSON = UserJSON.CreateFromJSON(data);
		Vector3 position = new Vector3(currentUserJSON.position[0], currentUserJSON.position[1], currentUserJSON.position[2]);
		Quaternion rotation = Quaternion.Euler(currentUserJSON.rotation[0], currentUserJSON.rotation[1], currentUserJSON.rotation[2]);
        GameObject p = Instantiate(player, position, rotation) as GameObject;
        p.name = currentUserJSON.name;
        connectPlayer.Add(currentUserJSON.name, p);
        localPlayer = p;
        ThirdPersonUserControl pc = p.GetComponent<ThirdPersonUserControl>();
        pc.isLocal = true;
    }
	void OnPlayerAction(SocketIOEvent socketIOEvent)
	{
		string data = socketIOEvent.data.ToString();
		reActionJSON actionJSON = reActionJSON.CreateFromJSON(data);
        //Vector3 Position = new Vector3(userJSON.position[0], userJSON.position[1], userJSON.position[2]);
        Vector3 move = new Vector3(actionJSON.move[0], actionJSON.move[1], actionJSON.move[2]);
        GameObject p = connectPlayer[actionJSON.name] as GameObject;
        ThirdPersonCharacter pc = p.GetComponent<ThirdPersonCharacter>();
        pc.Move(move, actionJSON.crouch, actionJSON.jump, actionJSON.kick);
        p.transform.position = new Vector3(actionJSON.position[0], actionJSON.position[1], actionJSON.position[2]);
        p.transform.rotation = Quaternion.Euler(actionJSON.rotation[0], actionJSON.rotation[1], actionJSON.rotation[2]);
    }
	void OnOtherPlayerDisconnect(SocketIOEvent socketIOEvent)
	{
		string data = socketIOEvent.data.ToString();
		UserJSON userJSON = UserJSON.CreateFromJSON(data);
        print(userJSON.name+" is disconnected");
        Destroy(connectPlayer[userJSON.name] as GameObject);
	}
    void OnTime(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        TimeJSON timeJSON = TimeJSON.CreateFromJSON(data);
        Count.timer = timeJSON.time;
        Count.doOnce = false;
    }
    void OnGameRestart(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        TimeJSON timeJSON = TimeJSON.CreateFromJSON(data);
        Winner.SetActive(false);
        Loser.SetActive(false);
        restart(timeJSON.time);
    }
    #endregion

    #region JSONMessageClasses
    [Serializable]
    public class ActionJSON
    {
        public float[] position;
        public float[] rotation;
        public float[] move;
        public bool jump;
        public bool crouch;
        public bool kick;

        public ActionJSON(Vector3 _position, Quaternion _rotation, Vector3 _move, bool _jump, bool _crouch, bool _kick)
        {
            position = new float[] { _position.x, _position.y, _position.z };
            rotation = new float[] { _rotation.eulerAngles.x, _rotation.eulerAngles.y, _rotation.eulerAngles.z };
            move = new float[] { _move.x, _move.y, _move.z };
            jump = _jump;
            crouch = _crouch;
            kick = _kick;
        }
        public static ActionJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<ActionJSON>(data);
        }
    }

    [Serializable]
    public class reActionJSON
    {
        public string name;
        public float[] position;
        public float[] rotation;
        public float[] move;
        public bool jump;
        public bool crouch;
        public bool kick;

        public reActionJSON(String _name, Vector3 _position, Quaternion _rotation, Vector3 _move, bool _jump, bool _crouch, bool _kick)
        {
            name = _name;
            position = new float[] { _position.x, _position.y, _position.z };
            rotation = new float[] { _rotation.eulerAngles.x, _rotation.eulerAngles.y, _rotation.eulerAngles.z };
            move = new float[] { _move.x, _move.y, _move.z };
            jump = _jump;
            crouch = _crouch;
            kick = _kick;
        }
        public static reActionJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<reActionJSON>(data);
        }
    }

    [Serializable]
	public class PlayerJSON
	{
		public List<PointJSON> playerSpawnPoints;

		public PlayerJSON(List<SpawnPoint> _playerSpawnPoints)
		{
			playerSpawnPoints = new List<PointJSON>();
			foreach (SpawnPoint playerSpawnPoint in _playerSpawnPoints)
			{
				PointJSON pointJSON = new PointJSON(playerSpawnPoint);
				playerSpawnPoints.Add(pointJSON);
			}
		}
	}

	[Serializable]
	public class PointJSON
	{
		public float[] position;
		public float[] rotation;
		public PointJSON(SpawnPoint spawnPoint)
		{
			position = new float[] {
				spawnPoint.transform.position.x,
				spawnPoint.transform.position.y,
				spawnPoint.transform.position.z
			};
			rotation = new float[] {
				spawnPoint.transform.eulerAngles.x,
				spawnPoint.transform.eulerAngles.y,
				spawnPoint.transform.eulerAngles.z
			};
		}
	}

	[Serializable]
	public class UserJSON
	{
		public string name;
		public float[] position;
		public float[] rotation;
		public int health;

		public static UserJSON CreateFromJSON(string data)
		{
			return JsonUtility.FromJson<UserJSON>(data);
		}
	}

    [Serializable]
    public class TimeJSON
    {
        public float time;

        public static TimeJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<TimeJSON>(data);
        }
    }
    #endregion
}
