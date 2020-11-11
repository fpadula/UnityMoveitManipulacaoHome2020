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
    public class FollowJointTrajectoryActionFeedback : ActionFeedback<FollowJointTrajectoryFeedback>
    {
        public const string RosMessageName = "control_msgs/FollowJointTrajectoryActionFeedback";

        public FollowJointTrajectoryActionFeedback() : base()
        {
            this.feedback = new FollowJointTrajectoryFeedback();
        }

        public FollowJointTrajectoryActionFeedback(Header header, GoalStatus status, FollowJointTrajectoryFeedback feedback) : base(header, status)
        {
            this.feedback = feedback;
        }
    }
}
