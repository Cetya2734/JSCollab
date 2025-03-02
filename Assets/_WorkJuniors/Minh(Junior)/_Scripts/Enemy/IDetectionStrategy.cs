using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public interface IDetectionStrategy
{
    bool Execute(Transform player, Transform detector, CountdownTimer timer);
}