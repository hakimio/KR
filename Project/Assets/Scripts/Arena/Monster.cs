using UnityEngine;
using System.Collections;

public class Monster: MonoBehaviour
{
    public string monsterName;
    public int totalHP;
    public int HP;
    public int nrOfSteps = 3;
    public int minDamage = 10;

    void Start()
    {
        HP = totalHP;
    }

    void OnMouseEnter()
    {
        if (!CombatManager.instance.playersTurn)
            return;
        OSD.instance.showTooltip(this);
        AI.instance.findMonstersMovementArea(this);
    }

    void OnMouseExit()
    {
        if (!CombatManager.instance.playersTurn)
            return;
        OSD.instance.hideTooltip(gameObject);
        AI.instance.clearMonstersMovementArea();
    }

    void OnMouseUp()
    {
        if (!CombatManager.instance.playersTurn)
            return;

        BaseChar selectedChar = GameMaster.instance.selectedChar;
        if (CombatManager.instance.didCharacterAttack(selectedChar))
        {
            HUD.instance.addMessage("Character already attacked this turn.");
            return;
        }

        GameObject selectedCharGO = selectedChar.gameObject;
        Shooter shooter = selectedCharGO.GetComponent<Shooter>();
        MeleeAttack meleeAttack = selectedCharGO.GetComponent<MeleeAttack>();

        bool success;

        if (shooter != null)
        {
            int luck = Random.Range(0, 10);
            if (luck > 1)
                success = shooter.shootAt(gameObject, false);
            else
                success = shooter.shootAt(gameObject, true);
            if (!success)
                HUD.instance.addMessage("Target blocked.");
        }
        else
        {
            success = meleeAttack.attack(gameObject);
            if (!success)
                HUD.instance.addMessage("Target too far.");
        }
    }
}
