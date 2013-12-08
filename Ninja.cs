﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.TutorialFramework;

namespace Mogre.Tutorials
{

    class Ninja : Character
    {


        protected int ennemySpotted;
        protected Vector3 castle;

        public Ninja(ref SceneManager mSceneMgr, Vector3 position)
        {
            ent = mSceneMgr.CreateEntity("Ninja" + count.ToString(), "ninja.mesh");
            ent.CastShadows = true;
            node = mSceneMgr.RootSceneNode.CreateChildSceneNode("NinjaNode" + count.ToString());
            node.AttachObject(ent);
            node.Scale(0.5f, 0.5f, 0.5f);
            name = count++.ToString();
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
            mAnimationState = ent.GetAnimationState("Walk");
            mAnimationState.Loop = true;
            mAnimationState.Enabled = true;
            mWalking = true;

            castle = new Vector3(-600, 0, -600);
            state = "free";
            ennemySpotted = 0;
            maxView = 3000;
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
            List<Character> listChar = env.lookCharacter(this);
            List<Stone> listStone = env.lookStone(this);

            double minDist = maxView;
            Vector3 position = Vector3.ZERO;
            foreach (Stone c in listStone)
            {
                double dist = (c.Node.Position - this.Node.Position).Length;
                double distFromCastle = (c.Node.Position - castle).Length;
                if (minDist > dist && distFromCastle > 100)
                {
                    minDist = dist;
                    position = c.Node.Position;
                    stoneTarget = c;
                }

            }
            if (position != Vector3.ZERO)
            {
                if (mWalkList.Count != 0)
                    mWalkList.RemoveFirst();
                mWalkList.AddFirst(position);

            }

            return stoneTarget;
        }
        //private bool findEnnemy(Environment env)
        //{
        //    List<Character> listchar = env.lookCharacter(this);
        //    double minDist = maxView;
        //    Vector3 orientation = Vector3.ZERO;
        //    foreach (Character c in listchar)
        //    {
        //        if (c is SpaceMarine)
        //        {
        //            this.state = "alert";
        //            ennemySpotted++;
        //        }

        //        double dist = (c.Node.Position - this.Node.Position).Length;
        //        if (minDist > dist)
        //        {
        //            minDist = dist;
        //        }

        //    }
        //    if (orientation != Vector3.ZERO)
        //    {
        //        if (mWalkList.Count != 0)
        //            mWalkList.RemoveFirst();
        //        mWalkList.AddFirst(orientation);
        //        return true;
        //    }

        //    return false;
        //}
        //private bool findFriends(Environment env)
        //{
        //    List<Character> listchar = env.lookCharacter(this);
        //    double minDist = maxView;
        //    Vector3 position = Vector3.ZERO;
        //    foreach (Character c in listchar)
        //    {
        //        double dist = (c.Node.Position - this.Node.Position).Length;
        //        if (minDist > dist)
        //        {
        //            minDist = dist;
        //            position = c.Node.Position;
        //        }

        //    }
        //    if (position != Vector3.ZERO)
        //    {
        //        if (mWalkList.Count != 0)
        //            mWalkList.RemoveFirst();
        //        mWalkList.AddFirst(position);
        //        return true;
        //    }

        //    return false;
        //}
        public override void move(FrameEvent evt, Environment env)
        {
            Stone stoneTarget = null;
            if (state == "free")
            {
                stoneTarget = findTarget(env);
            }
            if (!mWalking && mAnimationState.HasEnded)
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
                    mAnimationState = ent.GetAnimationState("Backflip");
                    mAnimationState.Enabled = true;
                    mAnimationState.Loop = true;
                    
                    mWalking = false;
                    stoneTarget.Node.Parent.RemoveChild(stoneTarget.Node);
                    node.AddChild(stoneTarget.Node);
                    stoneTarget.Node.Position = new Vector3(0, 200, 0);
                    mWalkList.AddFirst(castle);
                    this.state = "return to castle";
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
            else if (state == "return to castle")
            {
                mDestination = mWalkList.First.Value;
                mDirection = mDestination - Node.Position;
                mDistance = mDirection.Normalise();
                float move = mWalkSpeed * evt.timeSinceLastFrame;
                mDistance -= move;

                if (mDistance <= 0.2f)
                {
                    mAnimationState = ent.GetAnimationState("Backflip");
                    mAnimationState.Enabled = true;
                    mAnimationState.Loop = true;
                    mWalking = false;
                    Node temp = node.GetChild(0);
                    node.RemoveChild(0);
                    node.Parent.AddChild(temp);
                    temp.Position = node.Position;
                    mWalkList.AddFirst(castle);
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

        }
    }
}
