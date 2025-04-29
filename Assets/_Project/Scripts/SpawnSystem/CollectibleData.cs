using UnityEngine;

namespace Platformer
{
    [CreateAssetMenu(fileName = "CollectibleDate", menuName = "Platformer/Collectible Data")]
    public class CollectibleData : EntityData
    {
        public int score;
        // additional properties specific to collectibles
    }
}
