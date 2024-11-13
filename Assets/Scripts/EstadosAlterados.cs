using UnityEngine;

public class EstadosAlterados
{
    int incapacitat;
    int torns;
    int hp;
    int modAtk;
    int modDef;
    int modSpd;

    public int Torns { get => torns; set => torns = value; }
    public int Incapacitat { get => incapacitat; set => incapacitat = value; }
    public int Hp { get => hp; set => hp = value; }
    public int ModAtk { get => modAtk; set => modAtk = value; }
    public int ModDef { get => modDef; set => modDef = value; }
    public int ModSpd { get => modSpd; set => modSpd = value; }

    public void IniciarEstadoAlterado(EstadoAlteradoSO estadoSo)
    {
        EstadosAlterados estado = new EstadosAlterados(estadoSo.incapacitat, estadoSo.torns, estadoSo.hp, estadoSo.modAtk, estadoSo.modDef, estadoSo.modDef);
    }

    public EstadosAlterados(int incapacitat, int torns, int hp, int modAtk, int modDef, int modSpd)
    {
        this.incapacitat= incapacitat;
        this.torns= torns;
        this.hp= hp;
        this.modAtk= modAtk;
        this.modDef= modDef;
        this.modSpd= modSpd;
    }


}
