/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */



namespace RosSharp.RosBridgeClient.MessageTypes.Control
{
    public class FollowJointTrajectoryAction : Action<FollowJointTrajectoryActionGoal, FollowJointTrajectoryActionResult, FollowJointTrajectoryActionFeedback, FollowJointTrajectoryGoal, FollowJointTrajectoryResult, FollowJointTrajectoryFeedback>
    {
        public const string RosMessageName = "control_msgs/FollowJointTrajectoryAction";

        public FollowJointTrajectoryAction() : base()
        {
            this.action_goal = new FollowJointTrajectoryActionGoal();
            this.action_result = new FollowJointTrajectoryActionResult();
            this.action_feedback = new FollowJointTrajectoryActionFeedback();
        }

    }
}
