using UnityEngine;

public class DialogueManager : MonoBehaviour
{
 public static DialogueManager instance;
 
 //Braindump
 // if Script manager talking is false, hid the subtitles after some secconds,
 // Add code to show or hide subtitles based on if player is looking ar NPC or not, should this be here on in Subtitle class doe?
 
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
  _logs = FindFirstObjectByType<Logs>();
 }

 public void SayLine(string line, bool player = false)
 {
  if (!player) NpcSpeak(line);
  ChangeSubtitle(line, player);
  _logs.LogLine(line, player);
 }
 public void NpcSpeak(string line)
 {
  _npcAgent.Speak(line);
 }
 public void ChangeSubtitle (string line, bool player)
 {
  foreach (Subtitle subtitle in FindObjectsByType<Subtitle>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
  {
   subtitle.SetSubtitle(line, player);
  }
 }

  
  
  //TODO: Implement logging functionality and UI FOR It
 }
