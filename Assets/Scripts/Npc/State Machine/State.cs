/*Title: An introduction to finite state machines and the state patterm for game development
Author: The Shaggy Dev
Date: 12/08/2025
Availability: https://www.youtu.be/-ZP2Xm-mY4E?si=vA2NjSFFdNNDiejb
*/

using UnityEngine;

public interface State
{
    public State StateUpdate() //needs to return a state ig
    {
        return null;
    }
    public void enterState()
    {
        
    }

    public void exitState()
    {
        
    }
        
}
