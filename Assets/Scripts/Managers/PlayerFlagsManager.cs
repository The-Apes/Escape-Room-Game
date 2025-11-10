using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class PlayerFlagsManager : MonoBehaviour
    {
        public static PlayerFlagsManager instance;

        private void Awake()
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
        }

        [NonSerialized] public bool PickedUpItem;
        [NonSerialized] public bool DroppedAnItem;
        [NonSerialized] public bool InspectedAnItem;
        [NonSerialized] public bool ReadAnItem;
        [NonSerialized] public bool InteractedWithNpc;
        [NonSerialized] public bool SelectedChoice;
        [NonSerialized] public bool GaveNpcAnItem;
        [NonSerialized] public bool UsedTorch;

        
        //List of script names that are do once
        [NonSerialized] public List<string> CompletedScripts = new List<string>();
    }
}
