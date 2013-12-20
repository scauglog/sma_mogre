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
        private bool isCarried;

        public bool IsCarried
        {
            get { return isCarried; }
            set { isCarried = value; }
        }
        public SceneNode Node
        {
            get { return node; }
        }
        public String Name
        {
            get { return name; }
        }


        public Stone(ref SceneManager mSceneMgr, Vector3 position)
        {
            name = count.ToString();
            ent = mSceneMgr.CreateEntity("stone"+name, "ogrehead.mesh");
            ent.CastShadows = true;
            isCarried = false;
            node = mSceneMgr.RootSceneNode.CreateChildSceneNode("stoneNode"+name);
            node.AttachObject(ent);
            node.Position = position;
            node.Scale(0.35f, 0.35f, 0.35f);
            count++;
        }

    }
}
