using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.TutorialFramework;

namespace Mogre.Tutorials
{
    abstract class Character : BaseApplication
    {
        protected static int count=1;
        abstract protected void destroy();
        abstract protected void move();

    }
}
