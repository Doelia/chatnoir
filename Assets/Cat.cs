using UnityEngine;
using System.Collections;

public class Cat : MonoBehaviour {

	Hexagone isOn;

	public void moveTo(Hexagone h) {
		this.transform.localPosition = h.center;
	}

	public void moveToClosest() {
		ArrayList path = new ArrayList();
		path.Add (isOn);
		this.buildPath(isOn, path);
	}

	public ArrayList buildPath(Hexagone h, ArrayList path) {
		// TODO
		return path;
	}
}
