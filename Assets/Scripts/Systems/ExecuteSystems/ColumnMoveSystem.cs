using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

public class ColumnMoveSystem: IExecuteSystem
{
    private Contexts _contexts;
    private IGroup<GameEntity> _entityToMove;
    private int _targetColumn;
    private bool _canMove;
    private float _startSpeed;
    private List<GameEntity> _columnEntities;
    public ColumnMoveSystem(Contexts contexts,int column)
    {
        this._targetColumn = column;
        _contexts = contexts;
        _entityToMove = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.OnMove, GameMatcher.CanvasPosition));
    }

    public void Move()
    {
        _columnEntities = _entityToMove.GetEntities().Where(e => e.gridPosition.value.x == this._targetColumn).ToList();
        _startSpeed = GameExtension.spinSpeed;
        _canMove = true;
    }

    public void Stop()
    {
        _canMove = false;
    }
    
    public void Execute()
    {
        CheckInterpolation();
        if(!_canMove) return;
        foreach (var entity in _columnEntities)
        {
            if(!entity.isOnMove || !entity.hasCanvasPosition) continue;
            var newPos = entity.canvasPosition.value;
            var step = _startSpeed * Time.deltaTime;
            
            newPos.y -= step;
            entity.ReplaceCanvasPosition(newPos);
            
            if (newPos.y <= _contexts.game.sharedVal.value.bottomBoundary)
            {
                var rest = Mathf.Abs(newPos.y) - Mathf.Abs(_contexts.game.sharedVal.value.bottomBoundary);
                if (entity.hasMoveHelper)
                {
                    entity.ReplaceMoveHelper(rest);
                }
                else
                {
                    entity.AddMoveHelper(rest);
                }
                entity.isOutOfRange = true;
            }
        }
        _startSpeed -= _contexts.game.sharedVal.value.decreaseSpeedAmount;
    }

    private void CheckInterpolation()
    {
        if(!_canMove) return;
        if (_columnEntities == null || _columnEntities.Count <= 0) return;
        if (!(_startSpeed <= _contexts.game.sharedVal.value.minSpeed)) return;
            for (var i = 0; i < _columnEntities.Count; i++)
            {
                _columnEntities[i].isOnMove = false;
                _columnEntities[i].AddTranslateToPosition(_startSpeed);
               
                if (i == _columnEntities.Count - 1)
                {
                    Stop();
                }
            }
    }
}