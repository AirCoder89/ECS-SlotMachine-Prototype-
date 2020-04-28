using System;
using System.Collections;
using System.Collections.Generic;
using Entitas;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public struct PayLineData
{
    public string storedData;
    public string payLineValue;
}

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public bool debugWithSpace;
    public SharedValues sharedValues;
    public TestTries testing;
    [TabGroup("PayLines")][ReadOnly] public List<PayLineData> payLinesData;
    [TabGroup("PayLines")] public List<PayLines> paylines;
    
    
    private Systems _systems;
    private Contexts _contexts;
    private  ColumnMoveSystem[] _Movesystems;
    private int _systemCounter;
    private PayLinesTrie _trie;
    
    private void Awake()
    {
        if(Instance != null) return;
        Instance = this;
    }

    [TabGroup("PayLines")]
    [Button("Save", ButtonSizes.Medium)]
    private void SavePayLines()
    {
        payLinesData = new List<PayLineData>();
        foreach (var payLine in paylines)
        {
            payLinesData.Add(new PayLineData()
            {
                storedData = payLine.ToString(),
                payLineValue = payLine.GetValue()
            });
        }
    }
    
    [TabGroup("PayLines")]
    [Button("Load", ButtonSizes.Medium)]
    private void LoadPayLines()
    {
        var y = 0;
        paylines = new List<PayLines>();
        foreach (var payLine in payLinesData)
        {
            var pre = payLine.storedData.Split(char.Parse("&"));
            var points = 0;
            int.TryParse(pre[1], out points);
            var m = new bool[5, 3];
            var rows = pre[0].Split(char.Parse("#"));
            foreach (var row in rows)
            {
                if(string.IsNullOrEmpty(row)) continue;
                    for (var x = 0; x < row.Length; x++)
                    {
                        var ch = row.Substring(x, 1);
                        m[x, y] = ch == "1";
                    }

                    y++;
            }

            y = 0;
            var pl = new PayLines();
            pl.points = points;
            pl.SetPayLine(m);
            paylines.Add(pl);
            
        }
    }
    
    
    void Start()
    {
        this._contexts = Contexts.sharedInstance;
        this._contexts.game.SetSharedVal(this.sharedValues);
        this._contexts.game.SetPayLines(this.paylines);
        
        this._contexts.game.SetTesting(testing);
        SetPayLines();
        this._contexts.game.SetPayLinesTries(this._trie);
        GameExtension.SetContexts(this._contexts);
        
        this._systems = CreateSystems();
        this._systems.Initialize();
        LoadPayLines();
    }

    private void SetPayLines()
    {
        _trie = new PayLinesTrie();
        foreach (var data in payLinesData)
        {
            _trie.Insert(data.payLineValue);
        }
    }
    
    void Update()
    {
        if (!debugWithSpace)
        {
            this._systems.Execute();
            this._systems.Cleanup();
            
        }else if (Input.GetKeyDown(KeyCode.Space))
        {
            this._systems.Execute();
            this._systems.Cleanup();
        }
    }

    private Systems CreateSystems()
    {
        var feature = new Feature("Systems")
                .Add(new InitSlotMachineSystem(this._contexts))
                .Add(new PositionTranslatorSystem(this._contexts))
                .Add(new AddViewSystem(this._contexts))
                .Add(new ViewSetterSystem(this._contexts));

        
        _Movesystems = new ColumnMoveSystem[this.sharedValues.dimension.x];
        for (var x = 0; x < this.sharedValues.dimension.x; x++)
        {
            var systemMove = new ColumnMoveSystem(this._contexts, x);
            _Movesystems[x] = systemMove;
            feature.Add(systemMove);
        }

        feature.Add(new TypeGeneratorSystem(this._contexts));
        feature.Add(new PositionSetterSystem(this._contexts));
        feature.Add(new OutOfRangeSystem(this._contexts));
        feature.Add(new InterpolationSystem(this._contexts));
        feature.Add(new MatrixCreatorSystem(this._contexts));
        return feature;
    }

    
    public void StartSpinning()
    {
        _systemCounter = 0;
        OnSpine();
    }

    private void OnSpine()
    {
        if (_systemCounter < _Movesystems.Length)
        {
            this._Movesystems[_systemCounter].Move();
            var randomDelay = Random.Range(sharedValues.delayAmongColumnMoveRange.x,
                sharedValues.delayAmongColumnMoveRange.y);
            StartCoroutine(WaitAndExecuteNextMoveSystem(randomDelay));
        }
        else
        {
            StopAllCoroutines();
        }
    }

    public void StopAllMoveSystem()
    {
        foreach (var system in _Movesystems)
        {
            system.Stop();
        }
    }
    private IEnumerator WaitAndExecuteNextMoveSystem(float delay)
    {
        yield return new WaitForSeconds(delay);
        _systemCounter++;
        OnSpine();
    }
}
