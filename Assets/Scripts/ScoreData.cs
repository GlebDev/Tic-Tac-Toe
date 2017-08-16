using UnityEngine;
using System.Collections;

[System.Serializable]
public class ScoreData{
	
	public int WinCount,DefeatCount,DrawCount;
	public Player.PlayerHumanity PlayerWhoTurnFirst;

	public ScoreData(){
		WinCount = 0;
		DefeatCount = 0;
		DrawCount = 0;
		PlayerWhoTurnFirst =  Player.PlayerHumanity.Human;
	}

	public ScoreData(int win, int defeat, int draw, Player.PlayerHumanity humanity){
		WinCount = win;
		DefeatCount = defeat;
		DrawCount = draw;
		PlayerWhoTurnFirst =  humanity;
	}

}


