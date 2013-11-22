using System;
 using System.Collections.Generic;
 using System.Text;
 using Mogre;
 using Mogre.TutorialFramework;


namespace Mogre.Tutorials
{

    public class MoveDemo : Tutorial
    {
        AnimationState mAnimationState = null; //The AnimationState the moving object
        float mDistance = 0.0f;              //The distance the object has left to travel
        Vector3 mDirection = Vector3.ZERO;   // The direction the object is moving
        Vector3 mDestination = Vector3.ZERO; // The destination the object is moving towards
        LinkedList<Vector3> mWalkList = null; // A doubly linked containing the waypoints
        float mWalkSpeed = 50.0f;  // The speed at which the object is moving
        bool mWalking;
        

        protected override void CreateScene()
        {
            // Create SceneManager
            mSceneMgr = mRoot.CreateSceneManager(SceneType.ST_EXTERIOR_CLOSE);
            Environment env = new Environment(ref mSceneMgr);
            ////Set ambient light
            //mSceneMgr.AmbientLight = ColourValue.White;

            // Create the Robot entity
            Entity ent = mSceneMgr.CreateEntity("Robot", "robot.mesh");

            //// Create the Robot's SceneNode
            SceneNode node = mSceneMgr.RootSceneNode.CreateChildSceneNode("RobotNode",
                             new Vector3(0.0f, 0.0f, 0.25f));
            node.AttachObject(ent);

            // Create knot objects so we can see movement
            //ent = mSceneMgr.CreateEntity("Knot1", "knot.mesh");
            //node = mSceneMgr.RootSceneNode.CreateChildSceneNode("Knot1Node",
            //    new Vector3(0.0f, -10.0f, 25.0f));
            //node.AttachObject(ent);
            //node.Scale(0.1f, 0.1f, 0.1f);
            ////
            //ent = mSceneMgr.CreateEntity("Knot2", "knot.mesh");
            //node = mSceneMgr.RootSceneNode.CreateChildSceneNode("Knot2Node",
            //    new Vector3(550.0f, -10.0f, 50.0f));
            //node.AttachObject(ent);
            //node.Scale(0.1f, 0.1f, 0.1f);
            ////
            //ent = mSceneMgr.CreateEntity("Knot3", "knot.mesh");
            //node = mSceneMgr.RootSceneNode.CreateChildSceneNode("Knot3Node",
            //    new Vector3(-100.0f, -10.0f, -200.0f));
            //node.AttachObject(ent);
            //node.Scale(0.1f, 0.1f, 0.1f);

            // Create the walking list
            mWalkList = new LinkedList<Vector3>();
            mWalkList.AddLast(new Vector3(550.0f, 0.0f, 50.0f));
            mWalkList.AddLast(new Vector3(-100.0f, 0.0f, -200.0f));
            mWalkList.AddLast(new Vector3(0.0f, 0.0f, 25.0f));

            // Set idle animation
            mAnimationState = mSceneMgr.GetEntity("Robot").GetAnimationState("Idle");
            mAnimationState.Loop = true;
            mAnimationState.Enabled = true;
        }

        protected override void CreateFrameListeners()
        {
            base.CreateFrameListeners();
            mRoot.FrameStarted += new FrameListener.FrameStartedHandler(FrameStarted);


        }

        protected bool nextLocation()
        {
            if (mWalkList.Count == 0)
                return false;
            return true;
        }

        bool FrameStarted(FrameEvent evt)
        {
            Entity mEntity = mSceneMgr.GetEntity("Robot");
            Node mNode = mSceneMgr.GetSceneNode("RobotNode");
            if (!mWalking)
            //either we've not started walking or reached a way point
            {
                //check if there are places to go
                if (nextLocation())
                {
                    LinkedListNode<Vector3> tmp;

                    //Start the walk animation
                    mAnimationState = mEntity.GetAnimationState("Walk");
                    mAnimationState.Loop = true;
                    mAnimationState.Enabled = true;
                    mWalking = true;

                    //Update the destination using the walklist.
                    mDestination = mWalkList.First.Value; //get the next destination.
                    tmp = mWalkList.First; //save the node that held it
                    mWalkList.RemoveFirst(); //remove that node from the front of the list
                    mWalkList.AddLast(tmp);  //add it to the back of the list.

                    //update the direction and the distance
                    mDirection = mDestination - mNode.Position;
                    mDistance = mDirection.Normalise();

                }//if(nextLocation())
                else //nowhere to go. set the idle animation. (or Die)
                {
                    mAnimationState = mEntity.GetAnimationState("Idle");
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
                    mNode.Position=mDestination;
                    mDirection = Vector3.ZERO;
                    mWalking = false;
                }//if(mDistance <= 0.0f)
                else
                {
                    //Rotation code goes here
                    Vector3 src = mNode.Orientation * Vector3.UNIT_X;
                    if ((1.0f + src.DotProduct(mDirection)) < 0.0001f)
                    {
                        mNode.Yaw(180.0f);
                    }
                    else
                    {
                        Quaternion quat = src.GetRotationTo(mDirection);
                        mNode.Rotate(quat);
                    }
                    //movement code goes here
                    mNode.Translate(mDirection * move);
                }

            }
            //Update the Animation State.
            mAnimationState.AddTime(evt.timeSinceLastFrame * mWalkSpeed / 20);


            return true;
        }

    }
}