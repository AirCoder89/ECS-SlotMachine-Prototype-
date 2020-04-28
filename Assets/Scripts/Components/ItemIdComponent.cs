using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game]
public class ItemIdComponent:IComponent
{
   [EntityIndex]
   public string value;
}