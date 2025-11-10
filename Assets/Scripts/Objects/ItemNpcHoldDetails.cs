using UnityEngine;

namespace Objects
{
    public class ItemNpcHoldDetails : MonoBehaviour
    {
        public Vector3 holdPositionOffset;
        public Vector3 holdRotationOffset;
        //public Vector3 holdScaleOffset = Vector3.one; I have no time for Unity's bullshit right now, so I'm removing this feature because i like my sanity
        public float itemSize;
        public bool small;
    }
}
