using Entitas;
using UnityEngine;

public class InterpolationSystem:IExecuteSystem
{
    private Contexts _contexts;
    private IGroup<GameEntity> _entities;
    
    public InterpolationSystem(Contexts contexts)
    {
        this._contexts = contexts;
        _entities = this._contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.TranslateToPosition,
            GameMatcher.CanvasPosition, GameMatcher.GridPosition,GameMatcher.View));
    }
    
    public void Execute()
    {
        foreach (var entity in _entities.GetEntities())
        {
            if(!entity.hasTranslateToPosition) continue;
            
            var dir = GameExtension.GetCanvasPosition(entity.gridPosition.value) - entity.canvasPosition.value;
            var newPosition = entity.canvasPosition.value + dir.normalized * entity.translateToPosition.startSpeed * Time.deltaTime;
            entity.ReplaceCanvasPosition(newPosition);

            var dist = dir.magnitude;
            if (dist <= _contexts.game.sharedVal.value.minDistance)
            {
                //complete
                entity.ReplaceCanvasPosition(GameExtension.GetCanvasPosition(entity.gridPosition.value));
                entity.RemoveTranslateToPosition();
                entity.isMoveComplete = true;
                entity.isOutOfRange = false;
                entity.isInterpolationComplete = true;
            }
            
        }
    }
}