using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
public class ActionChase : FSMAction    
{
    [Header("Config")]
    [SerializeField] private float chaseSpeed;
    private EnemyBrain enemybrain;

    private void Awake()
    {
        enemybrain = GetComponent<EnemyBrain>();
    }
    public override void Act()
    {
        ChasePlayer();
    }
    private void ChasePlayer()
    {
        if (enemybrain.Player == null)
        {
            return;
        }
        Vector3 DirToPlayer = enemybrain.Player.position - transform.position;
        if (DirToPlayer.magnitude >1.25f)
        {
            transform.Translate(DirToPlayer.normalized * (chaseSpeed * Time.deltaTime));
        }
    }
}
