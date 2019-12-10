using UnityEngine;

public class ChaseAndEvade : MonoBehaviour
{
    public Transform player;
    public float reactionRadius = 10.0f;
    public bool chase;
    BoxCollider collider;
    bool enableCollider;

    float timer;
    float pauseTime = 1;

    void Start()
    {
        collider = GetComponent<BoxCollider>();
        enableCollider = true;
    }

    private void OnDisable()
    {
        enableCollider = false; 
    }

    void OnTriggerEnter(Collider other)
    {
        //if (enableCollider)
        //{
            if (other.gameObject.tag == "Obstacle")
            {
                //Debug.Break();
                print("obstacle");
            }
       // }        
    }

    public void SetTarget(Transform target_) => player = target_;

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        float dt = Time.deltaTime;
        timer -= dt;
        if (timer > 0)
        {
            //return exits out of the current method, so skips the rest of update
            return;
        }

        //player also inherits from gridMovement, so get its position               
        Vector3 playerPos = player.transform.position;
        //get this scripts position on grid
        Vector3 pos = transform.position;
        Vector3 movement = Vector3.zero;
        //float distance between this and player
        float d = Vector3.Distance(pos, playerPos);        
        //if distance is less than 5
        if(chase)
        {
            if (d > reactionRadius)
            {
                if (pos.x > playerPos.x)
                {
                    // To do: update movement.x properly
                    //movement.x--;
                    pos.x -= 0.3f;
                    //transform.position.x -= 1.0f;
                }
                else if (pos.x < playerPos.x)
                {
                    // To do: update movement.x properly      
                    pos.x += 0.3f;
                }

                if (pos.z > playerPos.z)
                {
                    // To do: update movement.y properly
                    pos.z -= 0.3f;
                }
                else if (pos.z < playerPos.z)
                {
                    // To do: update movement.y properly
                    pos.z += 0.3f;
                }
            }
            transform.position = pos;
        }
        else
        {
            if (d > reactionRadius)
            {
                if (pos.x > playerPos.x)
                {
                    // To do: update movement.x properly
                    //movement.x--;
                    pos.x += 0.3f;
                    //transform.position.x -= 1.0f;
                }
                else if (pos.x < playerPos.x)
                {
                    // To do: update movement.x properly      
                    pos.x -= 0.3f;
                }

                if (pos.z > playerPos.z)
                {
                    // To do: update movement.y properly
                    pos.z += 0.3f;
                }
                else if (pos.z < playerPos.z)
                {
                    // To do: update movement.y properly
                    pos.z -= 0.3f;
                }
            }
            transform.position = pos;
        }
        

        if (movement != Vector3.zero)
        {
            timer = pauseTime;
        }
    }
}
