using UnityEngine;

namespace RosSharp.RosBridgeClient.Actionlib
{
    [RequireComponent(typeof(RosConnector))]
    public class UnityKinovaGen3ActionSever : MonoBehaviour{
        // RosConnector
        private RosConnector rosConnector;
        // Classe que implementa o action-server
        private KinovaGen3ActionServer kinovagen3ActionServer;
        // Controlador do braço
        private ArmController ac;

        // Nome da ação
        public string actionName;
        // Status da ação
        public string status;     
        // Tempo máximo de espera entre um ponto da trajetória e outro
        public float timeout_ms;   

        private void Start(){
            // Pega referencia do RosConnector
            this.rosConnector = GetComponent<RosConnector>();
            // Pega referencia do ArmController
            this.ac = GetComponent<ArmController>();
            // Instancia o action-server
            this.kinovagen3ActionServer = new KinovaGen3ActionServer(actionName, rosConnector.RosSocket, new Log(x => Debug.Log(x)), this.ac, this.timeout_ms);
            // Inicia o servidor
            this.kinovagen3ActionServer.Initialize();
        }

        // A cada update publica o estado do action-server para o cliente
        // e exibe no editor o status
        private void Update(){
            this.kinovagen3ActionServer.PublishStatus();
            this.status = this.kinovagen3ActionServer.GetStatus().ToString();            
        }
    }

}