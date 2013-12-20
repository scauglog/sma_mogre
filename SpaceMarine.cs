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
            protected static Random rnd;
            protected static Vector3 returnPosition;
        public SpaceMarine(ref SceneManager mSceneMgr, Vector3 position) 
        {
            ent = mSceneMgr.CreateEntity("Robot" + count.ToString(), "robot.mesh");
            ent.CastShadows = true;
            node = mSceneMgr.RootSceneNode.CreateChildSceneNode("RobotNode" + count.ToString());
            node.AttachObject(ent);
            node.Position = position;
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
            returnPosition = Vector3.ZERO;
            state = "free";
            rnd = new Random();
        }
        protected override void destroy() { }
        protected bool nextLocation()
        {
            if (mWalkList.Count == 0)
                return false;
            return true;
        }

        private Stone findTarget(Environment env)
        {
            Stone stoneTarget = null;
            List<Character> listchar = env.lookCharacter(this);
            List<Stone> listStone = env.lookStone(this);
            double minDist=maxView;
            Vector3 position = Vector3.ZERO;

            if(state == "free"){
            foreach (Stone c in listStone)
                {
                    double dist = (c.Node.Position - this.Node.Position).Length;
                    //double distFromCastle = (c.Node.Position - castle).Length;
                    if (minDist > dist)// && distFromCastle > 100)
                    {
                        minDist = dist;
                        position = c.Node.Position;
                        stoneTarget = c;
                    }

                }

            
            }   
            //foreach (Character c in listchar)
            //{
            //    double dist = (c.Node.Position - this.Node.Position).Length;
            //    if (minDist > dist)
            //    {
            //        minDist = dist;
            //        position = c.Node.Position;
            //    }

            //}
             
            if (position!=Vector3.ZERO)
            {
                if(mWalkList.Count!=0)
                    mWalkList.RemoveFirst();
                mWalkList.AddFirst(position);
              
            }

            return stoneTarget;
        }

    //    public override void move(FrameEvent evt, Environment env)
    //    {
    //        if (!mWalking)
    //        {
    //            mAnimationState = ent.GetAnimationState("Walk");
    //            mAnimationState.Loop = true;
    //            mAnimationState.Enabled = true;
    //            mWalking = true;
    //            findTarget(env);
    //        }
    //        if (findTarget(env))
    //        {
    //            mDestination = mWalkList.First.Value;
    //            mDirection = mDestination - Node.Position;
    //            mDistance = mDirection.Normalise();
    //            float move = mWalkSpeed * evt.timeSinceLastFrame;
    //            mDistance -= move;

    //            if (mDistance <= 0.2f)
    //            {
    //                mAnimationState = ent.GetAnimationState("Shoot");
    //            }
    //            if (env.outOfGround(Node.Position))
    //            {

    //                //set our node to the destination we've just reached & reset direction to 0
    //                Node.Position = lastPosition;
    //                mDirection = Vector3.ZERO;
    //                mWalking = false;

    //            }
    //            else
    //            {
    //                lastPosition = Node.Position;
    //                //Rotation code goes here
    //                Vector3 src = Node.Orientation * forward;
    //                if ((1.0f + src.DotProduct(mDirection)) < 0.0001f)
    //                {
    //                    Node.Yaw(180.0f);
    //                }
    //                else
    //                {
    //                    Quaternion quat = src.GetRotationTo(mDirection);
    //                    Node.Rotate(quat);
    //                }
    //                //movement code goes here
    //                Node.Translate(mDirection * move);
    //            }
    //            //Update the Animation State.
    //            mAnimationState.AddTime(evt.timeSinceLastFrame * mWalkSpeed / 20);
    //        }
    //        else
    //        {
    //            Node.Position = lastPosition;
    //            mDirection = Vector3.ZERO;
    //            mWalking = false;
    //        }
    //    }
    //}
         public override void move(FrameEvent evt, Environment env)
        {
            Stone stoneTarget = null;
            if (state == "free")
            {
                stoneTarget = findTarget(env);
            }
            if (!mWalking)// && mAnimationState.HasEnded)
            {
                mAnimationState = ent.GetAnimationState("Walk");
                mAnimationState.Loop = true;
                mAnimationState.Enabled = true;
                mWalking = true;
            }
            if (stoneTarget != null)
            {
                //mWalking = false;

                mDestination = mWalkList.First.Value;
                mDirection = mDestination - Node.Position;
                mDistance = mDirection.Normalise();
                float move = mWalkSpeed * evt.timeSinceLastFrame;
                mDistance -= move;

                if (mDistance <= 0.2f)
                {
                    //mAnimationState = ent.GetAnimationState("Slump");
                    //mAnimationState.Enabled = true;
                    //mAnimationState.Loop = false;
                    
                    //mWalking = false;
                    stoneTarget.Node.Parent.RemoveChild(stoneTarget.Node);
                    node.AddChild(stoneTarget.Node);
                    stoneTarget.Node.Position = new Vector3(0, 100, 0);
                    mWalkList.RemoveFirst();
                    returnPosition = node.Position;
                    this.state = "stone";
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
            else if (state == "returnposition")
            {
                if(mWalkList.Count==0)
                    mWalkList.AddFirst(returnPosition);
                mDestination = mWalkList.First.Value;
                mDirection = mDestination - Node.Position;
                mDistance = mDirection.Normalise();
                float move = mWalkSpeed * evt.timeSinceLastFrame;
                mDistance -= move;

                if (mDistance <= 0.2f)
                {
                    
                    //mWalkList.RemoveFirst();
                    //mAnimationState = ent.GetAnimationState("Backflip");
                    //mAnimationState.Enabled = true;
                    //mAnimationState.Loop = false;
                    //mWalking = false;
                    mWalkList.RemoveFirst();
                    this.state = "free";
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
            else if (state == "stone")
            {
                if (mWalkList.Count == 0)
                {
                    float x = (float)rnd.NextDouble() + (float)rnd.Next(1000) - (float)rnd.Next(1000);
                    float z = (float)rnd.NextDouble() + (float)rnd.Next(1000) - (float)rnd.Next(1000);
                    mWalkList.AddFirst(new Vector3(x, 0, z));
                }

                mDestination = mWalkList.First.Value;
                mDirection = mDestination - Node.Position;
                mDistance = mDirection.Normalise();
                float move = mWalkSpeed * evt.timeSinceLastFrame;
                mDistance -= move;
                mWalking = true;

                if (mDistance <= 0.2f)
                {
                    mWalkList.RemoveFirst();
                    //mAnimationState = ent.GetAnimationState("Backflip");
                    //mAnimationState.Enabled = true;
                    //mAnimationState.Loop = false;
                    //mWalking = false;
                    Node temp = node.GetChild(0);
                    node.RemoveChild(0);
                    node.Parent.AddChild(temp);
                    temp.Position = node.Position;
                    this.state = "returnposition";
                }
                if (env.outOfGround(Node.Position))
                {

                    //set our node to the destination we've just reached & reset direction to 0
                    if(mWalkList.Count!=0)
                        mWalkList.RemoveFirst();
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
            else if (stoneTarget == null)
            {
                if (mWalkList.Count == 0)
                {
                    float x = (float)rnd.NextDouble() + (float)rnd.Next(1000) - (float)rnd.Next(1000);
                    float z = (float)rnd.NextDouble() + (float)rnd.Next(1000) - (float)rnd.Next(1000);
                    mWalkList.AddFirst(new Vector3(x, 0, z));
                }

                mDestination = mWalkList.First.Value;
                mDirection = mDestination - Node.Position;
                mDistance = mDirection.Normalise();
                float move = mWalkSpeed * evt.timeSinceLastFrame;
                mDistance -= move;
                mWalking = true;

                if (mDistance <= 0.2f)
                {
                    mWalkList.RemoveFirst();
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
        }
    }
}
