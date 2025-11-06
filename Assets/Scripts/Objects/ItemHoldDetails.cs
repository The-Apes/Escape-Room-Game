using UnityEngine;

namespace Objects
{
    public class ItemHoldDetails : MonoBehaviour
    {
        public Vector3 holdPositionOffset;
        public Vector3 holdRotationOffset;
        public Vector3 holdScaleOffset = Vector3.one;
        public HoldStyle holdStyle;
        public enum HoldStyle
        {
            Handle,
            Cup,
            Lantern,
            Palm,
            Key,
            Paper
        }
    }
}
