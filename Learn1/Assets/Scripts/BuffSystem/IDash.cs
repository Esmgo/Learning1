using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDash : IBuffTarget
{
    Stat DashSpeed { get; }
    Stat DashCooldown { get; }
    Stat DashDuration { get; }
}
