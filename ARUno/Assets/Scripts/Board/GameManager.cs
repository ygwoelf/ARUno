using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour {
	readonly static public int numberOfPlayers = 4;
	readonly static public int numberOfInitialCards = 7;
    readonly static public int unoPenalty = 2;
    static public GameManager shared;
	static public Player[] players {
        get {
            return playerHolder.GetComponentsInChildren<Player>();
        }
    }

    static public GameObject victoryUI;
    static public GameObject playerHolder;

    static int playerIndex = 0;
	static int direction = 1;	// init direction 1, reversed = -1
    
    // For Progressive Uno
    public static int ProgressiveUnoPenalty = 0;

    // For AR
	private int appMode = 0;
    public static bool IsPaused { get; set; }

	// current player index
    public static int PlayerIndex {
        get {
			return playerIndex;
		}
        set {
			playerIndex = (value + players.Length) % players.Length;
        }
    }

    // current player object
    public static Player CurrentPlayer {
        get {
            return players[PlayerIndex];
        }
    }

    // next player object
    public static Player NextPlayer {
        get {
			return players[mod(PlayerIndex + direction, players.Length)];
        }
    }

	// Awake is called when the script instance is being loaded.
	void Awake() {
        shared = this;
        IsPaused = false;
        playerHolder = GameObject.Find("PlayerHolder");
	}

	// Use this for initialization
    void Start() {
        for(int i = 0; i < numberOfPlayers; i++) {
            var player = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player"), playerHolder.transform);
            player.transform.localPosition = Vector3.zero;
            player.GetComponent<Player>().playerID = i;
        }
        StartGame();
    }

	// Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

	void OnGUI() {
        // turn on/off AR
		string modeString = appMode == 0 ? "AR" : "Normal";
		if (GUI.Button(new Rect(Screen.width -150.0f, 0.0f, 150.0f, 100.0f), modeString)) {
			appMode = (appMode + 1) % 2;
            Image image = CurrentCard.shared.gameObject.GetComponentInChildren<Image>();
            image.enabled = !image.enabled;
		}
        // "uno" house rule
        foreach(Player p in players) {
            if (!p.didCallUno && p.cardCount == 1) {
                // if player then set didCallUno, else draw 2
                if (GUI.Button(new Rect(Screen.width -150.0f, 100.0f, 150.0f, 100.0f), "UNO!")) {
                    if (p.playerID == EasyUnoAI.playerControlledIndex) {
                        p.didCallUno = true;
                    } else {
                        p.Draw(unoPenalty);
                    }
                }
                break;
            }
        }
	}

    // start the game
    public static void StartGame() {
        PlayerIndex = 0;
        // set first turn for all players true
        foreach(Player p in players) {
            p.Draw(numberOfInitialCards);  // initial hand for each player
        }
        CurrentPlayer.BeginTurn();
    }

    // helper to create text ui
    public static GameObject CreateUI(string text, string btnText) {
        GameObject createdUI = Resources.Load<GameObject>("Prefabs/AlertMenu");
        createdUI = Instantiate<GameObject>(createdUI, playerHolder.transform);
        createdUI.transform.SetAsLastSibling();
        createdUI.transform.localPosition = new Vector3(0, 300, 0);
        createdUI.GetComponentInChildren<Text>().text = text;
        createdUI.transform.GetChild(1).GetComponentInChildren<Text>().text = btnText;
        return createdUI;
    }

    // check for win
    public static void CheckForWin() {
        foreach (Player player in players) {
            if(player.cardViews.Count == 0) {
                IsPaused = true;
                victoryUI = CreateUI("Player " + player.playerID + " wins", "Restart");
                victoryUI.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(StartNewGame);
                victoryUI.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(Application.Quit);
            }
        }
    }

    // change current player
    public static void ToggleNextPlayer(int nextPlayer = 1) {
		PlayerIndex += nextPlayer * direction;
		CurrentPlayer.BeginTurn();
    }

    // reverse card action
    public static void Reverse() {
        direction *= -1;
        ToggleNextPlayer();
    }

    // skip card action
    public static void Skip() {
        GameManager.ToggleNextPlayer(2);
    }

    // reload scene
    public static void StartNewGame() {
        SceneManager.LoadScene("2Dgame");
    }

    // call "UNO!"
    public static void CallUno(int playerIndex) {
        foreach(Player p in players) {
            if (!p.didCallUno && p.cardCount == 1) {
                if (p.playerID == playerIndex) {
                    p.didCallUno = true;
                } else {
                    p.Draw(unoPenalty);
                }
            }
        }
    }

    // modulus helper for advance to next player
    public static int mod(int x, int m) {
        return (x%m + m)%m;
    }
}