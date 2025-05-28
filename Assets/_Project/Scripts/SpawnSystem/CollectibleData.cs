using UnityEngine;

namespace Platformer
{
    [CreateAssetMenu(fileName = "CollectibleDate", menuName = "Scriptable Objects/Collectible Data")]
    public class CollectibleData : EntityData
    {
        public int score;
        // additional properties specific to collectibles
    }
}
