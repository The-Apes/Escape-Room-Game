using UnityEngine;

public class DialogueManager : MonoBehaviour
{
 public static DialogueManager Instance;
 
 private NpcAgent npcAgent;
 private Subtitle subtitle;
 private Logs logs;
 public void Awake()
 {
  
  if (Instance == null)
  {
   Instance = this;
   DontDestroyOnLoad(gameObject);
  }
  else
  {
   Destroy(gameObject);
  }

  npcAgent = FindFirstObjectByType<NpcAgent>();
  logs = FindFirstObjectByType<Logs>();
 }

 public void SayLine(string line)
 {
  NpcSpeak(line);
  ChangeSubtitle(line);
  LogLine(line);
 }
 public void NpcSpeak(string line)
 {
  npcAgent.Speak(line);
 }
 public void ChangeSubtitle (string line)
 {
  foreach (Subtitle subtitle in FindObjectsByType<Subtitle>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
  {
   subtitle.SetSubtitle(line);
  }
 }
 public void LogLine(string line)
 {
  logs.lines.Add(line);
  
  //TODO: Implement logging functionality and UI FOR It
 }
 
}
