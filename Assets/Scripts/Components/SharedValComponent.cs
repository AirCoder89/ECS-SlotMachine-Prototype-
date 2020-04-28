using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game,Unique]
public class SharedValComponent:IComponent
{
    public SharedValues value;
}