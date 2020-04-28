
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class PositionTranslatorSystem : ReactiveSystem<GameEntity>
{
    private Contexts _contexts;
    
    public PositionTranslatorSystem(Contexts context) : base(context.game)
    {
        this._contexts = context;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.GridPosition);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasGridPosition && !entity.isOnMove && entity.isMoveComplete;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            if(entity.isOnMove || !entity.isMoveComplete) continue;
            var spacing = _contexts.game.sharedVal.value.spacing;
            var offset = _contexts.game.sharedVal.value.offset;
            var gridPos = entity.gridPosition.value;
            var desiredPos = new Vector2(gridPos.x * spacing.x, gridPos.y * spacing.y);

            entity.isVisible = entity.gridPosition.value.y != 0;
            //offset
            desiredPos.x += offset.x;
            desiredPos.y += offset.y;

            if (entity.hasCanvasPosition)
            {
                entity.ReplaceCanvasPosition(desiredPos);
            }
            else
            {
                entity.AddCanvasPosition(desiredPos);
            }
            
        }
    }
}