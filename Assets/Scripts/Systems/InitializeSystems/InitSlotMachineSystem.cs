using Entitas;

public class InitSlotMachineSystem : IInitializeSystem
{
    private Contexts _contexts;
    
    public InitSlotMachineSystem(Contexts contexts)
    {
        this._contexts = contexts;
    }
    
    public void Initialize()
    {
        var dimension = this._contexts.game.sharedVal.value.dimension;

        for (var y = 0; y < dimension.y; y++)
        {
            for (var x = 0; x < dimension.x; x++)
            {
                GameExtension.CreateEntity(
                    this._contexts,
                    new Dimension(x, y),
                    y != 0
                );
            }
        }
    }
}