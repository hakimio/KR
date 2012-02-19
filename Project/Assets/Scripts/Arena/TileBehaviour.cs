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
            this.renderer.material = OpaqueMaterial;
            this.renderer.material.color = orange;
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
            Color gray = Color.gray;
            gray.a = 158f / 255f;
            if (!tile.Passable)
                this.renderer.material.color = gray;
            else
                this.renderer.material.color = orange;

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
        Color red = Color.red;
        red.a = 158f / 255f;
        renderer.material.color = red;
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
        Color blue = Color.blue;
        blue.a = 158f / 255f;
        renderer.material.color = blue;
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
}
