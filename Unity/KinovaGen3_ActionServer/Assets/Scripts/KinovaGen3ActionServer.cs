using System;
using System.Threading;
using System.Collections.Generic;

using RosSharp.RosBridgeClient.MessageTypes.ActionlibTutorials;
using RosSharp.RosBridgeClient.MessageTypes.Actionlib;
using RosSharp.RosBridgeClient.MessageTypes.Control;

namespace RosSharp.RosBridgeClient.Actionlib{
    public class KinovaGen3ActionServer : ActionServer<FollowJointTrajectoryAction, FollowJointTrajectoryActionGoal, FollowJointTrajectoryActionResult, FollowJointTrajectoryActionFeedback, FollowJointTrajectoryGoal, FollowJointTrajectoryResult, FollowJointTrajectoryFeedback>{
        private ManualResetEvent isProcessingGoal = new ManualResetEvent(false);
        private Thread goalHandler;
        private ArmController ac;
        private float timeout;

        public KinovaGen3ActionServer(string actionName, RosSocket rosSocket, Log log, ArmController ac, float timeout){
            // Nome da ação
            this.actionName = actionName;
            // RosSocket
            this.rosSocket = rosSocket;
            // Interface de log
            this.log = log;
            // Controlador do braço
            this.ac = ac;
            // Armazena quanto tempo deve-se esperar entre dois pontos de uma trajetória antes de abortar
            this.timeout = timeout;

            // Variável que armazena a ação em si (como visto em http://docs.ros.org/en/electric/api/control_msgs/html/msg/FollowJointTrajectoryAction.html)
            this.action = new FollowJointTrajectoryAction();
        }

        // Verifica se uma meta é valida
        protected bool IsGoalValid(){
            // Não verificamos neste tutorial
            return true;
        }

        // Primeira etapa da máquina de estados (como visto na seção 2.1.1 de http://wiki.ros.org/actionlib/DetailedDescription)
        protected override void OnGoalReceived(){
            // Se valida a meta é aceita
            if (IsGoalValid()){
                SetAccepted("KinovaGen3 Action Server: The goal has been accepted");
            }
            // Caso contrário é rejeitada
            else{
                SetRejected("KinovaGen3 Action Server: Cannot perform goal");
            }
        }

        // Não fazemos nada nesse caso, apenas exibimos uma mensagem
        protected override void OnGoalRejected(){
            log("KinovaGen3 Action Server: Cannot perform goal");
        }

        // Se aceita, iniciamos uma thread que irá gerenciar nossa meta.
        // Note que a máquina de estado está no estado ACTIVE, e irá permanecer
        // assim até que a thread mude seu estado.
        protected override void OnGoalActive(){
            this.goalHandler = new Thread(ExecuteFollowJointTrajectoryGoal);
            this.goalHandler.Start();
        }

        // Rotina que executará a meta de fato.
        private void ExecuteFollowJointTrajectoryGoal(){
            // Variável que armazena o tempo atual de espera
            float curr_wait_time;
            // Seta o sinal de isProcessingGoal para true
            isProcessingGoal.Set();

            // Exibindo a quantidade de pontos da trajetória
            log("Trajectory of size (" + this.action.action_goal.goal.trajectory.points.Length  + ") received. Executing...");

            // Itera sobre todos os pontos da trajetória
            for (int i = 0; i < this.action.action_goal.goal.trajectory.points.Length; i++){

                // Se o sinal de isProcessingGoal é false, a ação deve ser abortada.
                // Note que o parâmetro 0 indica que não há espera ao checar o sinal.
                if (!isProcessingGoal.WaitOne(0)){
                    // Setamos o status do resultado para PREEMPTED
                    this.action.action_result.status.status = GoalStatus.PREEMPTED;
                    // Exibimos uma mensagem para o usuário
                    log("Canceled.");
                    // Vai para estado PREEMPED (ao chamar SetCanceled)
                    SetCanceled();
                    return;
                }
                
                // Dado o nome das juntas da trajetória, seta as juntas para a posição i da trajetória
                this.ac.Set_target_angles(this.action.action_goal.goal.trajectory.joint_names, this.action.action_goal.goal.trajectory.points[i].positions, true);
                // Inicializa o tempo de espera como 0
                curr_wait_time = 0;
                // Enquanto as juntas não chegarem no alvo, executa o loop
                while(!this.ac.joints_reached_target_position){
                    // Se o tempo de espera for maior que timeout, aborda a execução
                    if(curr_wait_time > this.timeout){
                        // Seta o status do resultado para ABORTED
                        this.action.action_result.status.status = GoalStatus.ABORTED;
                        // Exibimos uma mensagem para o usuário
                        log("Aboted.");
                        // Vai para estado ABORTED
                        SetAborted();
                        return;
                    }
                    // Espera 1ms
                    Thread.Sleep(1);
                    // Incrementa o contador em 1 (representa 1ms)
                    curr_wait_time+=1;
                }
                // Publica informação de feedback
                PublishFeedback();
            }
            // Terminamos o loop, ou seja, percorremos todos os pontos da trajetória
            log("Finished...");
            // Seta o status do resultado para SUCCEEDED
            this.action.action_result.status.status = GoalStatus.SUCCEEDED;            
            // Vai para estado SUCCEEDED
            SetSucceeded();
        }


        // Não fazemos nada em caso de Recalling
        protected override void OnGoalRecalling(GoalID goalID){
            // Left blank for this example
        }

        // O cliente quer cancelar a ação
        protected override void OnGoalPreempting(){
            // Em caso de preempção, setamos o sinal de isProcessingGoal para false.
            // goalHandler cuidará do sinal de retorno, só devemos esperar seu termino
            isProcessingGoal.Reset();
            // Esperando goalHandler terminar de executar
            this.goalHandler.Join();
        }

        // Essa função irá setar o estado interno da classe para SUCCEEDED, 
        // além de publicar o resultado para o cliente
        protected override void OnGoalSucceeded(){            
            UpdateAndPublishStatus(ActionStatus.SUCCEEDED);
        }

        // Publica o resultado para o cliente
        protected override void OnGoalAborted(){
            PublishResult();
        }

        // Publica o resultado para o cliente
        protected override void OnGoalCanceled(){
            PublishResult();
        }
    }
}
