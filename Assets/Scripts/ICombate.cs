using System;

public interface IAttack
{
    public AtacSO atac { set; }
}

public interface IDamageable
{
    public event Action<AtacSO> onDamaged;
    void RebreMal(AtacSO atac);
}

public interface Tornable
{
    public void IniciarTorn();

    public void AcabarTorn();
}


