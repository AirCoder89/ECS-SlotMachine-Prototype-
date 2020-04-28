using System.Collections.Generic;
using Entitas;


public class TypeGeneratorSystem: ReactiveSystem<GameEntity>
{
    private Contexts _contexts;
    
    public TypeGeneratorSystem(Contexts context) : base(context.game)
    {
        _contexts = context;
    }


    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(
            GameMatcher.OutOfRange, GameMatcher.OnMove,GameMatcher.Type
        ));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isOutOfRange && entity.isOnMove && entity.hasType;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.ReplaceType(this._contexts.game.sharedVal.value.GetRandomSlotInfo());
        }
    }
}