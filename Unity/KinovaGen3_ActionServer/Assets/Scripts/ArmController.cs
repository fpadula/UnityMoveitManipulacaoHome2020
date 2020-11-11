using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour{
    // Nome das juntas
    public string[] joint_names;
    // Referencia ao script de controle das juntas
    public ManipulatorJoint[] manipulator_joints;
    // Angulo alvo das juntas (para testes)
    public double[] target_angles;
    // Troca o angulo alvo das ManipulatorJoints para o valor especificado em target_angles (para testes)
    public bool set_target_angles;
    // Flag que determina se as juntas chegaram na posição alvo
    public bool joints_reached_target_position;
    // Dicionario em que a chave é o nome da junta e o valor a referencia ao script de controle
    // da junta
    public Dictionary<string, ManipulatorJoint> joints_dict;

    void Start(){
        // Cria o dicionário e armazena os pares <nome da joint, referencia para a joint>
        this.joints_dict = new Dictionary<string, ManipulatorJoint>();
        for (int i = 0; i < joint_names.Length; i++){
            this.joints_dict.Add(this.joint_names[i], this.manipulator_joints[i]);
        }
        this.joints_reached_target_position = false;
        this.set_target_angles = true;
    }

    // Checa se todas as juntas alcançaram a posição alvo
    void FixedUpdate(){
        // Começa com uma variável que determina se as juntas alcançaram a posição alvo
        // setada para true. Se apenas 1 das juntas não estiver na posição alvo, a operação
        // booleana irá retornar falso
        bool all_joints_set;
        all_joints_set = true;
        foreach (KeyValuePair<string, ManipulatorJoint> kvp in this.joints_dict){
            all_joints_set = all_joints_set && kvp.Value.Joint_reached_target_position();
        }
        this.joints_reached_target_position = all_joints_set;
    }

    // Seta o angulo de uma determinada junta baseada no seu nome. Recebemos um array double
    // pois as funções que usaremos no ROS usam, em sua maioria, estrutura do tipo double.
    // Tratar isso aqui, internamente, deixa o código mais limpo.
    public void Set_target_angles(string[] joint_names, double[] target_angles, bool in_rad){
        string name;
        float target_angle;        
        for (int i = 0; i < joint_names.Length; i++){
            name = joint_names[i];
            target_angle = (float)target_angles[i];
            // Se angulo estiver em radianos, converta:
            if(in_rad)
                target_angle = Mathf.Rad2Deg * target_angle;
            this.joints_dict[name].Set_joint_angle(target_angle);
        }
        this.joints_reached_target_position = false;
    }

    // Se a flag set_target_angles for verdadeira, sobrescreve o angulo alvo das juntas (para fins de teste)
    void Update(){
        if (this.set_target_angles){
            this.set_target_angles = false;
            this.Set_target_angles(this.joint_names, this.target_angles, false);
        }
    }

    public float GetJointAngle(string joint_name){
        return Mathf.Deg2Rad * this.joints_dict[joint_name].angle;
    }

    public float GetJointVelocity(string joint_name){
        return Mathf.Deg2Rad * this.joints_dict[joint_name].velocity;
    }

    public float GetJointEffort(string joint_name){
        return this.joints_dict[joint_name].effort;
    }
}
