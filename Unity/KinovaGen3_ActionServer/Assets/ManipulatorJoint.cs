using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulatorJoint : MonoBehaviour{
    // Flag que avisa para o script se o ângulo alvo da HingeJoint deve ser trocado
    public bool change_hj_target_angle;
    // Flag que avisa se a junta está na posição alvo
    public bool reached_target_angle;
    // Propriedades da junta (esforço, velocidade, ângulo etc)
    public float effort, velocity, angle;
    // ângulo alvo, erro angular e tolerância (para considerar que a junta chegou a posição alvo)
    public float target_angle, error, tol;
    // Variável que armazena referencia para a HingeJoint
    private HingeJoint hj;

    // Start é chamado antes do primeiro frame
    void Start(){
        this.hj = GetComponent<HingeJoint>();          
        this.change_hj_target_angle = false;
        this.error = 0;     
    }

    // FixedUpdate é chamado em cada passo da engine física
    void FixedUpdate(){
        // Extrai as informações pertinentes da junta
        this.angle = this.hj.angle;
        this.error = this.target_angle - this.angle;
        this.velocity = this.hj.velocity;
        this.effort = this.hj.currentTorque.magnitude;
        // Se o erro for menor que tol, então a junta chegou na posição alvo
        this.reached_target_angle = (Mathf.Abs(this.error) <= tol);   
    }

    // Update é chamado em cada passo da engine do unity
    void Update(){
        // Se change_hj_target_angle for true, então mude o ângulo alvo da HingeJoint
        if(this.change_hj_target_angle){
            this.change_hj_target_angle = false;
            // Como não podemos alterar diretamente a sprint da hinge joint, devemos
            // primeiro pegar uma referencia para essa spring, alterar seu ângulo alvo
            // e sobrescrever a spring presente na HingeJoint
            JointSpring curr_spring;            
            curr_spring = this.hj.spring;
            curr_spring.targetPosition = this.target_angle;
            this.hj.spring = curr_spring;            
        }
    }

    // Seta o ângulo da junta para um ângulo alvo. O motivo de não
    // fazermos essa alteração do ângulo alvo diretamente é devido
    // a uma proteção do Unity, que não permite que as variaveis da
    // engine sejam modificadas fora da thread principal
    public void Set_joint_angle(float angle){
        this.target_angle = angle;
        // Avisa ao script que queremos mudar o ângulo alvo da HingeJoint
        this.change_hj_target_angle = true;
    }

    // Retorna verdadeiro caso a junta tenha chegado à posição alvo
    public bool Joint_reached_target_position(){return this.reached_target_angle;}  
}
