using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.TutorialFramework;

namespace Mogre.Tutorials
{
    abstract class Character
    {

        protected Vector3 lastPosition; 
        protected Entity ent;
        protected SceneNode node;
        protected String name;
        protected AnimationState mAnimationState = null; //The AnimationState the moving object
        protected float mDistance = 0.0f; 
        protected Vector3 mDirection = Vector3.ZERO;   // The direction the object is moving
        protected Vector3 mDestination = Vector3.ZERO; // The destination the object is moving towards
        protected Vector3 forward;
        protected float viewingAngle;
        protected LinkedList<Vector3> mWalkList = null; // A doubly linked containing the waypoints
        protected static float mWalkSpeed = 50.0f;  // The speed at which the object is moving 50=natural walk
        protected bool mWalking;
        protected int maxView;
        protected String state;
        protected float walkSpeedFactor;

        public static float MWalkSpeed
        {
            get { return Character.mWalkSpeed; }
            set { if(value>=0)
                    Character.mWalkSpeed = value;
                }
        }
        public SceneNode Node
        {
            get { return node; }
        }

        public Vector3 Forward
        {
            get { return forward; }
        }
        public float ViewingAngle
        {
            get { return viewingAngle; }
        }
        public String State
        {
            get { return state; }
        }
        

        protected static int count=1;
        abstract protected void destroy();
        abstract public void move(FrameEvent evt, Environment env);
        
        
    }
}
