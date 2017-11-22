using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour {
	static public int numberOfPlayers = 4;
	static public int numberOfInitialCards = 7;
    static public GameManager shared;
	static public Player[] players {
        get {
            return playerHolder.GetComponentsInChildren<Player>();
        }
    }

    static public GameObject continueUI;
    static public GameObject victoryUI;
    static public GameObject playerHolder;

    static int playerIndex = 0;
	static int direction = 1;	// init direction 1, reversed = -1

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
        playerHolder = GameObject.Find("PlayerHolder");
	}

	// Use this for initialization
    public void Start() {
        for(int i = 0; i < numberOfPlayers; i++) {
            var player = GameObject.Instantiate(Resources.Load<GameObject>("Player"));
            player.transform.SetParent(playerHolder.transform);
            player.transform.localPosition = Vector3.zero;
            player.GetComponent<Player>().playerID = i;
        }
        StartGame();
    }

	// Update is called once per frame
    public void Update() {
        if(Input.GetKeyDown(KeyCode.Return) && continueUI) {
            OnContinueUIClick();
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
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
    public static GameObject createUI(string text, string btnText) {
        GameObject createdUI = Resources.Load<GameObject>("Press Continue");
        createdUI = Instantiate<GameObject>(createdUI);
        createdUI.transform.SetParent(playerHolder.transform);
        createdUI.transform.SetAsLastSibling();
        createdUI.transform.localPosition = new Vector3(0, 300, 0);
        createdUI.GetComponentInChildren<Text>().text = text;
        createdUI.transform.GetChild(1).GetComponentInChildren<Text>().text = btnText;
        createdUI.GetComponentInChildren<Button>().onClick.AddListener(GameManager.OnContinueUIClick);
        return createdUI;
    }

    // check for win
    public static void CheckForWin() {
        if(CurrentPlayer.cardViews.Count == 0) {
            Debug.Log("game ended");
            victoryUI = createUI("player " + PlayerIndex + " wins", "restart");
            victoryUI.GetComponentInChildren<Button>().onClick.AddListener(StartNewGame);
        }
    }

    // continue playing
    public static void OnContinueUIClick() {
        Destroy(continueUI);
    }

    public static void ShowContinueUI(int playerID) {
        GameManager.continueUI = GameManager.createUI("END OF PLAYER " + playerID + "'S TURN", "continue");
    }

    // change current player
    public static void ToggleNextPlayer(int nextPlayer = 1) {
		PlayerIndex += nextPlayer * direction;
		CurrentPlayer.BeginTurn();
    }

    // reverse card action
    public static void Reverse() {
        direction *= -1;
        GameManager.CurrentPlayer.EndTurn();
        ToggleNextPlayer();
    }

    // skip card action
    public static void Skip() {
        Debug.Log("Skips " + PlayerIndex + direction);
        GameManager.CurrentPlayer.EndTurn();
        GameManager.ToggleNextPlayer(2);
    }

    // reload scene
    public static void StartNewGame() {
        SceneManager.LoadScene("2Dgame");
    }

    // modulus helper for advance to next player
    public static int mod(int x, int m) {
        return (x%m + m)%m;
    }
}