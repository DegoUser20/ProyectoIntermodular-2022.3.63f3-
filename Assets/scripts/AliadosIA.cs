using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliadosIA : MonoBehaviour
{
    
    public Transform player;            // referencia al player
    public float followDistance = 15f;   // Distancia que intenta mantener
    public float speed = 3f;
    
    public float maxFollowDistance;

    public float detectionRange = 20f;
    public float fireRate = 1f;

    Transform currentTarget;            // enemigo actual
    float nextFireTime;

    Animator animacion;
    Vector3 lastPos;

    void Awake()
    {
        animacion = GetComponent<Animator>();
        lastPos = transform.position;
    }

    void Update()
    {
        SeguirJugador();
        BuscarObjetivo();
        AtacarObjetivo();
        ActualizarAnimacionMovimiento();

    }

    
    void SeguirJugador()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        
        //Si estoy demasiado lejos del jugador, dejo de seguirlo
        if (dist > maxFollowDistance)
            return;
        
        if (dist > followDistance)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            dir.y = 0;
            transform.position += dir * speed * Time.deltaTime;
            transform.forward = dir;
        }
    }

    void BuscarObjetivo()
    {
        if (currentTarget != null) return;

        // Busca el primer enemigo con tag "Enemy"
        GameObject enemigo = GameObject.FindWithTag("Enemigo");
        if (enemigo != null)
        {
            float dist = Vector3.Distance(transform.position, enemigo.transform.position);
            if (dist <= detectionRange)
                currentTarget = enemigo.transform;
        }    
    }

    void AtacarObjetivo()
    {
        if (currentTarget == null)
        {
            animacion.SetBool("disparar", false);
            return;
        }

        float dist = Vector3.Distance(transform.position, currentTarget.position);
        if (dist > detectionRange)
        {
            currentTarget = null;
            animacion.SetBool("disparar", false);
            return;
        }

        //Si está lejos del enemigo, avanzar hacia él.
        float stopDistance = 5f; //distancia a la que se planta para disparar

        if (dist > stopDistance)
        {
            Vector3 moverDireccion = (currentTarget.position - transform.position).normalized;
            moverDireccion.y = 0;
            transform.position += moverDireccion * speed * Time.deltaTime;
            transform.forward = moverDireccion;
            animacion.SetBool("disparar", false);       //Corre hacia el enemigo
        }
        else
        {
            //Ya está a distancia: solo rota y dispara
            Vector3 dir = currentTarget.position - transform.position;
            dir.y = 0;
            transform.forward = dir.normalized;

            if (Time.time >= nextFireTime)
            {
                Debug.Log("Aliado dispara al enemigo");
            
                // Buscar vida en el enemigo para eliminarlo
                vidaMaxima vidaEnemigo = currentTarget.GetComponent<vidaMaxima>();
                if (vidaEnemigo != null)
                    vidaEnemigo.TakeDamage(10);         // 10 puntos de daño de vida

                nextFireTime = Time.time + 1f / fireRate;
            
            }

            animacion.SetBool("disparar", true);        // animación de disparo
        }
    }

    void ActualizarAnimacionMovimiento()
    {
        float currentSpeed = (transform.position - lastPos).magnitude / Time.deltaTime;
        animacion.SetFloat("Velocidad", currentSpeed);
        lastPos = transform.position;
    }

}
