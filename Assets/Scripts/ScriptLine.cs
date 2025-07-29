using System;
using UnityEngine.Serialization;

[Serializable]
public class ScriptLine
{
    public String text;
    public String condition;
    //public Condition condition;
    
    public Action[] actions; // maybe make it a string too for additional parameters?

    public enum Action
    {
        Dance,
        LookAt,
    }
    
    //TODO
    // with the actions, I need a way to pass in a command "LookAT" and afterwards it provied a game object or a transfrom
    // idk how to do that with the current setup and da enums,  
    // 
    // Maybe make a third field for custom string fields or somethign?? or make a new class called commands? with a 
    // child class of a "LookAt" command that has a transform field?
}
