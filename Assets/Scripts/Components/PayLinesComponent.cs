using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game,Unique]
public class PayLinesComponent:IComponent
{
        public List<PayLines> value;
}