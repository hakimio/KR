using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class CharCreator : MonoBehaviour 
{
	private BaseChar myChar;
	
	private const int POINTS = 25;
	private const int MIN_ATTR_VALUE = 8;
	private const int START_VALUE = 8;
	private int pointsLeft;

	private const int OFFSET = 5;
	private const int LINE_HEIGHT = 26;

	private const int LABEL_WIDTH = 150;
	private const int VALUE_WIDTH = 30;

	private const int ATTR_OFFSET = 40;

	public GUISkin mySkin;
	private GUIContent[] comboBoxList;
	private ComboBox comboBoxControl;
	public GUIStyle listStyle;
	private bool[] disciplineToggles;
	private Discipline savedDiscipline1;
	private Discipline savedDiscipline2;
	private int selectedFeat1;
	private int selectedFeat2;
	public GUIStyle descriptionstyle;
	private string[] attrDescriptions;
	private string[] attr2Descriptions;
	public GameObject playerPrefab;

	// Use this for initialization
	void Start () {
		GameObject pc = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		pc.name = "Player Character";
		myChar = pc.GetComponent<BaseChar>();
		
		attrDescriptions = new string[Enum.GetValues(typeof(AttrNames)).Length];
		attrDescriptions[(int)AttrNames.Strength] = "Raw physical strength. A high"+
			" strength is important for physical characters, because it helps"+
			" them prevail in combat.";
		attrDescriptions[(int)AttrNames.Dexterity] = "Dexterity measures hand-eye"+
			" coordination, agility, reflexes, and balance. This ability is "+
			"important for characters who typically wear light or medium armor.";
		attrDescriptions[(int)AttrNames.Constitution] = "Constitution represents "+
			"your character's health and stamina.";
		attrDescriptions[(int)AttrNames.Technique] = "Technique determines resistance,"+
			" mana and the amount of damage done by magical attacks.";
		attr2Descriptions = new string[Enum.GetValues(typeof(SecondaryAttrNames)).Length];
		attr2Descriptions[(int)SecondaryAttrNames.AC] = "Armor class modifies the chance"+
			" to hit this character.";
		attr2Descriptions[(int)SecondaryAttrNames.HP] = "Hit points determine how much "+
			"damage your character can take before dying. If you reach 0 HP or less, you"+
			" are dead.";
		attr2Descriptions[(int)SecondaryAttrNames.TP] = "Technique points determine how "+
			"often character will be able to use his special techniques.";
		
		disciplineToggles = new bool[myChar.Archetype.getDisciplines().Length];
		for (int i = 0; i < disciplineToggles.Length; i++)
			disciplineToggles[i] = false;
		savedDiscipline1 = null;
		savedDiscipline2 = null;
		
		comboBoxControl = new ComboBox();
		listStyle = new GUIStyle();
		listStyle.normal.textColor = mySkin.label.normal.textColor;
		listStyle.hover.textColor = new Color(204f/255f, 206f/255f, 208f/255f);
		listStyle.padding.left = OFFSET;
		listStyle.padding.right = OFFSET;
		listStyle.padding.top = OFFSET;
		listStyle.padding.bottom = 2;
		
		Texture2D tex = new Texture2D(2,2);
		Texture2D tex2 = new Texture2D(2,2);
		Color[] colors = new Color[4];
		for (int i = 0; i < colors.Length; i++) 
			colors[i] = mySkin.label.normal.textColor;
		tex.SetPixels(colors);
		tex.Apply();
		listStyle.hover.background = tex;
		for (int i = 0; i < colors.Length; i++) 
			colors[i] = new Color(23f/255f, 36f/255f, 56f/255f);
		tex2.SetPixels(colors);
		tex2.Apply();
		listStyle.normal.background = tex2;
		
		comboBoxList = new GUIContent[2];
		comboBoxList[0] = new GUIContent("Stormer1", Archetypes.Stormer.getDescription());
		comboBoxList[1] = new GUIContent("Stormer2", Archetypes.Stormer.getDescription());
		
		pointsLeft = POINTS;
		
		for (int i = 0; i < Enum.GetValues(typeof(AttrNames)).Length; i++)
			myChar.getAttr(i).baseValue = START_VALUE;
		
		//myChar.attrUpdate();
	}
	void OnGUI()
	{
		GUI.skin = mySkin;
		showBasics();
		showBaseAttr();
		showSecondaryAttr();
		showDisciplines();
		showFeats();
		showFeats2();
		showArchetypes();
		showBackAndPlay();
		showDescriptions();
	}
	
	private void showBasics()
	{
		string weight, height;
		GUI.BeginGroup(new Rect(Screen.width/2 - 400 + OFFSET, 
		                        Screen.height/2 - 300 + OFFSET, 300, 200));
		GUI.Label(new Rect(OFFSET, 0, 100, LINE_HEIGHT), "Name:");
		myChar.charName = GUI.TextField(new Rect(105, 0, LABEL_WIDTH, LINE_HEIGHT), 
		                            myChar.charName);
		GUI.Label(new Rect(OFFSET, OFFSET + LINE_HEIGHT, 100, LINE_HEIGHT), "Weight:");
		weight = GUI.TextField(new Rect(105, OFFSET + LINE_HEIGHT, 45,
		                 LINE_HEIGHT), myChar.weight.ToString());
		myChar.weight = parseNumField(weight, 1, 200);
		GUI.Label(new Rect(OFFSET, 2* (OFFSET + LINE_HEIGHT), 100, LINE_HEIGHT), "Height:");
		height = GUI.TextField(new Rect(105, 2* (OFFSET + LINE_HEIGHT), 45,
		                 LINE_HEIGHT), myChar.height.ToString());
		myChar.height = parseNumField(height, 1, 250);
		GUI.EndGroup();
	}
		
	private void showBaseAttr()
	{
		GUIContent content;
		GUI.BeginGroup(new Rect(Screen.width/2 - 400 + OFFSET,
		                        Screen.height/2 - 300 + 4 * (OFFSET + LINE_HEIGHT), 325, 150));
		GUI.Label(new Rect(OFFSET, 0,
		                   LABEL_WIDTH, LINE_HEIGHT), "Points: " + pointsLeft);
		content = new GUIContent("Mods", "The modifier is the number you apply" +
		 	" to the die roll when your character tries to do something related" +
		 	" to that ability. You also use the modifier with some numbers that" +
		 	" aren't die rolls.");
		GUI.Label(new Rect(LABEL_WIDTH + 2*LINE_HEIGHT + 7*OFFSET + VALUE_WIDTH,
                   0, 50, LINE_HEIGHT), content);
		for (int i = 0; i < System.Enum.GetValues(typeof(AttrNames)).Length; i++)
		{
			content = new GUIContent(((AttrNames)i).ToString(), attrDescriptions[i]);
			GUI.Label(new Rect(OFFSET, (i+1)*(LINE_HEIGHT+OFFSET), LABEL_WIDTH, LINE_HEIGHT),
			          content);
			GUI.Label(new Rect(LABEL_WIDTH + LINE_HEIGHT + 3*OFFSET, (i+1)*(LINE_HEIGHT+OFFSET), 
			                   VALUE_WIDTH, LINE_HEIGHT),
			          myChar.getAttr(i).Value.ToString());

			if (GUI.Button(new Rect(LABEL_WIDTH + OFFSET, (i+1)*(LINE_HEIGHT + OFFSET),
			                        LINE_HEIGHT, LINE_HEIGHT), "-"))
			{
				if (myChar.getAttr(i).baseValue > MIN_ATTR_VALUE)
				{
					if (myChar.getAttr(i).baseValue < 15)
						pointsLeft++;
					else if (myChar.getAttr(i).baseValue < 17)
						pointsLeft += 2;
					else
						pointsLeft += 3;
					    
					myChar.getAttr(i).baseValue--;
					//myChar.attrUpdate();
				}
			}
			int takenPoints;

			if (GUI.Button(new Rect(LABEL_WIDTH + LINE_HEIGHT + 3*OFFSET + VALUE_WIDTH,
			                        (i+1)*(LINE_HEIGHT+OFFSET),
			                        LINE_HEIGHT, LINE_HEIGHT), "+"))
			{
				if (myChar.getAttr(i).baseValue > 15)
					takenPoints = 3;
				else if (myChar.getAttr(i).baseValue > 13)
					takenPoints = 2;
				else
					takenPoints = 1;
				
				if (pointsLeft >= takenPoints && myChar.getAttr(i).baseValue < 18)
				{
					pointsLeft -= takenPoints;
					
					myChar.getAttr(i).baseValue++;
					//myChar.attrUpdate();
				}
			}
			
			GUI.Label(new Rect(LABEL_WIDTH + 2*LINE_HEIGHT + 7*OFFSET + VALUE_WIDTH,
			                   (i+1)*(LINE_HEIGHT+OFFSET), VALUE_WIDTH, LINE_HEIGHT),
			          ModifiedStat.calcModValue(myChar.getAttr(i).Value).ToString());
		}
		GUI.EndGroup();
	}
	
	private void showSecondaryAttr()
	{
		GUIContent content;
		GUI.BeginGroup(new Rect(Screen.width/2 - 400 + OFFSET,
		                        Screen.height/2 - 300 + 9 * (OFFSET + LINE_HEIGHT), 300, 100));
		for (int i = 0; i < System.Enum.GetValues(typeof(SecondaryAttrNames)).Length; i++)
		{
			content = new GUIContent(((SecondaryAttrNames)i).ToString(), attr2Descriptions[i]);
			GUI.Label(new Rect(OFFSET, i*(LINE_HEIGHT+OFFSET), LABEL_WIDTH, LINE_HEIGHT),
				          content);
			GUI.Label(new Rect(LABEL_WIDTH + LINE_HEIGHT + 3*OFFSET,
			                   i*(LINE_HEIGHT+OFFSET), VALUE_WIDTH, LINE_HEIGHT),
			          myChar.getSecondaryAttr(i).Value.ToString());
		}
		GUI.EndGroup();
	}
	
	private void showDescriptions()
	{
		GUI.BeginGroup(new Rect(Screen.width/2 - 400 + OFFSET, 
			Screen.height/2 - 300 + 12* (OFFSET + LINE_HEIGHT), 350, 200));
		if (GUI.tooltip == "")
			GUI.Label(new Rect(OFFSET, 0, 345, 200), "Point your mouse cursor to some"+
			          " status to see its description.", descriptionstyle);
		else
			GUI.Label(new Rect(OFFSET, 0, 345, 200), GUI.tooltip, descriptionstyle);
		GUI.EndGroup();
	}
	
	private void showArchetypes()
	{
		GUI.BeginGroup(new Rect(Screen.width/2 - OFFSET, 
		                        Screen.height/2 - 300 + OFFSET, 300, 200));
		int selectedItemIndex = comboBoxControl.GetSelectedItemIndex();
		GUI.Label( new Rect(0, 0, 100, LINE_HEIGHT), 
		"Archetype " );
		selectedItemIndex = comboBoxControl.List(new Rect(110, 
			0, LABEL_WIDTH, LINE_HEIGHT), comboBoxList[selectedItemIndex], 
		    comboBoxList, listStyle );

		GUI.EndGroup();
	}
	
	private void showDisciplines()
	{
		GUIContent content;
		int nrOfToggledDisciplines = 0;
		foreach (bool toggle in disciplineToggles)
			if (toggle)
				nrOfToggledDisciplines++;
		GUI.BeginGroup(new Rect(Screen.width/2 - OFFSET, 
		                        Screen.height/2 - 300 + 2*OFFSET + LINE_HEIGHT, 400, 200));
		GUI.Label(new Rect(0, 0, 400, LINE_HEIGHT), 
			myChar.Archetype.getName() + " disciplines (selected: " +
		    nrOfToggledDisciplines + " max: 2):");
		for (int i = 0; i < myChar.Archetype.getDisciplines().Length; i++)
		{
			content = new GUIContent(myChar.Archetype.getDiscipline(i).getName(),
				myChar.Archetype.getDiscipline(i).getDescription());
			if (nrOfToggledDisciplines < 2 )
				disciplineToggles[i] = GUI.Toggle(new Rect(0, (i+1) * (LINE_HEIGHT + OFFSET), 
					LABEL_WIDTH, LINE_HEIGHT), disciplineToggles[i], content);
			else if (disciplineToggles[i] == true)
				disciplineToggles[i] = GUI.Toggle(new Rect(0, (i+1) * (LINE_HEIGHT + OFFSET), 
					LABEL_WIDTH, LINE_HEIGHT), disciplineToggles[i], content);
			else
				GUI.Toggle(new Rect(0, (i+1) * (LINE_HEIGHT + OFFSET), 
					LABEL_WIDTH, LINE_HEIGHT), disciplineToggles[i], content);
		}
		GUI.EndGroup();
	}
	
	private void showFeats()
	{
		int selectedDiscipline = -1;
		Feat[] feats;
		Discipline discipline1;
		List<GUIContent> featNames = new List<GUIContent>();
		
		for (int i = 0; i < disciplineToggles.Length; i++)
			if (disciplineToggles[i])
			{
				selectedDiscipline = i;
				break;
			}
		if (selectedDiscipline == -1)
			return;
		discipline1 = myChar.Archetype.getDiscipline(selectedDiscipline);
		if (savedDiscipline1 != discipline1)
		{
			savedDiscipline1 = discipline1;
			selectedFeat1 = 0;
		}
		GUI.BeginGroup(new Rect(Screen.width/2 - OFFSET, 
		                        Screen.height/2 - 100 + OFFSET, 400, 150));
		feats = myChar.Archetype.getDiscipline(selectedDiscipline).getFeats();
		
		GUI.Label(new Rect(0, 0, 400, LINE_HEIGHT), discipline1.getName() 
		          + " discipline feats (choose one):");
		for(int i = 0; i < feats.Length; i++)
			if (feats[i].getDependencies().Length == 0)
				featNames.Add(new GUIContent(feats[i].getName(), feats[i].getDescription()));
		selectedFeat1 = GUI.SelectionGrid(new Rect(0, LINE_HEIGHT + OFFSET, 400, 100),
		                                  selectedFeat1, featNames.ToArray(), 1, "toggle");
		GUI.EndGroup();
	}
	
	private void showFeats2()
	{
		int selectedDiscipline = -1;
		int nrOfSelectedDisciplines = 0;
		Feat[] feats;
		Discipline discipline2;
		List<GUIContent> featNames = new List<GUIContent>();
		
		for (int i = 0; i < disciplineToggles.Length; i++)
			if (disciplineToggles[i])
			{
				selectedDiscipline = i;
				nrOfSelectedDisciplines++;
			}
		if (nrOfSelectedDisciplines < 2)
			return;
		discipline2 = myChar.Archetype.getDiscipline(selectedDiscipline);
		if (savedDiscipline2 != discipline2)
		{
			savedDiscipline2 = discipline2;
			selectedFeat2 = 0;
		}
		GUI.BeginGroup(new Rect(Screen.width/2 - OFFSET, 
		                        Screen.height/2 + OFFSET + 50, 400, 200));
		feats = myChar.Archetype.getDiscipline(selectedDiscipline).getFeats();
		
		GUI.Label(new Rect(0, 0, 400, LINE_HEIGHT), discipline2.getName() 
		          + " discipline feats (choose one):");
		for(int i = 0; i < feats.Length; i++)
			if (feats[i].getDependencies().Length == 0)
				featNames.Add(new GUIContent(feats[i].getName(), feats[i].getDescription()));
		selectedFeat2 = GUI.SelectionGrid(new Rect(0, LINE_HEIGHT + OFFSET, 400, 100),
		                                  selectedFeat2, featNames.ToArray(), 1, "toggle");
		GUI.EndGroup();
	}
		
	private void showBackAndPlay()
	{	
		int nrOfSelectedDisciplines = 0;
		
		for (int i = 0; i < disciplineToggles.Length; i++)
			if (disciplineToggles[i])
				nrOfSelectedDisciplines++;
			
		if (GUI.Button(new Rect(Screen.width/2 - 400 + 2* OFFSET, 
			Screen.height/2 + 300 - OFFSET - LINE_HEIGHT, LABEL_WIDTH, LINE_HEIGHT), "Back"))
			Application.LoadLevel("MainMenu");
		if (pointsLeft > 0 || myChar.charName == "" || nrOfSelectedDisciplines < 2)
			GUI.enabled = false;
		
		if (GUI.Button(new Rect(Screen.width/2 + 400 - OFFSET - LABEL_WIDTH, 
		    Screen.height/2 + 300 - OFFSET - LINE_HEIGHT, LABEL_WIDTH, LINE_HEIGHT), "Play"))
		{
			Discipline[] disciplines = myChar.Archetype.getDisciplines();
			Discipline[] selectedDisciplines = new Discipline[2];
			int j = 0;
			
			for (int i = 0; i < disciplines.Length; i++)
				if (disciplineToggles[i])
				{
					disciplines[i].known = true;
					selectedDisciplines[j] = disciplines[i];
					j++;
				}
			j = 0;			
			for (int i = 0; i < selectedDisciplines[0].getFeats().Length; i++)
				if (selectedDisciplines[0].getFeat(i).getDependencies().Length == 0)
				{
					if (j == selectedFeat1)
					{
						selectedDisciplines[0].getFeat(i).known = true;
						break;
					}
					j++;
				}
			j = 0;
			for (int i = 0; i < selectedDisciplines[1].getFeats().Length; i++)
				if (selectedDisciplines[1].getFeat(i).getDependencies().Length == 0)
				{
					if (j == selectedFeat2)
					{
						selectedDisciplines[1].getFeat(i).known = true;
						break;
					}
					j++;
				}
			
			GameObject gs = GameObject.Find("Game Settings");
			GameSettings gsClass = gs.GetComponent<GameSettings>();
			gsClass.saveChar();
			Application.LoadLevel("Level1");
		}
		
		GUI.enabled = true;
	}
	
	private int parseNumField(string field, int min, int max)
	{
		int temp, num;
		temp = num = 0;
		if (int.TryParse(field, out temp))
		{
			num = Mathf.Clamp(temp, min, max);
		}
		else if (field == "")
			num = min;
		
		return num;
	}
	
}
