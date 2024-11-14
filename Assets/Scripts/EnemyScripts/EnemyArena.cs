using UnityEngine;

public class EnemyArena : MonoBehaviour
{
    public EnemySO EnemySO;
    public int id;
    private int hp;
    private int atk;
    private int def;
    private int spd;
    private int mana;

    void Awake()
    {
        this.hp = this.EnemySO.hp;
        this.atk = this.EnemySO.atk;
        this.def = this.EnemySO.def;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
