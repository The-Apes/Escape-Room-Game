using UnityEngine;
using System.Collections.Generic;

namespace Objects
{
    public class Interactable : MonoBehaviour
    {
        private Material _outlineMaterial;

        private List<Renderer> _renderers = new List<Renderer>();
        // Cache a copy of the original materials array for each renderer (not a reference!)
        private Dictionary<Renderer, Material[]> _originalMats = new Dictionary<Renderer, Material[]>();
        private bool _isHighlighted;

        private void Awake()
        {
            _renderers.AddRange(GetComponentsInChildren<Renderer>());
            _outlineMaterial = Resources.Load<Material>("Outline");

            foreach (Renderer rend in _renderers)
            {
                // Make a *copy* of the materials array so later changes won't mutate this cache.
                Material[] src = rend.materials; // returns a copy, but we'll defensively copy it again
                Material[] copy = new Material[src.Length];
                for (int i = 0; i < src.Length; i++)
                    copy[i] = src[i];
                _originalMats[rend] = copy;
            }
        }

        public void Highlight(bool enable)
        {
            if (enable && !_isHighlighted)
            {
                foreach (Renderer rend in _renderers)
                {
                    // Start from the cached ORIGINAL copy to build a new array to assign.
                    Material[] original = _originalMats[rend];
                    // create a new array so we never mutate 'original'
                    Material[] mats = new Material[Mathf.Max(1, original.Length)];

                    // copy originals into the new array
                    for (int i = 0; i < original.Length; i++)
                        mats[i] = original[i];

                    // ensure we have at least 2 slots without changing the cached original
                    if (mats.Length < 2)
                    {
                        System.Array.Resize(ref mats, 2);
                        mats[1] = mats[0]; // duplicate reference, not a new instance; fine for slot filler
                    }

                    // set the second slot to the outline material
                    mats[1] = _outlineMaterial;
                    rend.materials = mats; // assign the new array
                }

                _isHighlighted = true;
            }
            else if (!enable && _isHighlighted)
            {
                foreach (Renderer rend in _renderers)
                {
                    // restore the cached untouched original array
                    if (_originalMats.TryGetValue(rend, out Material[] original))
                    {
                        // assign a copy of original to avoid future accidental mutation
                        Material[] restore = new Material[original.Length];
                        for (int i = 0; i < original.Length; i++)
                            restore[i] = original[i];

                        rend.materials = restore;
                    }
                }

                _isHighlighted = false;
            }
        }
    }
}
