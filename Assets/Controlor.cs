using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Controlor : MonoBehaviour {

	void Start () {
	
	}
	
	public static Hexagone[] getAllHexagone() {
		return (Hexagone[]) FindObjectsOfType(typeof(Hexagone));
	}

	
	public static Hexagone[] orderByValue(Hexagone[] list) {
		IEnumerable query = list.OrderBy(a => a.val);
		return query.Cast<Hexagone>().ToArray();
	}

	public static Hexagone[] orderByValue(ArrayList list) {
		return orderByValue(list.Cast<Hexagone>().ToArray());
	}


	public static void prepareToBuildPath() {
		Hexagone[] obs = getAllHexagone();
		foreach (Hexagone h in obs) {
			h.val = h.getDistanceWithGoal();
			h.flag = 0;
		}
	}

	public static void setGoodColorToAll() {
		Hexagone[] obs = getAllHexagone();
		foreach (Hexagone h in obs) {
			h.setGoodColor();
		}
	}

	public void Update() {

	}

	public static int cpt = 0;
	public static Hexagone getGoal() {
		Debug.Log ("GetGoal"+cpt++);
		Hexagone[] obs = getAllHexagone();
		foreach (Hexagone h in obs) {
			if (h.isGoal) {
				return h;
			}
		}
		return null;
	}

}
