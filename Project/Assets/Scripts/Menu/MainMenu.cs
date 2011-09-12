using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	public bool isExitButton = false;
	
	void OnMouseOver()
	{
		renderer.material.color = new Color(102f/255f, 0f, 0f/255f);
	}
	
	void OnMouseExit()
	{
		renderer.material.color = Color.black;
	}
	
	void OnMouseUp()
	{
		if (isExitButton)
			Application.Quit();
		else
			Application.LoadLevel("charSelector");
	}
}
