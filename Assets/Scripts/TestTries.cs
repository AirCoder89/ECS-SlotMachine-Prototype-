using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public struct Cell
{
    public string value;
    public List<string> nextValues;
    public List<string> iterations;

    public Cell(string val)
    {
        this.value = val;
        nextValues = new List<string>(3);
        iterations = new List<string>(3);
    }
}
public class TestTries : MonoBehaviour
{
    public static TestTries Instance;

    [ShowInInspector]public SlotType[,] typeMatrix;
    public List<string> resultPayLines;
    public Cell[,] tmpMatValues;
    
    [TabGroup("Trie")]public List<string> inData;
    [TabGroup("Matrix")] [ShowInInspector] public string[,] matrix;
    [TabGroup("Matrix")] public List<string> generatedPaylines;
    private PayLinesTrie _tries;
    private System.Random _random = new System.Random();
    
    
     public void CheckMatchingPayLine()
        {
          
        }

    //-----------------------------------
    private void Start()
    {
        this._tries = new PayLinesTrie();
    }

}
