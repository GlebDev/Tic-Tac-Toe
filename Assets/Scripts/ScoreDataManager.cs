using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class ScoreDataManager {

	public static string FilePath = "Saves/ScoreData.txt";

	public static void SaveScoreData(ScoreData data){
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(FilePath);
		bf.Serialize(file,data);
		file.Close();
	}
	public static ScoreData LoadScoreData(){
		if (File.Exists (FilePath)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (FilePath, FileMode.Open);
			ScoreData sd = (ScoreData)bf.Deserialize (file);
			file.Close ();
			return sd;

		} else {
			return new ScoreData ();
		}
	
	}
}
