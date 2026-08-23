using System;
using UnityEngine;
using UnityEngine.UI;

namespace ArdJam2026.Gameplay.UI
{
    public class PossessedIndicatorItem : MonoBehaviour
    {
        [SerializeField]
        private Image target;

        [SerializeField]
        private Sprite possessed;

        [SerializeField]
        private Sprite unpossessed;
        private Pawn pawn;

        public void Initialize(Pawn pawn)
        {
            this.pawn = pawn;
            Refresh();
        }

        public void Refresh()
        {
            target.sprite = pawn.IsPossessed ? possessed : unpossessed;
        }
    }
}