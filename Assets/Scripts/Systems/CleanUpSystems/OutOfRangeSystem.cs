using System.Linq;
using Entitas;

public class OutOfRangeSystem:ICleanupSystem
{
    private Contexts _contexts;
    private IGroup<GameEntity> _entityOutOfRange;
    private IGroup<GameEntity> _entityOnMove;

    public OutOfRangeSystem(Contexts contexts)
    {
        this._contexts = contexts;
        _entityOutOfRange = _contexts.game.GetGroup(GameMatcher.OutOfRange);
        _entityOnMove =  _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.OnMove, GameMatcher.CanvasPosition));
    }
    
    public void Cleanup()
    {
        foreach (var entity in _entityOutOfRange.GetEntities())
        {
            entity.isOutOfRange = false;
            var pos = entity.canvasPosition.value;
            pos.y = (_contexts.game.sharedVal.value.topPosition - entity.moveHelper.value);
            entity.ReplaceCanvasPosition(pos);

            var columnEntities = _entityOnMove.GetEntities()
                .Where(e => e.gridPosition.value.x == entity.gridPosition.value.x 
                            && e.itemId != entity.itemId).ToList();
            entity.ReplaceGridPosition(new Dimension(entity.gridPosition.value.x,0));
            entity.isVisible = false;
            foreach (var previousEntity in columnEntities)
            {
                var newPos = new Dimension(previousEntity.gridPosition.value.x,previousEntity.gridPosition.value.y + 1);
                previousEntity.ReplaceGridPosition(newPos);
                previousEntity.isVisible = true;
            }
        }
    }
}