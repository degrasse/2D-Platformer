using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotator : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		//rotates the coin in each level
		transform.Rotate (new Vector3 (0, 0, 45) * Time.deltaTime);
		
	}
}
