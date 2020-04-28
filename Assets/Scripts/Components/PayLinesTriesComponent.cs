using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game,Unique]
public class PayLinesTriesComponent:IComponent
{
    public PayLinesTrie value;
}