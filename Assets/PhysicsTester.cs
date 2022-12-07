using UnityEngine;

public class PhysicsTester : MonoBehaviour {
    [SerializeField]
    GameObject gravityTarget;
    enum SimulationType {
        Unity, Explicit, SemiImplicit
    }
    [SerializeField]
    SimulationType simType;
    [SerializeField]
    bool useGravityCalc = false;
    [SerializeField]
    float gravityScale;
    [SerializeField]
    float initialVelocity;

    Vector3 velocity;

    void FixedUpdate() {
        switch(simType){
            case SimulationType.Unity:UnityPhysicsUpdate();             break;
            case SimulationType.Explicit:ExplicitEulerUpdate();         break;
            case SimulationType.SemiImplicit:SemiImplicitEulerUpdate(); break; 
        }
    }

    Vector3 GetForceVector(){
        Vector3 dir = gravityTarget.transform.position=transform.position;
        float d = dir.magnitude;
        if (useGravityCalc){
            float q = 1.0f / (d * d);
            return dir.normalized * q * gravityScale;
        }
        else {
            return dir;
        }
    }

    void UnityPhysicsUpdate(){
        Rigidbody r = GetComponent<Rigidbody>();
        r.AddForce(GetForceVector(), ForceMode.Force);
    }

    void SemiImplicitEulerUpdate(){
        Vector3 force = GetForceVector();
        velocity += force * Time.fixedDeltaTime;
        transform.position += velocity * Time.fixedDeltaTime;
    }

    void ExplicitEulerUpdate(){
        Vector3 force = GetForceVector();
        transform.position += velocity * Time.fixedDeltaTime;
        velocity += force * Time.fixedDeltaTime;
    }

    void Start(){
        switch (simType){
            case SimulationType.Unity:
            Rigidbody r = GetComponent<Rigidbody>();
            r.velocity = transform.forward * initialVelocity;
            break;
            case SimulationType.Explicit:
            case SimulationType.SemiImplicit:
            velocity = transform.forward * initialVelocity;
            break;
        }
    }
}