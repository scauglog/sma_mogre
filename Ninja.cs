using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.TutorialFramework;

namespace Mogre.Tutorials
{
       
    class Ninja : Character
    {  
        public Ninja(ref SceneManager mSceneMgr, Vector3 position)
        {
            ent = mSceneMgr.CreateEntity("Ninja"+count.ToString(), "ninja.mesh");
            ent.CastShadows = true;
            node = mSceneMgr.RootSceneNode.CreateChildSceneNode("NinjaNode" + count.ToString());
            node.AttachObject(ent);
            node.Scale(0.5f, 0.5f, 0.5f);
            name=count++.ToString();
            mWalkSpeed = 50.0f;
            mAnimationState = ent.GetAnimationState("Idle1");
            mAnimationState.Loop = true;
            mAnimationState.Enabled = true;
            mWalkList = new LinkedList<Vector3>();
            mWalkList.AddLast(new Vector3(550.0f, 0.0f, 50.0f));
            mWalkList.AddFirst(new Vector3(-100.0f, 0.0f, -200.0f));
            mWalkList.AddLast(new Vector3(0.0f, 0.0f, 25.0f));
            forward = Vector3.NEGATIVE_UNIT_Z;
            viewingAngle = 40;
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
                    mWalkList.RemoveFirst(); //remove that node from the front of the list
                    mWalkList.AddLast(tmp);  //add it to the back of the list.

                    //update the direction and the distance
                    mDirection = mDestination - Node.Position;
                    mDistance = mDirection.Normalise();

                }//if(nextLocation())
                else //nowhere to go. set the idle animation. (or Die)
                {
                    mAnimationState = ent.GetAnimationState("Idle1");
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
                if (mDistance <= 0.0f)
                {
                    //set our node to the destination we've just reached & reset direction to 0
                    Node.Position=mDestination;
                    mDirection = Vector3.ZERO;
                    mWalking = false;
                }//if(mDistance <= 0.0f)
                else
                {
                    //Rotation code goes here
                    Vector3 src = Node.Orientation * forward;
                    if ((1.0f + src.DotProduct(mDirection)) < 0.0001f)
                    {
                        Node.Yaw(180.0f);
                    }
                    else
                    {
                        Quaternion quat = src.GetRotationTo(mDirection);
                        Node.Rotate(quat);
                    }
                    //movement code goes here
                    Node.Translate(mDirection * move);
                }

            }
            //Update the Animation State.
            mAnimationState.AddTime(evt.timeSinceLastFrame * mWalkSpeed / 20);
        }
        
    }
}
