using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMove :IBuffTarget
{
    Stat MoveSpeed { get; }
}
