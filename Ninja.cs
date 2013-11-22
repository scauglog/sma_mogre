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
        }
        protected override void destroy() { }
        protected override void move(FrameEvent evt) { }
    }
}
