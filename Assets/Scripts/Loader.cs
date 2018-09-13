using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	public void Play(){
		Application.LoadLevel ("Level 1");

	}

	public void Quit()
	{
		Application.Quit();
	}
}
