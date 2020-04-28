using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class PositionSetterSystem : IExecuteSystem
{
    private Contexts _contexts;
    private IGroup<GameEntity> _targetGroup;
    
    public PositionSetterSystem(Contexts contexts)
    {
        this._contexts = contexts;
        _targetGroup = _contexts.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.CanvasPosition, GameMatcher.View));
    }
    
    public void Execute()
    {
        foreach (var entity in _targetGroup.GetEntities())
        {
            entity.view.background.gameObject.GetComponent<RectTransform>().anchoredPosition =
                entity.canvasPosition.value;
        }
    }
}