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
    public class FollowJointTrajectoryActionResult : ActionResult<FollowJointTrajectoryResult>
    {
        public const string RosMessageName = "control_msgs/FollowJointTrajectoryActionResult";

        public FollowJointTrajectoryActionResult() : base()
        {
            this.result = new FollowJointTrajectoryResult();
        }

        public FollowJointTrajectoryActionResult(Header header, GoalStatus status, FollowJointTrajectoryResult result) : base(header, status)
        {
            this.result = result;
        }
    }
}
