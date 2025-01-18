using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BulletScript : MonoBehaviour
{
    private ConeRaycaster shooter;
    private Rigidbody rb;

    void Start(){
        rb = GetComponent<Rigidbody>();
        if(rb == null){
            Debug.Log("RBody not attached to bullet");
        }
    }

    public void SetShooter(ConeRaycaster shooter){
        this.shooter = shooter;
    }

    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.layer == LayerMask.NameToLayer("Wall")){
            if (rb != null){
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
            }

            //do stuff after the wall hit
            
        }else{

            DOVirtual.DelayedCall(3f, () => timeOut());
        }

    }

    private void timeOut(){
        if(shooter != null){
            shooter.ResetShoot();
        }
        Destroy(gameObject);
    }
}
