using UnityEngine;

public class Tile : MonoBehaviour
{
    public int X;
    public int Y;
    public GameObject IndicatorOut;
    public GameObject IndicatorIn;
    public GameObject DashTile;
    public bool IsFull;
    public Enemy Enemy;
    public Collider Collider;
    public ICollectable Collectable;
}
