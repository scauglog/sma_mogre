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


        protected Entity ent;
        protected SceneNode node;
        protected string name;
        protected AnimationState mAnimationState = null; //The AnimationState the moving object
        protected float mDistance = 0.0f; 
        protected Vector3 mDirection = Vector3.ZERO;   // The direction the object is moving
        protected Vector3 mDestination = Vector3.ZERO; // The destination the object is moving towards
        protected LinkedList<Vector3> mWalkList = null; // A doubly linked containing the waypoints
        protected float mWalkSpeed = 50.0f;  // The speed at which the object is moving
        protected bool mWalking;


        protected static int count=1;
        abstract protected void destroy();
        abstract public void move(FrameEvent evt, Environment env);

    }
}
