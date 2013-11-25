using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.TutorialFramework;

namespace Mogre.Tutorials
{
    class SpaceMarine : Character
    {
        public SpaceMarine(ref SceneManager mSceneMgr, Vector3 position) 
        {
            ent = mSceneMgr.CreateEntity("Robot" + count.ToString(), "robot.mesh");
            ent.CastShadows = true;
            node = mSceneMgr.RootSceneNode.CreateChildSceneNode("RobotNode" + count.ToString());
            node.AttachObject(ent);
            name = count++.ToString();
            mWalkSpeed = 50.0f;
            mAnimationState = ent.GetAnimationState("Idle");
            mAnimationState.Loop = true;
            mAnimationState.Enabled = true;
            mWalkList = new LinkedList<Vector3>();
            mWalkList.AddLast(new Vector3(550.0f, 0.0f, 50.0f));
            mWalkList.AddLast(new Vector3(-100.0f, 0.0f, -200.0f));
            mWalkList.AddLast(new Vector3(0.0f, 0.0f, 25.0f));
            lastPosition = position;
        }
        protected override void destroy() { }
        protected bool nextLocation()
        {
            if (mWalkList.Count == 0)
                return false;
            return true;
        }
        public override void move(FrameEvent evt, Environment env)
        {
            if (!mWalking)
            //either we've not started walking or reached a way point
            {
                //check if there are places to go
                if (nextLocation())
                {
                    
                    LinkedListNode<Vector3> tmp;

                    //Start the walk animation
                    mAnimationState = ent.GetAnimationState("Walk");
                    mAnimationState.Loop = true;
                    mAnimationState.Enabled = true;
                    mWalking = true;

                    //Update the destination using the walklist.
                    mDestination = mWalkList.First.Value; //get the next destination.
                    tmp = mWalkList.First; //save the node that held it
                    Console.WriteLine(tmp.Value.x);
                    mWalkList.RemoveFirst(); //remove that node from the front of the list
                    mWalkList.AddLast(tmp);  //add it to the back of the list.

                    //update the direction and the distance
                    mDirection = mDestination - node.Position;
                    mDistance = mDirection.Normalise();

                }//if(nextLocation())
                else //nowhere to go. set the idle animation. (or Die)
                {
                    mAnimationState = ent.GetAnimationState("Idle");
                    //mAnimationState = mEntity.GetAnimationState("Die");
                    //mAnimationState.SetLoop(false);
                }
            }
            else //we're in motion
            {
               
                //determine how far to move this frame
                float move = mWalkSpeed * evt.timeSinceLastFrame;
                mDistance -= move;
                //Check to see if we've arrived at a waypoint
                if (mDistance <= 0.0f || env.outOfGround(node.Position))
                {
                    //set our node to the destination we've just reached & reset direction to 0
                    node.Position=lastPosition;
                    mDirection = Vector3.ZERO;
                    mWalking = false;

                }//if(mDistance <= 0.0f)
                else
                {
                    lastPosition = node.Position;
                    //Rotation code goes here
                    Vector3 src = node.Orientation * Vector3.UNIT_X;
                    if ((1.0f + src.DotProduct(mDirection)) < 0.0001f)
                    {
                        node.Yaw(180.0f);
                    }
                    else
                    {
                        Quaternion quat = src.GetRotationTo(mDirection);
                        node.Rotate(quat);
                    }
                    //movement code goes here
                    node.Translate(mDirection * move);
                }

            }
            //Update the Animation State.
            mAnimationState.AddTime(evt.timeSinceLastFrame * mWalkSpeed / 20);

        }
        


    }
}
