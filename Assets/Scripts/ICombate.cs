using System;
using UnityEngine;

    public interface IAttack
    {
        public AtacSO atac { set; }
    }

    public interface IDamageable
    {
        public event Action<AtacSO> onDamaged;
        void RebreMal(AtacSO atac);
    }

