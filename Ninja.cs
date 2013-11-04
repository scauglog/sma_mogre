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
        public Ninja(SceneManager mSceneMgr, Vector3 position)
        {
            Entity ent = mSceneMgr.CreateEntity("Ninja"+count.ToString(), "ninja.mesh");
            ent.CastShadows = true;
            SceneNode node = mSceneMgr.RootSceneNode.CreateChildSceneNode("NinjaNode");
            node.AttachObject(ent);
            node.Scale(0.5f, 0.5f, 0.5f);
            count++;
        }
        protected override void destroy() { }
        protected override void move() { }
    }
}
