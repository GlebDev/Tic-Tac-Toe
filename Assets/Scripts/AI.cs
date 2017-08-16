using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour {

	private Cell[,] board;
	private Cell.CellState HumanPlayerCellState,AIPlayerCellState;
	private Difficulty DifficultyLevel;
	private int RecurtionDepth,RecurtionMaxDepth;
	private Move BstMove;
	public enum Difficulty
	{
		Easy,
		Medium,
		Invincible
	}
	private class Move{
		public float score;
		public Cell cell;
		public Move(){
		}
		public Move(int scoreValue ,Cell cellValue){
			score = scoreValue;
			cell = cellValue;
		}
	}
	public AI(Cell[,] cellsLink, Difficulty difficulty_level,Cell.CellState _humanPlayerCellState,Cell.CellState _aIPlayerCellState){
		board = cellsLink;
		DifficultyLevel = difficulty_level;
		HumanPlayerCellState = _humanPlayerCellState;
		AIPlayerCellState = _aIPlayerCellState;
		switch(DifficultyLevel){
		case Difficulty.Easy:
			RecurtionMaxDepth = 1;
			break;
		case Difficulty.Medium:
			RecurtionMaxDepth = 3;
			break;
		case Difficulty.Invincible:
			RecurtionMaxDepth = 9;
			break;
		default:
			RecurtionMaxDepth = 1;
			break;
		}
	}

	public void SetPlayersCellState(Cell.CellState _humanPlayerCellState,Cell.CellState _aIPlayerCellState){
		HumanPlayerCellState = _humanPlayerCellState;
		AIPlayerCellState = _aIPlayerCellState;
	}

	private Cell[,] CopyBoard(Cell[,] value){
		Cell[,] newArray=new Cell[value.GetLength(0),value.GetLength(1)];
		for(int i = 0; i < value.GetLength(1); i++)
		{
			for(int j = 0; j < value.GetLength(0); j++)
			{
				newArray [j, i] = value [j, i];
			}
		}
		return newArray;
	}

	public void Set(Cell[,] cellsLink, Difficulty difficulty_level,Cell.CellState _humanPlayerCellState,Cell.CellState _aIPlayerCellState){
		board = cellsLink;
		DifficultyLevel = difficulty_level;
		HumanPlayerCellState = _humanPlayerCellState;
		AIPlayerCellState = _aIPlayerCellState;
	}

	public Cell MakeAMove(){
		Cell[,] NewBoard = CopyBoard (board);
		return MiniMax (NewBoard, Cell.CellState.O).cell;
	}

	private List<Cell> FindEmptyCells(Cell[,] cellsLink){
		List<Cell> TempCellList = new List<Cell>(9);	
		for(int i = 0; i < 3; i++)
		{
			for(int j = 0; j < 3; j++)
			{
				if (cellsLink[j, i].GetState () == Cell.CellState.NONE) {
					TempCellList.Add(cellsLink [j, i]);
				}
			}
		}
		return TempCellList;
	}

	private Move MiniMax(Cell[,] newBoard, Cell.CellState playerMark){
		RecurtionDepth++;
		List<Cell> EmptyCells = FindEmptyCells (newBoard);
		List<Move> moves = new List<Move> ();
		for (int i = 0; i < EmptyCells.Count; i++) {
			Move curMove = new Move ();
			curMove.cell = EmptyCells [i];
			EmptyCells [i].SetState (playerMark);
			if (IsWinning (newBoard, HumanPlayerCellState)) {
				curMove.score = -10;
			} else if (IsWinning (newBoard, AIPlayerCellState)) {
				curMove.score = 10;
			} else if (IsDraw(newBoard)) {
				curMove.score = 0;
			} else {
				if (RecurtionDepth < RecurtionMaxDepth) {
					if (playerMark == AIPlayerCellState) {
						curMove.score = MiniMax (newBoard, HumanPlayerCellState).score;
					} else {
						curMove.score = MiniMax (newBoard, AIPlayerCellState).score;
					}
				}
			}
			moves.Add (curMove);
			EmptyCells[i].SetState (Cell.CellState.NONE);
		}
		Move BestMove = new Move();
		List<Move> BestMoves = new List<Move> ();
		float totalScore = 0;
		if (playerMark == AIPlayerCellState) {
			float bestScore = -10000;
			for (int i = 0; i < moves.Count; i++) {
				if (moves [i].score > bestScore) {
					bestScore = moves [i].score;
					BestMove = moves [i];
				}
			}

		} else if(playerMark == HumanPlayerCellState){
			float bestScore = 10000;
			for (int i = 0; i < moves.Count; i++) {
				if (moves [i].score < bestScore) {
					bestScore = moves [i].score;
					BestMove = moves [i];
				}
			}
		}
		for (int i = 0; i < moves.Count; i++) {
			if (moves [i].score == BestMove.score) {
				BestMoves.Add (moves [i]);
			}
		}
		BestMove = BestMoves [Random.Range (0, BestMoves.Count-1)];
		RecurtionDepth--;
		return BestMove;

	}
	private bool IsWinning(Cell[,] newBoard, Cell.CellState playerMark){
		for(int i = 0; i < 3; i++)
		{
			if(newBoard[0,i].GetState() != Cell.CellState.NONE && newBoard[0,i].GetState() == playerMark && newBoard[0,i].GetState() == newBoard[1,i].GetState() && newBoard[0,i].GetState() == newBoard[2,i].GetState() ){
				return true;
			}
			if(newBoard[i,0].GetState() != Cell.CellState.NONE && newBoard[i,0].GetState() == playerMark && newBoard [i, 0].GetState () == newBoard [i, 1].GetState () && newBoard[i, 0].GetState () == newBoard[i, 2].GetState () ) {
				return true;
			}
		}
		if ((newBoard [0, 0].GetState () != Cell.CellState.NONE && newBoard [0, 0].GetState () == playerMark && newBoard [0, 0].GetState () == newBoard [1, 1].GetState () && newBoard [0, 0].GetState () == newBoard [2, 2].GetState ()) || 
			(newBoard [0, 2].GetState () != Cell.CellState.NONE && newBoard [0, 2].GetState () == playerMark && newBoard [0, 2].GetState () == newBoard [1, 1].GetState () && newBoard [0, 2].GetState () == newBoard [2, 0].GetState ())) {
			return true;
		}
		return false;
	}
	private bool IsDraw(Cell[,] NewBoard){
		for(int i = 0; i <  NewBoard.GetLength(1); i++)
		{
			for(int j = 0; j <  NewBoard.GetLength(0); j++)
			{
				if ( NewBoard [j, i].GetState () == Cell.CellState.NONE) {
					return false;
				}
			}
		}
		return true;
	}

		
}
