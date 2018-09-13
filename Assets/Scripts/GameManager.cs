using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

	[Tooltip("An Audioclip that is played when the player dies")]
	public AudioClip dieSound;

	[Tooltip("An Audioclip that is played when the player reaches the goal")]
	public AudioClip goalSound;


	private AudioSource myAudioSource;

	private void Awake() {
		GameManager[] m = GameObject.FindObjectsOfType<GameManager>();
		if (m.Length > 1) {
			Destroy(gameObject);
		}

		myAudioSource = GetComponent<AudioSource>();
	}

	void Start () {
		DontDestroyOnLoad(gameObject);

	}

	void Update () {
		//restart game after reaching the end
		if (Input.GetKeyDown (KeyCode.Space) && SceneManager.GetActiveScene ().name == "Game Over") {
			SceneManager.LoadScene ("Level 1");
		}

	}



	public void PlayDieSound() {

		myAudioSource.PlayOneShot(dieSound);
	}

	public void PlayGoalSound() {
		myAudioSource.PlayOneShot(goalSound);
	}
}


