using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class ShiftScript : MonoBehaviour {

	public bool flipped = false;
	public int speed = 10;
	public bool gravflipped = false;
	public Vector2 originalGravity;
	public bool standingOnBlack = false;
	public bool standingOnWhite = false;
	public bool standingOnWall = false;

	public bool collected = false;

	void Update() {

		//player can restart the level when they're stuck
		if (Input.GetKey ("r")) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name); //reload the current scene
			if (gravflipped) {
				Vector3 temp = transform.localScale;
				temp.y *= -1; //flip the image so that the body of the player is always facing the right direction according to gravity
				transform.localScale = temp;
				transform.rotation = Quaternion.Euler(0, 0, 0);
				transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0);
				Physics2D.gravity *= -1; //reverse the direction of gravity
			}
			flipped = false;
			gravflipped = false;
            Time.timeScale = 1;
		}

		//escape from level to main menu
		if (Input.GetKeyDown(KeyCode.Escape)) {
			SceneManager.LoadScene("MAIN_MENU");
			if (gravflipped) {
				Vector3 temp = transform.localScale;
				temp.y *= -1; //flip the image so that the body of the player is always facing the right direction according to gravity
				transform.localScale = temp;
				transform.rotation = Quaternion.Euler(0, 0, 0);
				transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0);
				Physics2D.gravity *= -1; //reverse the direction of gravity
			}
			flipped = false;
			gravflipped = false;
			Time.timeScale = 1;
		}


		transform.rotation = Quaternion.Euler(0, 0, 0); //black player is always standing up straight
		transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0); //so is the white player

		//make sure the white player is always at the same position as the black player
		transform.GetChild (0).GetComponent<SpriteRenderer> ().transform.position = transform.position;

		//flip direction of gravity
		if (Input.GetKeyDown (KeyCode.Space) ) {
			Vector3 temp = transform.localScale;
			temp.y *= -1; //flip the image so that the body of the player is always facing the right direction according to gravity
			transform.localScale = temp;
			transform.rotation = Quaternion.Euler(0, 0, 0);
			transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0);
			Physics2D.gravity *= -1; //reverse the direction of gravity
			gravflipped = !gravflipped;
		}

		//flip between black and white when standing on a black or white surface
		if ((Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift)) && (standingOnBlack || standingOnWhite) && !standingOnWall) {
			Vector3 temp = transform.localScale;
			temp.y *= -1;
			transform.localScale = temp;
			Physics2D.gravity *= -1; //flips the direction of gravity

			//change the color of the player when switching between black/white by moving the white player to a higher/lower sorting order
			if (transform.GetChild (0).GetComponent<SpriteRenderer> ().sortingOrder == 0) {
				transform.GetChild (0).GetComponent<SpriteRenderer> ().sortingOrder = 20;
				transform.GetComponent<SpriteRenderer> ().sortingOrder = 0;
			} else {
				transform.GetChild (0).GetComponent<SpriteRenderer> ().sortingOrder = 0;
				transform.GetComponent<SpriteRenderer> ().sortingOrder = 20;
			}

			//based on the current gravity and current black/white flip, determine where the object should be moved to
			if (!gravflipped) {
				if (!flipped) {
					if (transform.position.y > 0) {
						transform.position = (new Vector3 (transform.position.x, transform.position.y, 0));
					} else {
						transform.position = (new Vector3 (transform.position.x, -GetComponent<SpriteRenderer> ().bounds.size.y, 0));
					}
				} else {
					transform.position = (new Vector3 (transform.position.x, transform.position.y, 0));
				}
				gravflipped = !gravflipped;
			} else {
				if (!flipped) {
					transform.position = (new Vector3 (transform.position.x, transform.position.y, 0));
				} else {
					transform.position = (new Vector3 (transform.position.x, transform.position.y, 0));
				}
				gravflipped = !gravflipped;
			}
			flipped = !flipped;

			//"black" blocks are enabled colliders when the player is currently black, so that the player can stand on them
			GameObject[] blackbg = GameObject.FindGameObjectsWithTag ("black");
			foreach (var item in blackbg)
			{
				if (item.GetComponent<BoxCollider2D> () != null) {
					item.GetComponent<BoxCollider2D> ().enabled = !item.GetComponent<BoxCollider2D> ().enabled;
				}
				if (item.GetComponent<PolygonCollider2D> () != null) {
					item.GetComponent<PolygonCollider2D> ().enabled = !item.GetComponent<PolygonCollider2D> ().enabled;
				}
				if (item.GetComponent<EdgeCollider2D> () != null) {
					item.GetComponent<EdgeCollider2D> ().enabled = !item.GetComponent<EdgeCollider2D> ().enabled;
				}
			}
			//and vice versa for "white" blocks. these are not colliders when the player is black, but are colliders when the player is white
			GameObject[] whitebg = GameObject.FindGameObjectsWithTag ("white");
			foreach (var item in whitebg)
			{
				if (item.GetComponent<BoxCollider2D> () != null) {
					item.GetComponent<BoxCollider2D> ().enabled = !item.GetComponent<BoxCollider2D> ().enabled;
				}
				if (item.GetComponent<PolygonCollider2D> () != null) {
					item.GetComponent<PolygonCollider2D> ().enabled = !item.GetComponent<PolygonCollider2D> ().enabled;
				}
				if (item.GetComponent<EdgeCollider2D> () != null) {
					item.GetComponent<EdgeCollider2D> ().enabled = !item.GetComponent<EdgeCollider2D> ().enabled;
				}
			}

		}

		//just check that the object is in the correct position/rotation after whatever changes are made above
		transform.GetChild (0).transform.position = transform.position;
		transform.rotation = Quaternion.Euler(0, 0, 0);
		transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0);
	}
		
	void Start()
	{
		originalGravity = Physics2D.gravity; //keep track of what gravity is at the beginning of the level for when the level is restarted

		//"white" block colliders are not enabled at the beginning of the level, but "black" blocks are
		GameObject[] whitebg= GameObject.FindGameObjectsWithTag("white");
		foreach (var item in whitebg)
		{
			if (item.GetComponent<BoxCollider2D> () != null) {
				item.GetComponent<BoxCollider2D> ().enabled = !item.GetComponent<BoxCollider2D> ().enabled;
			}
			if (item.GetComponent<PolygonCollider2D> () != null) {
				item.GetComponent<PolygonCollider2D> ().enabled = !item.GetComponent<PolygonCollider2D> ().enabled;
			}

			if (item.GetComponent<EdgeCollider2D> () != null) {
				item.GetComponent<EdgeCollider2D> ().enabled = !item.GetComponent<EdgeCollider2D> ().enabled;
			}
		}

		//move the child white player to the same position as the black player
		transform.GetChild (0).transform.position = transform.position;
	}

	void OnCollisionEnter2D(Collision2D other){

		//when the player hits a spike they die and the level starts over
		if (other.collider.tag == "spike") {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			Physics2D.gravity = originalGravity;
			flipped = false;
			gravflipped = false;
		}

		//so that we can check to make sure that the player is standing on a black or white surface before they flip
		//this way they don't try to flip into a wall, and don't flip in mid air
		if (other.collider.tag == "black") {
			standingOnBlack = true;
		} else if (other.collider.tag == "white") {
			standingOnWhite = true;
		} else if (other.collider.tag == "wall") {
			standingOnWall = true;
		}
	}

	void OnCollisionExit2D(Collision2D other) {
		//the player is no longer standing on black/white/wall when they are no longer colliding with them
		if (other.collider.tag == "black") {
			standingOnBlack = false;
		} else if (other.collider.tag == "white") {
			standingOnWhite = false;
		} else if (other.collider.tag == "wall") {
			standingOnWall = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		//determine if the player has collected the coin to unlock the door
		if (other.gameObject.CompareTag ("coin")) {
			other.gameObject.SetActive (false);
			collected = true;
		}
	}

	void OnTriggerStay2D(Collider2D other) {

		//load the appropriate next level when the player goes through the door and the door has been unlocked with the coin
		if (Input.GetKeyDown (KeyCode.UpArrow) && other.tag == "door" && collected) {
			string scenetoload = "Level 1";
			string currentlevel = SceneManager.GetActiveScene ().name;

			if (currentlevel == "Level 1") {
				scenetoload = "Level 2";
			} else if (currentlevel == "Level 2") {
				scenetoload = "Level 3";
			} else if (currentlevel == "Level 3") {
				scenetoload = "Level 4";
			} else if (currentlevel == "Level 4") {
				scenetoload = "Level 5";
			} else if (currentlevel == "Level 5")  {
                scenetoload = "Level 6";
            } else if (currentlevel == "Level 6") {
                scenetoload = "Level 7";
            } else if (currentlevel == "Level 7") {
                scenetoload = "Level 8";
            } else if (currentlevel == "Level 8") {
                scenetoload = "Level 9";
            } else if (currentlevel == "Level 9") {
				scenetoload = "Level 10";
			} else if (currentlevel == "Level 10") {
				scenetoload = "Game Over";
			}

			//load next scene and reset the gravity/flipped/gravflipped variables for the next level
			SceneManager.LoadScene(scenetoload);
			if (gravflipped) {
				Vector3 temp = transform.localScale;
				temp.y *= -1; //flip the image so that the body of the player is always facing the right direction according to gravity
				transform.localScale = temp;
				transform.rotation = Quaternion.Euler(0, 0, 0);
				transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0);
				Physics2D.gravity *= -1; //reverse the direction of gravity
			}
			flipped = false;
			gravflipped = false;
		}

		//if touching a conveyer belt toward the right then move the position right
		if (other.tag == "conveyer right") {
            transform.position += (new Vector3(.85f, -.01f, 0) * Time.deltaTime * 4);
		}

		//if touching a conveyer belt toward the left then move the position left
		if (other.tag == "conveyer left") {
            transform.position -= (new Vector3(.85f, -.01f, 0) * Time.deltaTime * 4);
		}
	}
}
