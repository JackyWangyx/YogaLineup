using System;
using System.Collections.Generic;
using Aya.Extension;

public class PlayerBase : ComponentBase<PlayerBase>
{
    [SubComponent] public Player Self { get; set; }
    [SubComponent] public PlayerState State { get; set; }
    [SubComponent] public PlayerControl Control { get; set; }
    [SubComponent] public PlayerMove Move { get; set; }
    [SubComponent] public PlayerBuff Buff { get; set; }
    [SubComponent] public PlayerRender Render { get; set; }
}
