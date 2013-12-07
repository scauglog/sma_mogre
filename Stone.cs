﻿using System;
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
        protected static int count=2;
        public SceneNode Node
        {
            get { return node; }
        }



        public Stone(ref SceneManager mSceneMgr, Vector3 position)
        {
            ent = mSceneMgr.CreateEntity("OgreHead" + count.ToString(), "ogrehead.mesh");
            ent.CastShadows = true;
            node = mSceneMgr.RootSceneNode.CreateChildSceneNode("OgreHeadNode" + count.ToString());
            node.AttachObject(ent);
            name = count++.ToString();
        }

    }
}