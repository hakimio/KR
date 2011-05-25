using UnityEngine;
using System.Collections;
using AstarClasses;

public class ProceduralLevel : MonoBehaviour {

	//This script will generate a procedural grid and then pass it to the A* system, make sure this is not called in the Awake function since the A* system might fail then

	public int angleIncrement = 4;
	public float distanceIncrement = 0.03F;
	public int numberOfNodes = 3000;
	public int numberOfLevels = 2;
	public float levelDistanceIncrement = 5;
	public int connectionOffset = 0;
	
	public GeneratorMode mode = GeneratorMode.Spiral;
	public enum GeneratorMode {
		Circle,
		Spiral
	}
	
	public void Start () {
		//Create an array of nodes to use, we will use a one-dimensional array for simplicity
		SimpleNode[][] nodes = new SimpleNode[numberOfLevels][];
		
		for (int i=0;i<nodes.Length;i++) {
			nodes[i] = new SimpleNode[numberOfNodes];
		}
		
		//First initialize the nodes at different positions
		float circleIncrement = 360/angleIncrement * distanceIncrement;
		
		//You almost always first initialize the nodes and position them, and then you iterate the list and make the connections, otherwise you might end up having to make a connection to a node which isn't initialized yet.
		for (int y=0;y<nodes.Length;y++) {//The grid index
			//This keeps track of at which ring we are now in the Circle mode
			int lastRing = 0;
			
			for (int x=0;x<nodes[y].Length;x++) {//The node index
			
				SimpleNode node = nodes[y][x] = new SimpleNode (Vector3.zero);
				if (mode == GeneratorMode.Spiral) {
					node.vectorPos.x = Mathf.Sin (x * Mathf.Deg2Rad * angleIncrement) * x * distanceIncrement;
					node.vectorPos.z = Mathf.Cos (x * Mathf.Deg2Rad * angleIncrement) * x * distanceIncrement;
				} else {
					node.vectorPos.x = Mathf.Sin (x * Mathf.Deg2Rad * angleIncrement) * lastRing * circleIncrement;
					node.vectorPos.z = Mathf.Cos (x * Mathf.Deg2Rad * angleIncrement) * lastRing * circleIncrement;
				}
				
				node.vectorPos.x += (numberOfNodes/(360.0F/angleIncrement))*circleIncrement*y*2+y;
				
				if (x * angleIncrement - 360*lastRing >= 360) {
					lastRing++;
				}
				
			}
		}
		
		/* Then create the connections between the nodes
		Other optional settings to add to the node are costs to each neighbour and angles to each neighbours (make sure the angles are divided by 90 before adding them to the node)
		See the SimpleNode class in the AstarClasses script for more info about other settings
		*/
		for (int y=0;y<nodes.Length;y++) {//Grid index
			for (int x=0;x<nodes[y].Length;x++) {//Node index
				SimpleNode node = nodes[y][x];
				
				ArrayList neighbours = new ArrayList ();
				
				if (x != 0) {
					//Add the previous node in the circle/spiral
					neighbours.Add (nodes[y][x-1]);
				} else {
					//Make connections from the circle/spiral center to the next/previous circle
					if (y != 0) {
						neighbours.Add (nodes[y-1][0]);
					}
					if (y < nodes.Length-1) {
						neighbours.Add (nodes[y+1][0]);
					}
				}
				
				//Add the next node
				if (x != nodes[y].Length-1) {
					neighbours.Add (nodes[y][x+1]);
				}
				
				//One ring of nodes consist of [360/angleIncrement] nodes, therefore, when adding the node in the inner/outer ring we can just subract/add this value to the index and get the correct node.
				int offset = Mathf.RoundToInt (360.0F/angleIncrement + connectionOffset);
				
				//Add the node further in towards the center of the spiral
				if (x - offset >= 0) {
					neighbours.Add (nodes[y][x-offset]);
				}
				
				//Add the node further away from the center of the spiral
				if (x + offset < nodes[y].Length) {
					neighbours.Add (nodes[y][x+offset]);
				}
				
				//Apply the settings to the node
				node.neighbours = neighbours.ToArray (typeof(SimpleNode)) as SimpleNode[];
			}
		}
		
		//Tell the A* script to create the navmesh
		AstarPath.active.CreateGrid (nodes);
		
	}
}
