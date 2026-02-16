using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigosIA : MonoBehaviour
{
    public float detectionRange = 20f;
    public float fireRate = 1f;
    public float movimiento = 2f;           //Moverse hacia el aliado
    public float DetenerseDist = 6f;        //Distancia mínima para disparar

    Transform currentTarget;                //Aliado actual
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
        BuscarObjetivo();
        AtacarObjetivo();
        ActualizarAnimacionMovimiento();
    }

    void ActualizarAnimacionMovimiento()
    {
        float currentSpeed = (transform.position - lastPos).magnitude / Time.deltaTime;
        animacion.SetFloat("Velocidad", currentSpeed);
        lastPos = transform.position;
    }
    void BuscarObjetivo()
    {
        if (currentTarget != null) return;

        //Busca un aliado por tag
        GameObject aliado = GameObject.FindWithTag("Aliado");
        if (aliado != null)
        {
            float dist = Vector3.Distance(transform.position, aliado.transform.position);
            if (dist <= detectionRange)
                currentTarget = aliado.transform;
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

        // Si está lejos del aliado, moverse hacia él
        if (dist > DetenerseDist)
        {
            Vector3 moverDireccion = (currentTarget.position - transform.position).normalized;
            moverDireccion.y = 0;
            transform.position += moverDireccion * movimiento * Time.deltaTime;
            transform.forward = moverDireccion;
            animacion.SetBool("disparar", false);           // animación de correr/andar
        }
        else
        {
          // A distancia de disparo: quedarse plantado, mirar y disparar
          Vector3 dir = currentTarget.position - transform.position;
          dir.y = 0;
          transform.forward = dir.normalized;

          if (Time.time >= nextFireTime)
          {
            Debug.Log("Enemigo dispara al aliado");

            //Hacer daño real si el aliado tiene vida
            vidaMaxima vidaAliado = currentTarget.GetComponent<vidaMaxima>();
            if (vidaAliado != null)
                vidaAliado.TakeDamage(10);

            nextFireTime = Time.time + 1f / fireRate;
          }

          animacion.SetBool("disparar", true);          //Animación de disparo
        }

    }

}
