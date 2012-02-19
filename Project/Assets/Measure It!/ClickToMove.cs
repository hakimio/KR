using UnityEngine;
using System.Collections;

// This is a small demo featuring the probabilities of Measure It!

public class ClickToMove : MonoBehaviour {

    void Update()
    {
        if (Input.GetMouseButtonUp(0)) {
            MeasureIt.Count("Clicks");
            MeasureIt.End("Time between Clicks (s)");
            MeasureIt.Begin("Time between Clicks (s)");
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            transform.position = r.GetPoint(-Camera.main.transform.position.z);
        }

        transform.Rotate(new Vector3(20, 20, 20) * Time.deltaTime);
	}

    void OnGUI() 
    {
        if (GUILayout.Button("Clear Time"))
            MeasureIt.Clear("Time between Clicks");
        if (GUILayout.Button("Clear Counter"))
            MeasureIt.Clear("Clicks");
        if (GUILayout.Button("Clear all"))
            MeasureIt.Clear();
        GUILayout.Label("Resolution (nanoseconds): " + MeasureIt.ResolutionNs);
        if (MeasureIt.IsHighResolutionTimer)
            GUILayout.Label("HighRes-Timer!");

        MeasureIt.Set("Cursor", Event.current.mousePosition);
    }
    
}
