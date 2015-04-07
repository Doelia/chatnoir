using UnityEngine;
using System.Collections;

public class Cat : MonoBehaviour {

	Hexagone isOn;

	public void Start() {
		GameObject o = GameObject.Find ("hexagone");
		Hexagone h = o.GetComponent<Hexagone>();
		this.moveTo(h);
	}

	public void moveTo(Hexagone h) {
		Debug.Log ("Move to "+h.transform.localPosition);
		isOn = h;
		this.transform.localPosition = h.center;
	}

	public void moveToClosest() {
		Controlor.setGoodColorToAll();

		Controlor.prepareToBuildPath();
		ArrayList path = new ArrayList();

		path = this.buildPath(isOn, path);

		if (path == null) {
			Debug.Log ("Pas de solution");
		} else {
			foreach (Hexagone h in path) {
				//Debug.Log ("loop");
				Debug.Log ("Path "+h.transform.localPosition);
				if (!h.isGoal)
					h.colorInGreen();
			}
		}
	}

	public ArrayList buildPath(Hexagone h, ArrayList path) {
		path.Add (isOn);
		Debug.Log ("Add "+isOn.ID);
		
		ArrayList pathOut = new ArrayList();
		while (true) {
			Debug.Log ("loop, pathCount="+path.Count);
			if (path.Count == 0) {
				return null;
			}

			Hexagone[] obs = Controlor.orderByValue(path);
			path = new ArrayList(obs);

			Hexagone cnode = (Hexagone) path[0];
			pathOut.Add (cnode);
			Debug.Log ("add "+cnode.ID);
			path.RemoveAt(0);

			if (cnode.isGoal) {
				Debug.Log("cnode is goal");
				return pathOut;
			}

			cnode.flag = 1;

			foreach (Hexagone voisin in cnode.voisins) {
				if (voisin.flag == 0 && !voisin.blocked)
					path.Add (voisin);
			}
		}
	}

	public void Update () {
		if (Input.GetKeyDown(KeyCode.B)) {
			this.moveToClosest();
		}
	}

	public float getDistanceWith(Hexagone g) {
		return Vector2.Distance(this.transform.position, g.transform.position);
	}
}
