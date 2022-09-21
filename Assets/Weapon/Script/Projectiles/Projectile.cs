using UnityEngine;

namespace Item.Weapon
{
    public class Projectile : MonoBehaviour
    {
        [HideInInspector]
        public Gun gunUsingThis;
        public bool isUseGravity;
        [SerializeField]
        private float gravityDownForce;
        public float moveSpeed;
        protected Vector3 velocity;
        protected float velocityY;

        protected virtual void Awake()
        {
            velocity = transform.forward * moveSpeed;
            velocityY = velocity.y;
        }

        void Update()
        {
            Movement();
        }

        protected void Movement()
        {
            if (isUseGravity)
            {
                velocityY -= gravityDownForce * Time.deltaTime;
                velocity.y = velocityY;
            }
            transform.position = transform.position + velocity * Time.deltaTime;
        }
    }
}