using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
    
	void Update () {
        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(transform.rotation.x, transform.rotation.y, 0);
	}
}
