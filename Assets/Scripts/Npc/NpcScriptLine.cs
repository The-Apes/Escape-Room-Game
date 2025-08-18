using System;
using UnityEngine;

namespace Npc
{
    [Serializable]
    public class ScriptLine
    {
        public String text;
        public bool player;
        public String continueCondition = "wait: 2";
        public String[] actions; // maybe make it a string too for additional parameters?

        [Header("other")] 
        public GameObject customObject;
        public Vector3 location;


        //TODO
        // with the actions, I need a way to pass in a command "LookAT" and afterwards it provides a game object or a transform
        // idk how to do that with the current setup and da enums,  
        // 
        // Maybe make a third field for custom string fields or something?? or make a new class called commands? with a 
        // child class of a "LookAt" command that has a transform field?
    }
}
