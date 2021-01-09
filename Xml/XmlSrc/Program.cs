using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlSrc
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World");
            DemoClass.Demo demo = new DemoClass.Demo();
            DemoClass.DemoAccount account = new DemoClass.DemoAccount();
            DemoClass.DemoDebug debug = new DemoClass.DemoDebug();
            account.email = "email";
            account.pseudo = "pseudo";
            account.hashPassword = "password";
            debug.version = "version 1";
            demo.account = account;
            demo.debug = debug;
            Serializateur.Serializer(demo);
        }    
    }   
}
