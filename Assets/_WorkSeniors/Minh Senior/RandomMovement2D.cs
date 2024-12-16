using UnityEngine;

public class RandomMovement2D : MonoBehaviour
{
    public float speed = 2.0f;        
    public float changeDirectionTime = 2.0f; 

    private Vector2 direction;        
    private float timer;              

    void Start()
    {
        ChangeDirection();            
    }

    void Update()
    {
        timer += Time.deltaTime;      

        if (timer >= changeDirectionTime)
        {
            ChangeDirection();        
            timer = 0;                
        }

        transform.Translate(direction * speed * Time.deltaTime); 
    }

    void ChangeDirection()
    {
        
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
