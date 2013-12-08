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
            lastPosition = position;
            forward = Vector3.UNIT_X;
            viewingAngle = 0.9f;
            maxView = 3000;
            
        }
        protected override void destroy() { }
        protected bool nextLocation()
        {
            if (mWalkList.Count == 0)
                return false;
            return true;
        }

        private bool findTarget(Environment env)
        {
            List<Character> listchar = env.lookCharacter(this);
            double minDist=maxView;
            Vector3 position = Vector3.ZERO;
            foreach (Character c in listchar)
            {
                double dist = (c.Node.Position - this.Node.Position).Length;
                if (minDist > dist)
                {
                    minDist = dist;
                    position = c.Node.Position;
                }

            }
            if (position!=Vector3.ZERO)
            {
                if(mWalkList.Count!=0)
                    mWalkList.RemoveFirst();
                mWalkList.AddFirst(position);
                return true;
            }

            return false;
        }

        public override void move(FrameEvent evt, Environment env)
        {
            if (!mWalking)
            {
                mAnimationState = ent.GetAnimationState("Walk");
                mAnimationState.Loop = true;
                mAnimationState.Enabled = true;
                mWalking = true;
                findTarget(env);
            }
            if (findTarget(env))
            {
                Console.Write("test");
                mDestination = mWalkList.First.Value;
                mDirection = mDestination - Node.Position;
                mDistance = mDirection.Normalise();
                float move = mWalkSpeed * evt.timeSinceLastFrame;
                mDistance -= move;

                if (mDistance <= 0.2f)
                {
                    mAnimationState = ent.GetAnimationState("Shoot");
                }
                if (env.outOfGround(Node.Position))
                {

                    //set our node to the destination we've just reached & reset direction to 0
                    Node.Position = lastPosition;
                    mDirection = Vector3.ZERO;
                    mWalking = false;

                }
                else
                {
                    lastPosition = Node.Position;
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
                //Update the Animation State.
                mAnimationState.AddTime(evt.timeSinceLastFrame * mWalkSpeed / 20);
            }
            else
            {
                Node.Position = lastPosition;
                mDirection = Vector3.ZERO;
                mWalking = false;
            }
        }
    }
}
