using UnityEngine;
using System.Collections;

public class Player  {
	public Player(PlayerHumanity ph, PlayerTurnState pts, Cell.CellState cs){
		Humanity = ph;
		TurnState = pts;
		CellState = cs;
	}

	public  enum PlayerHumanity {
		AI,
		Human
	}

	public enum PlayerTurnState {
		FirstPlayer,
		SecondPlayer,
		None
	}

	private PlayerHumanity Humanity;
	private PlayerTurnState TurnState;
	private Cell.CellState CellState;

	public void SetHumanity(PlayerHumanity value){
		Humanity = value;
	}
	public void SetTurnState(PlayerTurnState value){
		TurnState = value;
	}
	public void SetCellState(Cell.CellState value){
		CellState = value;
	}
	public PlayerHumanity GetHumanity(){
		return Humanity;
	}
	public PlayerTurnState GetTurnState(){
		return TurnState;
	}
	public Cell.CellState GetCellState(){
		return CellState;
	}
}
