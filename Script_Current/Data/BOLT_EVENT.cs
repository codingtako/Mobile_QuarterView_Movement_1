using System.Collections;
using System.Collections.Generic;
using Bolt.Addons.Community;
using Ludiq;
using UnityEngine;

#region �⺻

[IncludeInSettings(true)]
public class NB_Roll_Down : IDefinedEvent
    {
    
    }

[IncludeInSettings(true)]
public class NB_Roll_Up : IDefinedEvent
{

}
[IncludeInSettings(true)]
public class Bolt_Attack: IDefinedEvent
{
    
}
[IncludeInSettings(true)]
public class Bolt_AttackFinish: IDefinedEvent
{
    
}
[IncludeInSettings(true)]
public class Bolt_LeftHandAttack: IDefinedEvent
{
    
}
[IncludeInSettings(true)]
public class Bolt_RightHandAttack: IDefinedEvent
{
    
}
#endregion
#region
[IncludeInSettings(true)]
public class Player_MoveBegin : IDefinedEvent
{
    
}
[IncludeInSettings(true)]
public class Player_MoveFinish : IDefinedEvent
{
    
}
#endregion