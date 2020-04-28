using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum SlotType
{
    Red,Orange,Blue,Heart,Diamond,Yellow,Green,Energy
}


[System.Serializable]
public struct Dimension
{
    public int x;
    public int y;
    
    public Dimension(int xPos, int yPos)
    {
        this.x = xPos;
        this.y = yPos;
    }
    
}

[System.Serializable]
public struct SlotInfo
{
    public SlotType type;
    [PreviewField]public Sprite sprite;
    [PreviewField]public Sprite background;
}

[CreateAssetMenu(menuName = "SharedValues",fileName = "SharedValues")]
public class SharedValues : ScriptableObject
{
    [TableList] public List<SlotInfo> slotsResources;

    [BoxGroup("Matrix")][ShowInInspector] public Sprite[,] matrix;
    
    
    public GameObject slotItemPrefab;
    public RectTransform holder;
    public Dimension dimension;
    public Vector2 offset;
    public Vector2 spacing;

    public float bottomBoundary;
    public float topPosition;
    public Vector2 delayAmongColumnMoveRange;
    public Vector2 speedRange;
    public float decreaseSpeedAmount;
    public float minSpeed;
    public float minDistance;
    public SlotInfo GetRandomSlotInfo()
    {
        var rnd = Random.Range(0, slotsResources.Count);
        return this.slotsResources[rnd];
    }
}
