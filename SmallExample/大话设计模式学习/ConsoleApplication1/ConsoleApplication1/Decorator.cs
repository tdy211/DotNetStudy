///装饰模式 大话设计模式
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{

    class Person
    {
        public Person()
        { }
        private string name;
        public Person(string name)
        {
            this.name = name;
        }
        public virtual void Show()
        {
            Console.WriteLine("装扮的{0}", name);
        }
    }
    class Finery : Person
    {
        protected Person component;
        public void Decorate(Person component)
        {
            this.component = component;
        }
        public override void Show()
        {
            if (component != null)
                component.Show();
        }

    }
    class TShirts : Finery
    {
        public override void Show()
        {
            Console.WriteLine("大T恤");
            base.Show();
        }
    
    }
    class BigTrouses : Finery
    {
        public override void Show()
        {
            Console.WriteLine("大裤衩");
            base.Show();
        }
    
    }
    class Decorator
    {/*
      //调用装饰模式
        static void main()
        {
            Person xiaoCai = new Person("小菜");
            BigTrouses bts = new BigTrouses();
            TShirts tsx = new TShirts();
            bts.Decorate(xiaoCai);//大裤头来装饰小菜童鞋
            tsx.Decorate(bts);
            tsx.Show();
      
        }
       */
    }
}
