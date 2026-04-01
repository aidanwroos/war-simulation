// See https://aka.ms/new-console-template for more information
using System;

class Program
{

    public class Person
    {
        public int age;
        public string name;

        public Person(int age, string name)
        {
            this.age = age;
            this.name = name;
        }
    }
   
    




    static void Main(string[] args)
    {
        Person person1 = new Person(23, "Aidan");
        Console.WriteLine("Name is: " + person1.name + "\nAge is: " + person1.age);
    }
}



