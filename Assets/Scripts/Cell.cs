using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cell: MonoBehaviour{

	[SerializeField]private Texture X_texture,O_texture,NONE_texture;
	[SerializeField]private RawImage icon;
	private TTTController controller;
	private CellState state;
	public TTTController Controller{
		get{
			return controller;
		}
		set{
			controller = value;
		}
	}

	public enum CellState
	{
		NONE,
		O,
		X
	}

	public void SetState( CellState value){
		state = value;
		switch(value){
		case  CellState.NONE:
			icon.texture = NONE_texture;
			break;
		case  CellState.X:
			icon.texture = X_texture;
			break;
		case CellState.O:
			icon.texture = O_texture;
			break;
		default:
			Debug.Log ("Erorr");
			break;
		}
	}
		

	public  CellState GetState(){
		return state;
	} 


	public void SendInfToController(){
		if (GetState () == CellState.NONE) {
			Controller.CellClick(this);
		}
	} 
}
