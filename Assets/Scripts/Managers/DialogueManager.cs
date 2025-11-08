using Npc;
using Player;
using UI;
using UnityEngine;

namespace Managers
{
 public class DialogueManager : MonoBehaviour
 {
  public static DialogueManager instance;
 
  //Brain dump
  // if Script manager talking is false, hid the subtitles after some seconds,
  // Add code to show or hide subtitles based on if player is looking ar NPC or not, should this be here on in Subtitle class doe?
 
  private FpController _player;
  private NpcAgent _npcAgent;
  private Subtitle _subtitle;
  private Logs _logs;
  public void Awake()
  {
  
   if (instance == null)
   {
    instance = this;
    DontDestroyOnLoad(gameObject);
   }
   else
   {
    Destroy(gameObject);
   }

   _npcAgent = FindFirstObjectByType<NpcAgent>();
   _player = FindFirstObjectByType<FpController>();
   _logs = FindFirstObjectByType<Logs>();
  }

  public void SayLine(string line, bool player = false, float duration = -1f)
  {
   float time = duration;
   if (Mathf.Approximately(time, -1f)) time = Mathf.Max(1,line.Split(' ').Length * 0.70f); // 0.70 seconds per word, can be adjusted
  
   if (!player) _npcAgent.Speak(line, time);
   if (player) _player.PlayTalkSound();
   ChangeSubtitle(line, player, time);
   _logs.LogLine(line, player);
  }

  private void ChangeSubtitle (string line, bool player, float duration)
  {
   foreach (Subtitle subtitle in FindObjectsByType<Subtitle>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
   {
    subtitle.SetSubtitle(line, player, duration);
   }
  }

  
  
  //TODO: Implement logging functionality and UI FOR It
 }
}
