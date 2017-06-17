using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFacConsole.SomeRegisterWay
{
    public class User
    {
        public User()
        {

        }
        public User(int _age)
        {
            this.age = age;
        }
        public User(int _age,string _name)
        {
            this.age = _age;
            this.name = _name;
        }
        public User(Phone _phone)
        {
            this.phone = _phone;
        }
        public User(Computer _computer)
        {
            this.computer = _computer;
        }
        public User(Phone _phone,Computer _computer)
        {
            this.phone = _phone;
            this.computer = _computer;
        }
        public Phone phone { get; set; }
        public Computer computer { get; set; }
        public int age { get; set; }
        public string name { get; set; }
        public override string ToString()
        {
            var str= "我叫" + this.name + "今年" + this.age + "岁";
            if (phone != null)
            {
                str += phone.ToString();
            }
            if (computer != null)
            {
                str += computer.ToString();
            }
            return str;
        }

        public void say(Computer com)
        {
            Console.WriteLine(com.ToString());
        }
    }
}
