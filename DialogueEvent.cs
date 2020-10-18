using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public struct DialogueEvent
{
    public string Name;
    public UnityEvent Event;
}
