using UnityEngine;
using System.Collections;

public class GameArea:MonoBehaviour  {

	private Cell[,] cells = new Cell[3 , 3];

	public Cell[,] GetCellsArray(){
		return cells;
	}
	public void SetCellsArray(Cell[,] newCells){
		cells = newCells;
	}
	public Cell GetCell (int ColumnIndex, int LineIndex){
		if (ColumnIndex <= cells.GetLength (0) && LineIndex <= cells.GetLength (1)) {
			return cells [LineIndex, ColumnIndex];
		}
		return null;
	}
	public void SetCell (int ColumnIndex, int LineIndex, Cell value){
		if (ColumnIndex <= cells.GetLength (0) && LineIndex <= cells.GetLength (1)) {
			cells [LineIndex, ColumnIndex] = value;
		}
	}

}
