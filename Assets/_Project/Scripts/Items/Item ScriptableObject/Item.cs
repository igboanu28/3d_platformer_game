using UnityEngine;

namespace Platformer
{
    [CreateAssetMenu(fileName ="New Item",menuName = "Scriptable Objects/Create New Item")]
    public class Item : ScriptableObject
    {
        public int id;
        public string itemName;
        public int value;
        public Sprite icon;
    }
}
