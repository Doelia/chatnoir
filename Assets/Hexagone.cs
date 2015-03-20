using UnityEngine;
using System.Collections;

public class Hexagone : MonoBehaviour {

	public Transform prefab;
	public ArrayList voisins;
	private int distanceCenter = 0;
	public Vector2 center = new Vector2(0,0);
	public bool blocked;

	public Transform accessCat;

	void Awake() {
		voisins = new ArrayList();
		blocked = false;
	}	

	void Start () {
	}

	private float rayon = 4.3f;

	public void blockIt() {
		this.blocked = true;
		this.GetComponent<SpriteRenderer>().color = new Color(0,0,0);
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
		Debug.Log (this.voisins.Count+" voisins");
		foreach (Hexagone v in voisins) {
			Debug.Log (v.center);
		}
		this.blockIt();
		this.accessCat.GetComponent<Cat>().moveTo(this);
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.A))
			genVoisins();
	}

	public bool isABord() {
		return (this.voisins.Count < 6);
	}
}
