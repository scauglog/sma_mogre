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


        protected int ennemySpotted;
        protected Vector3 castle;
        protected static Random rnd;
        protected int nbStoneCastle;
        protected int stoneCastle;
        public Ninja(ref SceneManager mSceneMgr, Vector3 position)
        {
            ent = mSceneMgr.CreateEntity("Ninja" + count.ToString(), "ninja.mesh");
            ent.CastShadows = true;
            node = mSceneMgr.RootSceneNode.CreateChildSceneNode("NinjaNode" + count.ToString());
            node.AttachObject(ent);
            node.Scale(0.5f, 0.5f, 0.5f);
            node.Position = position;
            name = count++.ToString();
            walkSpeedFactor = 1.0f;
            mAnimationState = ent.GetAnimationState("Idle1");
            mAnimationState.Loop = true;
            mAnimationState.Enabled = true;
            mWalkList = new LinkedList<Vector3>();
            //mWalkList.AddLast(new Vector3(550.0f, 0.0f, 50.0f));
            //mWalkList.AddFirst(new Vector3(-100.0f, 0.0f, -200.0f));
            //mWalkList.AddLast(new Vector3(0.0f, 0.0f, 25.0f));
            forward = Vector3.NEGATIVE_UNIT_Z;
            viewingAngle = 40;
            mAnimationState = ent.GetAnimationState("Walk");
            mAnimationState.Loop = true;
            mAnimationState.Enabled = true;
            mWalking = true;
            rnd = new Random();
            stoneCastle = 0;
            castle = new Vector3();
            castle = Vector3.ZERO;
            nbStoneCastle = 0;
            state = "free";
            ennemySpotted = 0;
            maxView = 200;
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
                    carriedStoneName = c.Name;
                }

            }
            
            if (listStone.Count > stoneCastle && stoneTarget != null)
            {
                stoneCastle = listStone.Count;
                castle = stoneTarget.Node.Position;
                stoneTarget = null;
            }
            if (position != Vector3.ZERO && stoneTarget != null)
            {
                if (mWalkList.Count != 0)
                    mWalkList.RemoveFirst();
                mWalkList.AddFirst(position);

            }
            return stoneTarget;
        }
        
        public override void move(FrameEvent evt, Environment env)
        {
            Stone stoneTarget = null;
            if (state == "free")
            {
                stoneTarget = findTarget(env);
            }
            if (!mWalking && mAnimationState.HasEnded)
            {
                mAnimationState.Enabled = false;
                mAnimationState = ent.GetAnimationState("Walk");
                mAnimationState.Loop = true;
                mAnimationState.Enabled = true;
                mWalking = true;
            }
            #region has a stone target
            if (stoneTarget != null)
            {
                //mWalking = false;

                mDestination = mWalkList.First.Value;
                mDirection = mDestination - Node.Position;
                mDistance = mDirection.Normalise();
                float move = (MWalkSpeed*walkSpeedFactor) * evt.timeSinceLastFrame;
                mDistance -= move;

                if (mDistance <= 0.2f)
                {
                    mAnimationState = ent.GetAnimationState("Backflip");
                    mAnimationState.Enabled = true;
                    mAnimationState.Loop = false;
                    
                    mWalking = false;
                    stoneTarget.Node.Parent.RemoveChild(stoneTarget.Node);
                    node.AddChild(stoneTarget.Node);
                    stoneTarget.Node.Position = new Vector3(0, 200, 0);
                    //must set stone to unavailable
                    mWalkList.RemoveFirst();
                    mWalkList.AddFirst(castle);
                    this.state = "stone";
                    env.setCarriedStone(carriedStoneName);
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
                mAnimationState.AddTime(evt.timeSinceLastFrame * (MWalkSpeed*walkSpeedFactor) / 20);
            }
            #endregion
            #region carry a stone
            else if (state == "stone")
            {
                mDestination = mWalkList.First.Value;
                mDirection = mDestination - Node.Position;
                mDistance = mDirection.Normalise();
                float move = MWalkSpeed * evt.timeSinceLastFrame;
                mDistance -= move;

                if (mDistance <= 0.2f)
                {
                    mWalkList.RemoveFirst();
                    mAnimationState = ent.GetAnimationState("Backflip");
                    mAnimationState.Enabled = true;
                    mAnimationState.Loop = false;
                    mWalking = false;
                    try
                    {
                        Node temp = node.GetChild("stoneNode"+carriedStoneName);
                        //node.RemoveChild(0);
                        node.RemoveAllChildren();
                        node.Parent.AddChild(temp);
                        temp.Position = node.Position;
                        env.setUncarriedStone(carriedStoneName);
                    }
                    catch
                    {

                    }
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
                mAnimationState.AddTime(evt.timeSinceLastFrame * MWalkSpeed / 20);
            }
            #endregion
            #region no stone targeted
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
                float move = MWalkSpeed * evt.timeSinceLastFrame;
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
                mAnimationState.AddTime(evt.timeSinceLastFrame * MWalkSpeed / 20);
            }
            #endregion
        }
    }
}
