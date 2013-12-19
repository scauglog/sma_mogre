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
        protected static MOIS.Keyboard mEnvKeyboard;

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
        protected override void InitializeInput()
        {
            base.InitializeInput();

            int windowHandle;
            mRenderWindow.GetCustomAttribute("WINDOW", out windowHandle);
            mInputMgr = MOIS.InputManager.CreateInputSystem((uint)windowHandle);

            mEnvKeyboard = (MOIS.Keyboard)mInputMgr.CreateInputObject(MOIS.Type.OISKeyboard, true);
        }

        protected override bool OnFrameRenderingQueued(FrameEvent evt)
        {
            base.OnFrameRenderingQueued(evt);
            
            mEnvKeyboard.Capture();
            Vector3 spotlightMove = Vector3.ZERO;

            if (mEnvKeyboard.IsKeyDown(MOIS.KeyCode.KC_I))
                spotlightMove.z -= 100;

            if (mEnvKeyboard.IsKeyDown(MOIS.KeyCode.KC_K))
                spotlightMove.z += 100;

            if (mEnvKeyboard.IsKeyDown(MOIS.KeyCode.KC_J))
                spotlightMove.x -= 100;

            if (mEnvKeyboard.IsKeyDown(MOIS.KeyCode.KC_L))
                spotlightMove.x += 100;

            if (mEnvKeyboard.IsKeyDown(MOIS.KeyCode.KC_U))
                spotlightMove.y += 100;

            if (mEnvKeyboard.IsKeyDown(MOIS.KeyCode.KC_O))
                spotlightMove.y -= 100;

            if (spotlightMove != Vector3.ZERO)
                env.spotLight.Translate(spotlightMove * evt.timeSinceLastFrame);

            if (mEnvKeyboard.IsKeyDown(MOIS.KeyCode.KC_MINUS) && mEnvKeyboard.IsKeyDown(MOIS.KeyCode.KC_LCONTROL))
                env.removeNinja(ref mSceneMgr);

            if (mEnvKeyboard.IsKeyDown(MOIS.KeyCode.KC_ADD) && mEnvKeyboard.IsKeyDown(MOIS.KeyCode.KC_LCONTROL))
                env.addNinja(ref mSceneMgr);

            if (mEnvKeyboard.IsKeyDown(MOIS.KeyCode.KC_R))
                env.removeSpaceMarine(ref mSceneMgr);

            if (mEnvKeyboard.IsKeyDown(MOIS.KeyCode.KC_F))
                env.addSpaceMarine(ref mSceneMgr);

            

            
            return true;
        }

    }
}