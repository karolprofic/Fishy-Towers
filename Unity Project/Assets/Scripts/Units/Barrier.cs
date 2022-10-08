using UnityEngine;
using Battle;
using UnityEngine.Events;

namespace Units
{
    public class Barrier : MonoBehaviour 
    {
        [SerializeField] private Collider2D collider;
        public UnityEvent onEnemyCollide;
        bool once=true;
        private void OnTriggerEnter2D(Collider2D other) {
            if(other.CompareTag(Bullet.Tag))
            {
                var bullet = other.GetComponent<Bullet>();
                bullet.GoToStorage();
                return;
            }
            if(other.CompareTag(Unit.EnemyTag))
            {
                var enemy = other.GetComponent<Unit>();
                enemy.GoToStorage();
                if(once)
                    onEnemyCollide.Invoke();
                once = false;
                return;
            }
            if(other.CompareTag(Unit.AllyTag))
            {
                return;
            }

        }
    }

}