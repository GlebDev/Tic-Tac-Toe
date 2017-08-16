using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

	[SerializeField] private string sceneName;
	[SerializeField] private  Dropdown DifficultyDropdown;
	private int difficulty = 0;

	public void ClearProgress(){
		ScoreDataManager.SaveScoreData (new ScoreData(0,0,0,Player.PlayerHumanity.Human));
	}

	public void ExitGame(){
		Application.Quit();
	}

	public void LoadLevel(){
		switch(DifficultyDropdown.value){
		case 0:
			TTTController.difficulty = AI.Difficulty.Easy;
			break;
		case 1:
			TTTController.difficulty = AI.Difficulty.Medium;
			break;
		case 2:
			TTTController.difficulty = AI.Difficulty.Invincible;
			break;
		default:
			TTTController.difficulty = AI.Difficulty.Easy;
			break;
		}
		SceneManager.LoadScene(sceneName);
	}

}
