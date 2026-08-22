using UnityEngine;
using UnityEngine.Tilemaps;

namespace ArdJam2026.Gameplay
{
    [CreateAssetMenu(menuName = "Game/Gameplay Tile", fileName = "GameplayTile")]
    public class GameplayTile : Tile
    {
        private enum TileType
        {
            Default,
            Collision,
            Death
        }

        [SerializeField]
        private TileType type;

        public bool Collides => type == TileType.Collision;
        public bool Deadly => type == TileType.Death;
    }
}