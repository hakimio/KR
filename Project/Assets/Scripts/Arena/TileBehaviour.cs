using UnityEngine;
using System.Collections;

public class TileBehaviour: MonoBehaviour
{
    public Tile tile;
    public Material OpaqueMaterial;
    public Material defaultMaterial;
    Color orange = new Color(255f / 255f, 127f / 255f, 0, 127f/255f);

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
        if (CharacterMovement.instance.IsMoving)
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
            tile.Passable = true;

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
            && this != GridManager.instance.originTileTB)
        {
            this.renderer.material = defaultMaterial;
            this.renderer.material.color = Color.white;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag.Equals("Obstacle"))
        {
            tile.Passable = false;
            changeColor(Color.gray);
        }
    }
}
