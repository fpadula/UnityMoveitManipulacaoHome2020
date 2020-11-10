using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RosSharp.RosBridgeClient{
    // Classe extende o classe UnityPublisher com mensagem do tipo JointState
    public class ArmJointStatePublisher : UnityPublisher<MessageTypes.Sensor.JointState>{
        // ID do frame
        public string FrameId = "Unity";
        // Mensagem que será enviada ao ROS
        private MessageTypes.Sensor.JointState message;
        // Referencia do controlador do braço
        private ArmController ac;

        // Inicializa mensagem e armazena referência do controlador do braço
        protected override void Start(){
            this.ac = this.transform.GetComponent<ArmController>();
            base.Start();
            InitializeMessage();
        }

        // Inicializa mensagem do tipo JointState (mais informações em
        // http://docs.ros.org/en/melodic/api/sensor_msgs/html/msg/JointState.html)
        private void InitializeMessage(){
            int jointStateLength = this.ac.joint_names.Length;
            this.message = new MessageTypes.Sensor.JointState{
                header = new MessageTypes.Std.Header { frame_id = FrameId },
                name = new string[jointStateLength],
                position = new double[jointStateLength],
                velocity = new double[jointStateLength],
                effort = new double[jointStateLength]
            };
        }

        // Lê as informações das juntas e publica essas informações no tópico especificado
        void FixedUpdate(){
            this.message.header.Update();
            int i = 0;
            foreach (KeyValuePair<string, ManipulatorJoint> kvp in this.ac.joints_dict){
                this.message.name[i] = kvp.Key;
                this.message.position[i] = this.ac.GetJointAngle(kvp.Key);
                this.message.velocity[i] = this.ac.GetJointVelocity(kvp.Key);
                this.message.effort[i] = this.ac.GetJointEffort(kvp.Key);
                i++;
            }
            Publish(this.message);
        }

    }
}
