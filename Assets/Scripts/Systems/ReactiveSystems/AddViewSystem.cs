using System.Collections.Generic;
using Entitas;
using UnityEngine;
using UnityEngine.UI;

public class AddViewSystem: ReactiveSystem<GameEntity>
{
    private Contexts _contexts;

    public AddViewSystem(Contexts contexts): base(contexts.game)
    {
        this._contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(GameMatcher.GridPosition, GameMatcher.Type));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasType && entity.hasGridPosition;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        var prefab = this._contexts.game.sharedVal.value.slotItemPrefab;
        var holder = this._contexts.game.sharedVal.value.holder;

        foreach (var entity in entities)
        {
            if(entity.hasView) continue;
            var itemView = GameObject.Instantiate(prefab, holder);
            itemView.name = entity.itemId.value;
            var background = itemView.GetComponent<Image>();
            var icon = itemView.transform.GetChild(0).gameObject.GetComponent<Image>();
            entity.AddView(background,icon);
        }
    }
}