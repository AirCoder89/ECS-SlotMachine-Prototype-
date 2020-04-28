using Entitas;
using UnityEngine;

public class MatrixCreatorSystem:ICleanupSystem
{
    private Contexts _contexts;
    private IGroup<GameEntity> _targetGroup;
    private Dimension _dimension;
    private int _lastSize;
    private int _nbCompleteColumn;
    
    public MatrixCreatorSystem(Contexts contexts)
    {
        _nbCompleteColumn = 0;
        _lastSize = 0;
        this._contexts = contexts;
        _targetGroup = _contexts.game.GetGroup(GameMatcher.InterpolationComplete);
        _dimension = _contexts.game.sharedVal.value.dimension;
        _contexts.game.sharedVal.value.matrix = new Sprite[_dimension.x,_dimension.y-1];
        //GameExtension.resultMatrix = new SlotType[_dimension.x,_dimension.y-1];
        _contexts.game.testing.value.typeMatrix = new SlotType[_dimension.x,_dimension.y-1];
    }
    
    public void Cleanup()
    {
        if (_lastSize == 0 && _targetGroup.GetEntities().Length > 0)
        {
            _lastSize = _targetGroup.GetEntities().Length;
        }
        if (_targetGroup.GetEntities().Length == 0 && _lastSize > 0)
        {
            _lastSize = 0;
            _nbCompleteColumn++;
            Debug.Log("Column " +  _nbCompleteColumn + "Complete");
            if (_nbCompleteColumn >= _dimension.x)
            {
                _nbCompleteColumn = 0;
                Debug.Log("All Column Complete .. Time for matching !");
                //GameExtension.CheckMatchingPayLine();
                _contexts.game.testing.value.CheckMatchingPayLine();
            }
            
        }
        foreach (var entity in _targetGroup.GetEntities())
        {
            entity.isInterpolationComplete = false;
            if (entity.gridPosition.value.y == 0)
            {
                entity.isVisible = false;
                continue;
            }
            var gridPos = entity.gridPosition.value;
            //_contexts.game.sharedVal.value.matrix[gridPos.x, gridPos.y - 1] = entity.entityHelper.value;
            _contexts.game.sharedVal.value.matrix[gridPos.x, gridPos.y - 1] = entity.type.value.sprite;
            //GameExtension.resultMatrix[gridPos.x, gridPos.y - 1] = entity.type.value.type;
            _contexts.game.testing.value.typeMatrix[gridPos.x, gridPos.y - 1] = entity.type.value.type;
        }
    }
}