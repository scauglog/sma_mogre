using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.TutorialFramework;

namespace Mogre.Tutorials
{
    public class Stone
    {
        protected Entity ent;
        protected SceneNode node;
        protected String name;
        protected String state;
        protected static int count=1;
        public SceneNode Node
        {
            get { return node; }
        }


        public Stone(ref SceneManager mSceneMgr, Vector3 position)
        {
            name = count.ToString();
            ent = mSceneMgr.CreateEntity("OgreHead" + name, "ogrehead.mesh");
            ent.CastShadows = true;
            node = mSceneMgr.RootSceneNode.CreateChildSceneNode("OgreHeadNode" + name);
            node.AttachObject(ent);
            name = count.ToString();
            node.Position = position;
            node.Scale(0.35f, 0.35f, 0.35f);
            count++;
        }

    }
}
