using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class TTTController:MonoBehaviour {

	public static AI.Difficulty difficulty = AI.Difficulty.Invincible;
	[SerializeField] private Cell CellPrefab;
	[SerializeField] private Transform GameArea;
	[SerializeField] private GameObject HumanTurnText,AITurnText,ResultPanel,PauseMenu;
	[SerializeField] private Text ResultText,ScoreText;
	[SerializeField] private GameArea gameArea;
	[SerializeField] private string sceneName;
	private AI curAI;
	private ScoreData scoreData = new ScoreData ();
	private Player HumanPlayer = new Player (Player.PlayerHumanity.Human, Player.PlayerTurnState.FirstPlayer, Cell.CellState.X);
	private Player AIPlayer = new Player (Player.PlayerHumanity.AI, Player.PlayerTurnState.SecondPlayer,Cell.CellState.O);
	private Player NonePlayer = new Player (Player.PlayerHumanity.AI, Player.PlayerTurnState.None,Cell.CellState.NONE);
	private Player playerWhoTurn,playerWhoWon;

	private void Start(){
		SetGameAreaSize (Screen.height);
		gameArea.SetCellsArray(CreateCells ());
		curAI = new AI(gameArea.GetCellsArray(),difficulty,HumanPlayer.GetCellState(),AIPlayer.GetCellState());
		StartGame ();
	}

	private void Update(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			TooglePauseMenuVisible ();
		}
	}

	private void SetGameAreaSize(int ScreenHeight){
		gameArea.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(ScreenHeight/1.5f,ScreenHeight/1.5f);
		gameArea.gameObject.GetComponent<GridLayoutGroup> ().cellSize = new Vector2(ScreenHeight/4.5f,ScreenHeight/4.5f);
		PauseMenu.GetComponent<RectTransform> ().sizeDelta = new Vector2 (ScreenHeight/1.5f,0);

	}
		
	private Cell[,] CreateCells(){
		Cell[,] cells = new Cell[3 , 3];
		for(int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++) {
				cells [j, i] = Instantiate (CellPrefab, GameArea) as Cell;
				cells [j, i].SetState (Cell.CellState.NONE);
				cells [j, i].Controller = this;
			}
		}
		return cells;
	}

	public void CellClick(Cell cell){
		if (cell != null) {
			cell.SetState (GetPlayerWhoTurn ().GetCellState ());
		}
		CheckResult (gameArea.GetCellsArray());
		TogglePlayerWhoTurn ();
		TogglePlayersTurnText ();
	}

	public void SetPlayerWhoTurn(Player value){
		playerWhoTurn = value;
		if (value == AIPlayer) {
			CellClick(curAI.MakeAMove ());
		}
	}

	public Player GetPlayerWhoTurn(){
		return playerWhoTurn;
	}

	private void TogglePlayersTurnText(){
		switch (GetPlayerWhoTurn ().GetHumanity()) {
		case Player.PlayerHumanity.Human:
			HumanTurnText.SetActive (true);
			AITurnText.SetActive (false);
			break;
		case  Player.PlayerHumanity.AI:
			HumanTurnText.SetActive (false);
			AITurnText.SetActive (true);
			break;
		default:
			Debug.Log ("Erorr");
			break;
		}
	}

	private void TogglePlayerWhoTurn(){
		if (GetPlayerWhoTurn () == AIPlayer) {
			SetPlayerWhoTurn (HumanPlayer);
		} else if (GetPlayerWhoTurn () == HumanPlayer) {
			SetPlayerWhoTurn (AIPlayer);
		}
	}

	private void CheckResult(Cell[,] cells){
		Cell cellTmp;
		int horizontal;
		for(int i = 0; i < 3; i++)
		{
			if(cells[0,i].GetState() == cells[1,i].GetState() && cells[0,i].GetState() == cells[2,i].GetState() && cells[0,i].GetState() != Cell.CellState.NONE){
				SetPlayerWhoWonByCellState(cells[0,i].GetState());
				ShowResult  ();
				return;
			}
			if (cells [i, 0].GetState () == cells [i, 1].GetState () && cells [i, 0].GetState () == cells [i, 2].GetState () && cells [i, 0].GetState () != Cell.CellState.NONE) {
				SetPlayerWhoWonByCellState(cells [i, 0].GetState ());
				ShowResult  ();
				return;
			}
		}
		if ((cells [0, 0].GetState () == cells [1, 1].GetState () && cells [0, 0].GetState () == cells [2, 2].GetState () && cells [0, 0].GetState () != Cell.CellState.NONE) || (cells [0, 2].GetState () == cells [1, 1].GetState () && cells [0, 2].GetState () == cells [2, 0].GetState () && cells [0, 2].GetState () != Cell.CellState.NONE)) {
			SetPlayerWhoWonByCellState (cells [1, 1].GetState ());
			ShowResult  ();
			return;
		}
		if(IsDraw (cells)) {
			playerWhoWon = NonePlayer;
			ShowResult  ();
			return;
		}
	}

	private void SetPlayerWhoWonByCellState(Cell.CellState value){
		if (value == HumanPlayer.GetCellState ()) {
			playerWhoWon = HumanPlayer;
		}else if(value == AIPlayer.GetCellState ()){
			playerWhoWon = AIPlayer;
		}
	}
	private bool IsDraw(Cell[,] cells){
		
		for(int i = 0; i < 3; i++)
		{
			for(int j = 0; j < 3; j++)
			{
				if (cells [j, i].GetState () == Cell.CellState.NONE) {
					return false;
				}
			}
		}
		return true;
	}
	private void ShowResult (){
		SetPlayerWhoTurn (NonePlayer);
		string ResultLabel = string.Empty;
		if (playerWhoWon == AIPlayer) {
			scoreData.DefeatCount++;
			scoreData.PlayerWhoTurnFirst = playerWhoWon.GetHumanity();
			ResultLabel = "YOU LOSE";
		}else if(playerWhoWon == HumanPlayer){
			scoreData.WinCount++;
			scoreData.PlayerWhoTurnFirst = playerWhoWon.GetHumanity();
			ResultLabel = "YOU WIN";
		}else if(playerWhoWon == NonePlayer){
			ResultLabel = "TIE";
			scoreData.DrawCount++;
		}
		ResultPanel.SetActive (true);
		ResultText.text = ResultLabel;
		ScoreDataManager.SaveScoreData (scoreData);
		SetScoreDataText (scoreData.WinCount.ToString(),scoreData.DefeatCount.ToString(),scoreData.DrawCount.ToString());
	}

	public void StartGame(){
		ClearGameArea ();
		scoreData = ScoreDataManager.LoadScoreData ();
		SetScoreDataText (scoreData.WinCount.ToString(),scoreData.DefeatCount.ToString(),scoreData.DrawCount.ToString());
		ResultPanel.SetActive (false);
		switch(scoreData.PlayerWhoTurnFirst){
		case Player.PlayerHumanity.Human:
			SetPlayerWhoTurn (HumanPlayer);
			break;
		case Player.PlayerHumanity.AI:
			SetPlayerWhoTurn (AIPlayer);
			break;
		default :
			Debug.Log ("Erorr");
			break;
		}

		TogglePlayersTurnText ();
	}

	private void ClearGameArea(){
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
				gameArea.GetCell(j, i).SetState (Cell.CellState.NONE);
			}
		}
	}

	public void TooglePauseMenuVisible(){
		PauseMenu.SetActive (!PauseMenu.activeSelf);
	}

	public void ExitGame(){
		Application.Quit();
	}
		
	private void SetScoreDataText (string win_count,string defeat_count,string draw_count){
		if (ScoreText) {
			ScoreText.text = "WINS: " + win_count + "   DEFEATS: " + defeat_count + "   TIE: " + draw_count;
		}
	}
	public void LoadLevel(){
		SceneManager.LoadScene(sceneName);
	}


}
