using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlCommentSerialization;

namespace XmlSrc
{
    public class DemoClass
    {
        public class Demo
        {
            public DemoAccount account;
            public DemoDebug debug;
        }        
        public class DemoAccount
        {
            [XmlComment("Hello world")]
            public string email = "hello@world.csharp";
            public string pseudo = "Enzo9741";
            public string hashPassword = "password";
        }
        public class DemoDebug
        {
            public string version = "V0.0.1";
        }
    }
    
}
