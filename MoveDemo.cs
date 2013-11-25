using System;
 using System.Collections.Generic;
 using System.Text;
 using Mogre;
 using Mogre.TutorialFramework;


namespace Mogre.Tutorials
{

    public class MoveDemo : Tutorial
    {
        
        Environment env;

        protected override void CreateScene()
        {
            // Create SceneManager
            mSceneMgr = mRoot.CreateSceneManager(SceneType.ST_EXTERIOR_CLOSE);
            env = new Environment(ref mSceneMgr);
            

        }

        protected override void CreateFrameListeners()
        {
            base.CreateFrameListeners();
            mRoot.FrameStarted += new FrameListener.FrameStartedHandler(FrameStarted);


        }

        bool FrameStarted(FrameEvent evt)
        {
            env.moveCharacters(evt);
            return true;
            
        }

    }
}