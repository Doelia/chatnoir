using UnityEngine;
using System.Collections;

public class Hexagone : MonoBehaviour {

	public Transform accessCat;
	public Transform prefab;

	public ArrayList voisins;
	private int distanceCenter = 0;
	public Vector2 center = new Vector2(0,0);
	public bool blocked;
	public bool isGoal = false;
	public bool isGreen = false;

	// NODE DATA
	public float val;
	public int flag;

	public static int AI = 0;
	public int ID;

	void Awake() {
		voisins = new ArrayList();
		blocked = false;
		isGoal = false;
		ID = AI++;
	}	

	public float getVal() {
		return val;
	}

	void Start () {
	}

	private float rayon = 4.3f;

	public void blockIt() {
		this.blocked = true;
		this.GetComponent<SpriteRenderer>().color = new Color(0,0,0);
	}

	public void unBlockIt() {
		this.blocked = false;
		this.GetComponent<SpriteRenderer>().color = new Color(1,1,1);
	}

	void setAsGoal(bool b) {
		if (b) {
			this.GetComponent<SpriteRenderer>().color = new Color(0,0,1);
		} else {
			this.GetComponent<SpriteRenderer>().color = new Color(1,1,1);
		}
		Debug.Log (this.GetComponent<SpriteRenderer>().color);
		isGoal = b;
	}

	public void colorInGreen() {
		isGreen = true;
		this.GetComponent<SpriteRenderer>().color = new Color(0,1,0);
	}

	public void uncolor() {
		this.GetComponent<SpriteRenderer>().color = new Color(1,1,1);
	}

	public void setGoodColor() {
		isGreen = false;
		if (blocked) {
			blockIt();
		} else if (isGoal) {
			this.setAsGoal(isGoal);
		} else {
			uncolor();
		}
	}

	Hexagone haveVoisinAtSamePosition(Vector2 position) {
		foreach (Hexagone v in voisins) {
			if (Mathf.Abs(v.center.x - position.x) < 1
			    && Mathf.Abs(v.center.y - position.y) < 1 
			    ) {
				return v;
			}
		}
		return null;
	}

	void addVoisin(Hexagone x) {
		if (!this.voisins.Contains(x)) {
			this.voisins.Add(x);
		}
	}

	void genVoisins() {

		Hexagone prec = null;
		Hexagone first = null;

		for (int step = 0; step < 6; step++) {

			float alpha = 2f*Mathf.PI/6f * step;
			float x = Mathf.Cos (alpha) * rayon;
			float y = Mathf.Sin (alpha) * rayon;
			Vector2 pos = new Vector2(x,y);
			pos.Set (x+center.x, y+center.y);

			Hexagone nouv;
			Hexagone tmp = this.haveVoisinAtSamePosition(pos);
			if (tmp == null) {
				Transform f = Instantiate(prefab);
				f.position = pos;
				nouv = f.GetComponent<Hexagone>();
				nouv.Awake();
			} else {
				nouv = tmp;
			}
				
			nouv.distanceCenter = distanceCenter + 1;
			nouv.center = pos;
			
			this.addVoisin (nouv);
			nouv.addVoisin (this);

			if (step == 0) {
				first = nouv;
			}
			
			if (prec != null) {
				nouv.addVoisin (prec);
				prec.addVoisin (nouv);
			}
			if (step == 5) {
				nouv.addVoisin (first);
				first.addVoisin (nouv);
			}
			
			
			if (nouv.distanceCenter < 1) {
				nouv.genVoisins();
			}
			
			prec = nouv;

		}
	}


	void OnMouseDown() {
		if (Input.GetKey(KeyCode.T)) { // Debug
			Debug.Log (this.getDistanceWithGoal());
			Debug.Log ("center = "+this.center);
			Debug.Log ("ID = "+this.ID);
		}
		else if (Input.GetKey(KeyCode.Z)) { // Bouger le chas
			Cat c = GameObject.Find ("cat").GetComponent<Cat>();
			c.moveTo(this);
		} else if (Input.GetKey(KeyCode.E)) { // Définit un goal
			Debug.Log ("Set goal");
			if (Controlor.getGoal() != null) {
				Controlor.getGoal().setAsGoal(false);
				Debug.Log ("No old goal found");
			}
			this.setAsGoal(true);
		} else { // Ajoute un obstacle
			if (this.blocked) {
				this.unBlockIt();
			} else {
				this.blockIt();
			}
		}

	}

	bool equals(Hexagone h) {
		return ID == h.ID;
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.A))
			genVoisins();
	}

	public bool isABord() {
		return (this.voisins.Count < 6);
	}

	public float getDistanceWithGoal() {
		return Vector2.Distance (this.center, Controlor.getGoal().center);
	}


}
