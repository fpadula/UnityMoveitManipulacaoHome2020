/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */

using RosSharp.RosBridgeClient.MessageTypes.Std;
using RosSharp.RosBridgeClient.MessageTypes.Actionlib;

namespace RosSharp.RosBridgeClient.MessageTypes.Control
{
    public class FollowJointTrajectoryActionGoal : ActionGoal<FollowJointTrajectoryGoal>
    {
        public const string RosMessageName = "control_msgs/FollowJointTrajectoryActionGoal";

        public FollowJointTrajectoryActionGoal() : base()
        {
            this.goal = new FollowJointTrajectoryGoal();
        }

        public FollowJointTrajectoryActionGoal(Header header, GoalID goal_id, FollowJointTrajectoryGoal goal) : base(header, goal_id)
        {
            this.goal = goal;
        }
    }
}
