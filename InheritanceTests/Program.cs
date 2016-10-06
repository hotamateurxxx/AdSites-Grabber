using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InheritanceTests
{

    class A
    {

        public A()
        {
        }

        public A(string param) 
            : this()
        {
        }

    }

    class B : A
    {

        public B(string param) 
            : base(param)
        {
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            A obj = new B("paramValue");
        }
    }

}
