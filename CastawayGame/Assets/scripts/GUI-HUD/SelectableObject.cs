using UnityEngine;
using System.Collections;

public class SelectableObject : MonoBehaviour {

	private bool selected = false;
	private TextMesh tm;

	private Color startcolor;
	void OnMouseEnter()
	{	
		selected = true;
		tm = GetComponent<TextMesh>();
		startcolor = tm.color;
		tm.color = Color.white;
	}
	void OnMouseExit()
	{
		tm.color = startcolor;
				selected = false;
	}

}
