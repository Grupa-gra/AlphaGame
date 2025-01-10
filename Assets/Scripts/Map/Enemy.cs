using System;
using UnityEngine;

namespace GG
{
    public class Enemy : MonoBehaviour
    {
        //public static Enemy Instance; // Dodajemy statyczn� referencj� do instancji klasy

        [SerializeField] private GameObject enemyMarker;
        [SerializeField] private Animator enemyAnimator;

        public int HealthPoints;
        public int DifficultyLevel;
        public String Name;

        private void Awake()
        {
            //// Je�li instancja ju� istnieje, niszczymy ten obiekt
            //if (Instance != null && Instance != this)
            //{
            //    Destroy(gameObject);
            //}
            //else
            //{
            //    // Inicjalizujemy Instance
            //    Instance = this;
            //}
        }

        public void EnemyDead()
        {
            enemyMarker.SetActive(false);
            enemyAnimator.SetTrigger("Dead");
        }
    }
}