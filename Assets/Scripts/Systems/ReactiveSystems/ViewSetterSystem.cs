using System.Collections.Generic;
using Entitas;

public class ViewSetterSystem: ReactiveSystem<GameEntity>
{
    private Contexts _contexts;
    
    public ViewSetterSystem(Contexts contexts) : base(contexts.game)
    {
        this._contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(GameMatcher.View, GameMatcher.Type, GameMatcher.CanvasPosition));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasView && entity.hasType && entity.hasCanvasPosition;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.view.background.sprite = entity.type.value.background;
            entity.view.icon.sprite = entity.type.value.sprite;
        }
    }
}