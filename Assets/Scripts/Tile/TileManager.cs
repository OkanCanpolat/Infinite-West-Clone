using System;
using UnityEngine;

[Serializable]
public struct TileContainer
{
    public Tile[] Rows;
}
public class TileManager : MonoBehaviour
{
    public TileContainer[] Columns;
    private int width;
    private int height;

    private void Awake()
    {
        width = Columns.Length;
        height = Columns[0].Rows.Length;
        SetTileXY();
    }
    public Vector3 GetTilePosition(int column, int row)
    {
        if (IsTileOutOfRange(column, row))
        {
            return Vector3.negativeInfinity;
        }

        return Columns[column].Rows[row].transform.position;
    }
    public Vector3 GetTilePosition(Vector2Int target)
    {
        if (IsTileOutOfRange(target.x, target.y))
        {
            return Vector3.negativeInfinity;
        }

        return Columns[target.x].Rows[target.y].transform.position;
    }
    public Tile GetTile(Vector2Int target)
    {
        if (IsTileOutOfRange(target.x, target.y))
        {
            return null;
        }
        return Columns[target.x].Rows[target.y];
    }
    public Vector2Int GetTile(Tile target)
    {

        for (int i = 0; i < Columns.Length; i++)
        {
            TileContainer container = Columns[i];

            for (int j = 0; j < container.Rows.Length; j++)
            {
                if (container.Rows[j] == target)
                {
                    return new Vector2Int(i, j);
                }
            }
        }
        return new Vector2Int(-1, -1);

    }
    public bool TryGetTile(Vector2Int target, ref Tile tile)
    {
        if (IsTileOutOfRange(target.x, target.y))
        {
            return false;
        }
        tile = Columns[target.x].Rows[target.y];
        return true;
    }
    public void EnableIndicators(Vector2Int source, Vector2Int[] directions, Color color)
    {
        foreach (Vector2Int tilePos in directions)
        {
            Vector2Int result = new Vector2Int(source.x + tilePos.y, source.y + tilePos.x);
            if (IsTileOutOfRange(result.x, result.y)) continue;

            Color colorOut = color;
            Color colorIn = color;

            GameObject indicatorOut = Columns[result.x].Rows[result.y].IndicatorOut;
            GameObject indicatorIn = Columns[result.x].Rows[result.y].IndicatorIn;

            indicatorIn.SetActive(true);

            Tile tile = GetTile(result);

            SpriteRenderer rendererOut = indicatorOut.GetComponent<SpriteRenderer>();
            SpriteRenderer rendererIn = indicatorIn.GetComponent<SpriteRenderer>();

            colorOut.a = tile.IsFull ? 0.3f : 1f;
            colorIn.a = tile.IsFull ? 0.1f : 0.5f;

            rendererIn.color = colorIn;
            rendererOut.color = colorOut;
        }
    }
    public void EnableIndicator(Vector2Int source, Color color)
    {
        Tile tile = GetTile(source);
        Color colorOut = color;
        Color colorIn = color;

        GameObject indicatorOut = tile.IndicatorOut;
        GameObject indicatorIn = tile.IndicatorIn;

        indicatorIn.SetActive(true);

        SpriteRenderer rendererOut = indicatorOut.GetComponent<SpriteRenderer>();
        SpriteRenderer rendererIn = indicatorIn.GetComponent<SpriteRenderer>();

        colorOut.a = tile.IsFull ? 0.3f : 1f;
        colorIn.a = tile.IsFull ? 0.1f : 0.5f;

        rendererIn.color = colorIn;
        rendererOut.color = colorOut;
    }
    public void DisableIndicators()
    {
        foreach (TileContainer container in Columns)
        {
            foreach (Tile tile in container.Rows)
            {
                if (tile.IndicatorOut.activeSelf)
                {
                    tile.IndicatorIn.SetActive(false);
                }
            }
        }
    }
    public void EnableColliders()
    {
        foreach (TileContainer container in Columns)
        {
            foreach (Tile tile in container.Rows)
            {
                tile.Collider.enabled = true;
            }
        }
    }
    public void DisableColliders()
    {
        foreach (TileContainer container in Columns)
        {
            foreach (Tile tile in container.Rows)
            {
                tile.Collider.enabled = false;
            }
        }
    }
    public void EnableDash(Vector2Int source, Vector2Int[] directions)
    {
        foreach (Vector2Int tile in directions)
        {
            Vector2Int result = new Vector2Int(source.x + tile.y, source.y + tile.x);
            if (IsTileOutOfRange(result.x, result.y)) continue;
            GameObject dashTile = Columns[result.x].Rows[result.y].DashTile;
            dashTile.SetActive(true);
        }
    }
    public void DisableDash(Vector2Int source, Vector2Int[] directions)
    {
        foreach (Vector2Int tile in directions)
        {
            Vector2Int result = new Vector2Int(source.x + tile.y, source.y + tile.x);
            if (IsTileOutOfRange(result.x, result.y)) continue;
            GameObject dashTile = Columns[result.x].Rows[result.y].DashTile;
            dashTile.SetActive(false);
        }
    }
    private bool IsTileOutOfRange(int column, int row)
    {
        return column < 0 || column >= width || row < 0 || row >= height;
    }
    private void SetTileXY()
    {
        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                Columns[x].Rows[y].X = x;
                Columns[x].Rows[y].Y = y;
            }
        }
    }
}
