using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawningPlatform : MonoBehaviour
{
    [SerializeField]private GameObject platform;

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.name == "Player")
        {
            Invoke("Despawn", .3f);
        }
    }

    void Despawn()
    {
        platform.SetActive(false);
        Invoke("RespawnAfterCooldown", 2f);
    }

    void RespawnAfterCooldown()
    {
        platform.SetActive(true);
    }
}
