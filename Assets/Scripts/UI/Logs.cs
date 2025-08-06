using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Logs : MonoBehaviour
{
  [SerializeField] private GameObject content;
  [SerializeField] private GameObject logEntryPrefab;

  public List<String> lines;

  

  public void LogLine(string line, bool player)
  {
    if (player)
    {
      lines.Add("P " + line);
    }
    else
    {
      lines.Add("N " + line);
    }
  }

  public void ShowLogs()
  {
    lines.Reverse();
    foreach (var line in lines)
    {
      GameObject logEntry = Instantiate(logEntryPrefab, content.transform);
      TextMeshProUGUI logText = logEntry.GetComponentInChildren<TextMeshProUGUI>();
      bool isPlayer = line.StartsWith('P');
      String trimmedLine = line.Substring(2).Trim(); // Remove the "P " or "N " prefix
      logText.SetText(trimmedLine);
      logText.color = isPlayer ? Color.yellow : Color.white;
      logEntry.name = "log: "+trimmedLine;
    }
  } public void ClearLogs()
  {
    foreach (Transform child in content.transform)
    {
      Destroy(child.gameObject);
    }
    lines.Reverse();
  }
}
