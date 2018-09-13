using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtons : MonoBehaviour {

	public GameObject MenuPanel;
	public GameObject SelectPanel;

	// Use this for initialization
	void Start () {
		MenuPanel.SetActive (true);
		SelectPanel.SetActive(false);
	}

	public void ShowPanel(){
		MenuPanel.SetActive (false);
		SelectPanel.SetActive (true);

	}

	public void ShowMenuPanel(){
		MenuPanel.SetActive (true);
		SelectPanel.SetActive (false);
}
}