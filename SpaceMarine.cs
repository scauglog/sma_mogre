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
            node.Scale(0.5f, 0.5f, 0.5f);
            name = count++.ToString();
            mWalkSpeed = 50.0f;
        }
        protected override void destroy() { }
        protected override void move(FrameEvent evt)
        {
            
        }
    }
}
