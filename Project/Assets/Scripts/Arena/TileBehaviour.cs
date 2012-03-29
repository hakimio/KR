using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileBehaviour: MonoBehaviour
{
    public Tile tile;
    public Material OpaqueMaterial;
    public Material defaultMaterial;
    Color orange = new Color(255f / 255f, 127f / 255f, 0, 127f/255f);
    bool movementEnabled = true;

    void Awake()
    {
        Messenger.AddListener("walkableTilesChanged", walkableTilesChanged);
        Messenger.AddListener("monsterTilesChanged", walkableTilesChanged);
        Messenger<bool>.AddListener("enable movement", enableMovement);
    }

    void walkableTilesChanged()
    {
        if (!tile.Passable || this == GridManager.instance.originTileTB
			|| this == GridManager.instance.destTileTB)
            return;

        List<Tile> charTiles = CombatManager.instance.walkableTiles;
        List<Tile> monsterTiles = AI.instance.monsterTiles;
        if (monsterTiles.Contains(tile) && charTiles.Contains(tile))
            changeColor(new Color(71f / 255f, 60f / 255f, 139f / 255f));
        else if (monsterTiles.Contains(tile))
            changeColor(new Color(0, 56f / 255f, 168f / 255f));
        else if (charTiles.Contains(tile))
            changeColor(new Color(0, 100f / 255f, 0, 120f / 255f));
        else
        {
            renderer.material = defaultMaterial;
            renderer.material.color = Color.white;
        }
    }

    void enableMovement(bool enable)
    {
        movementEnabled = enable;
    }

    void OnMouseEnter()
    {
        GridManager.instance.selectedTile = tile;
        if (tile.Passable && this != GridManager.instance.destTileTB
            && this != GridManager.instance.originTileTB)
        {
            changeColor(orange);
        }
    }

    void OnMouseOver()
    {
        if (!movementEnabled)
            return;
        BaseChar selectedChar = GameMaster.instance.selectedChar;
        CharacterMovement cm = selectedChar.gameObject.
            GetComponent<CharacterMovement>();
        if (cm.IsMoving)
            return;

        if (Input.GetMouseButtonUp(1))
        {
            if (this == GridManager.instance.destTileTB ||
                this == GridManager.instance.originTileTB)
                return;
            tile.Passable = !tile.Passable;
            if (!tile.Passable)
                changeColor(Color.gray);
            else
                changeColor(orange);

            GridManager.instance.generateAndShowPath();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (!tile.Passable)
                return;
            CombatManager CM = CombatManager.instance;
            if (!CM.playersTurn)
                return;

            if (!CM.walkableTiles.Contains(tile))
            {
                HUD.instance.addMessage("Not enough steps left");
                return;
            }

            TileBehaviour originTileTB = GridManager.instance.originTileTB;
            if (this == originTileTB || originTileTB == null)
                originTileChanged();
            else
                destTileChanged();

            GridManager.instance.generateAndShowPath();
        }
    }

    void changeColor(Color color)
    {
        if (color.a == 1)
            color.a = 130f / 255f;
        renderer.material = OpaqueMaterial;
        renderer.material.color = color;
    }

    void originTileChanged()
    {
        var originTileTB = GridManager.instance.originTileTB;
        if (this == originTileTB)
        {
            GridManager.instance.originTileTB = null;
            renderer.material = defaultMaterial;
            return;
        }
        GridManager.instance.originTileTB = this;
        changeColor(Color.red);
    }

    void destTileChanged()
    {
        var destTile = GridManager.instance.destTileTB;
        if (this == destTile)
        {
            GridManager.instance.destTileTB = null;
            renderer.material.color = orange;
            return;
        }
        if (destTile != null)
            destTile.renderer.material = defaultMaterial;
        GridManager.instance.destTileTB = this;
        changeColor(Color.blue);
    }

    void OnMouseExit()
    {
        GridManager.instance.selectedTile = null;
        if (tile.Passable && this != GridManager.instance.destTileTB
            && this != GridManager.instance.originTileTB
            && !CombatManager.instance.walkableTiles.Contains(tile))
        {
            this.renderer.material = defaultMaterial;
            this.renderer.material.color = Color.white;
        }
        else if (CombatManager.instance.walkableTiles.Contains(tile)
            && tile.Passable && this != GridManager.instance.destTileTB
            && this != GridManager.instance.originTileTB)
        {
            changeColor(new Color(0, 100f / 255f, 0, 120f / 255f));
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag.Equals("Obstacle"))
            setAsImpassable();
    }

    public void setAsOrigin()
    {
        Color blue = Color.blue;
        blue.a = 158f / 255f;
        changeColor(blue);
        tile.Passable = false;
        GridManager.instance.originTileTB = this;
    }

    public void setAsNormal()
    {
        renderer.material = defaultMaterial;
        renderer.material.color = Color.white;
        tile.Passable = true;
    }

    public void setAsImpassable()
    {
        changeColor(Color.gray);
        tile.Passable = false;
    }
}
