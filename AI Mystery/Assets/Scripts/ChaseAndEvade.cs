using UnityEngine;

public class ChaseAndEvade : MonoBehaviour
{
    public Transform target = null;
    public bool chase = false;
    public float arrivalRadius = 1.0f;
    public float maxSpeed = 1.0f;

    public void SetTarget(Transform target_) => target = target_;

    void Update()
    {
        #region Chase & Evade Based On Distance
        //if (player == null) return;
        //float dt = Time.deltaTime;
        //timer -= dt;
        //if (timer > 0)
        //{
        //    //return exits out of the current method, so skips the rest of update
        //    return;
        //}

        ////player also inherits from gridMovement, so get its position               
        //Vector3 playerPos = player.transform.position;
        ////get this scripts position on grid
        //Vector3 pos = transform.position;
        //Vector3 movement = Vector3.zero;
        ////float distance between this and player
        //float d = Vector3.Distance(pos, playerPos);        
        ////if distance is less than 5
        //if(chase)
        //{
        //    if (d > reactionRadius)
        //    {
        //        if (pos.x > playerPos.x)
        //        {
        //            // To do: update movement.x properly
        //            //movement.x--;
        //            pos.x -= 0.3f;
        //            //transform.position.x -= 1.0f;
        //        }
        //        else if (pos.x < playerPos.x)
        //        {
        //            // To do: update movement.x properly      
        //            pos.x += 0.3f;
        //        }

        //        if (pos.z > playerPos.z)
        //        {
        //            // To do: update movement.y properly
        //            pos.z -= 0.3f;
        //        }
        //        else if (pos.z < playerPos.z)
        //        {
        //            // To do: update movement.y properly
        //            pos.z += 0.3f;
        //        }
        //    }
        //    transform.position = pos;
        //}
        //else
        //{
        //    if (d > reactionRadius)
        //    {
        //        if (pos.x > playerPos.x)
        //        {
        //            // To do: update movement.x properly
        //            //movement.x--;
        //            pos.x += 0.3f;
        //            //transform.position.x -= 1.0f;
        //        }
        //        else if (pos.x < playerPos.x)
        //        {
        //            // To do: update movement.x properly      
        //            pos.x -= 0.3f;
        //        }

        //        if (pos.z > playerPos.z)
        //        {
        //            // To do: update movement.y properly
        //            pos.z += 0.3f;
        //        }
        //        else if (pos.z < playerPos.z)
        //        {
        //            // To do: update movement.y properly
        //            pos.z -= 0.3f;
        //        }
        //    }
        //    transform.position = pos;
        //}

        //if (movement != Vector3.zero)
        //{
        //    timer = pauseTime;
        //}
        #endregion

        if (target == null) return;

        if (chase)
        {
            var desired = transform.position - target.position;

            if (desired.magnitude < arrivalRadius)
            {
                target.gameObject.GetComponent<PlayerController>().SetPlayerRole(true);
            }
            else
            {
                transform.position += -desired.normalized * maxSpeed * Time.deltaTime;
            }
        }
        else
        {
            var desired = target.position - transform.position;
            transform.position += desired.normalized * maxSpeed * Time.deltaTime;
        }
    }
}
