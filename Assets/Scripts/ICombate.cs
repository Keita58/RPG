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

public interface Avisable
{
    public event Action<string> OnIniciarTornUI;
    public event Action<string, string> OnRebreEstadoAlteradoUI;
    public event Action<string, int> OnRebreMalUI;
    public event Action<string> OnEmpezarVentajaUI;
    public event Action OnSeleccionarTargetUI;
}


