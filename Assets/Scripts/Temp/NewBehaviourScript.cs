using System;

namespace SchoolEntrySystem
{
    // Interface for authentication method functionality
    public interface IAuthenticationMethod
    {
        void RegisterPerson(Person person);
        void Use();
        string AdditionalInfo { get; set; }
    }

    // Base class for all persons
    public abstract class Person
    {
        public string Name { get; private set; }
        public int ID { get; private set; }
        public IAuthenticationMethod AuthenticationMethod { get; private set; }

        protected Person(string name, int id, IAuthenticationMethod authMethod)
        {
            Name = name;
            ID = id;
            AuthenticationMethod = authMethod;
            AuthenticationMethod.RegisterPerson(this);
        }

        public void UseAuthenticationMethod()
        {
            AuthenticationMethod.Use();
        }

        public abstract string GetAdditionalInfo();
    }

    // Derived class for teachers
    public class Teacher : Person
    {
        public string Subject { get; private set; }

        public Teacher(string name, int id, string subject, IAuthenticationMethod authMethod) 
            : base(name, id, authMethod)
        {
            Subject = subject;
            authMethod.AdditionalInfo = GetAdditionalInfo();
        }

        public override string GetAdditionalInfo()
        {
            return $"Subject: {Subject}";
        }
    }

    // Derived class for students
    public class Student : Person
    {
        public string Grade { get; private set; }

        public Student(string name, int id, string grade, IAuthenticationMethod authMethod) 
            : base(name, id, authMethod)
        {
            Grade = grade;
            authMethod.AdditionalInfo = GetAdditionalInfo();
        }

        public override string GetAdditionalInfo()
        {
            return $"Grade: {Grade}";
        }
    }

    // Derived class for staff
    public class Staff : Person
    {
        public string Department { get; private set; }

        public Staff(string name, int id, string department, IAuthenticationMethod authMethod) 
            : base(name, id, authMethod)
        {
            Department = department;
            authMethod.AdditionalInfo = GetAdditionalInfo();
        }

        public override string GetAdditionalInfo()
        {
            return $"Department: {Department}";
        }
    }

    // Card class that registers and handles person information
    public class Card : IAuthenticationMethod
    {
        public Person RegisteredPerson { get; private set; }
        public string AdditionalInfo { get; set; }
        public DateTime BirthDate { get; private set; }

        public Card(DateTime birthDate)
        {
            BirthDate = birthDate;
        }

        public void RegisterPerson(Person person)
        {
            RegisteredPerson = person;
            Console.WriteLine($"Registered {person.Name} with ID {person.ID} and Birthdate {BirthDate.ToShortDateString()}.");
        }

        public void Use()
        {
            if (RegisteredPerson != null)
            {
                Console.WriteLine($"{RegisteredPerson.Name} with ID {RegisteredPerson.ID} has used the card.");
                if (!string.IsNullOrEmpty(AdditionalInfo))
                {
                    Console.WriteLine($"Additional Info: {AdditionalInfo}");
                }
            }
            else
            {
                Console.WriteLine("No person is registered to this card.");
            }
        }
    }

    // Phone class that registers and handles person information
    public class Phone : IAuthenticationMethod
    {
        public Person RegisteredPerson { get; private set; }
        public string AdditionalInfo { get; set; }
        public DateTime BirthDate { get; private set; }
        public string PhoneNumber { get; private set; }

        public Phone(DateTime birthDate, string phoneNumber)
        {
            BirthDate = birthDate;
            PhoneNumber = phoneNumber;
        }

        public void RegisterPerson(Person person)
        {
            RegisteredPerson = person;
            Console.WriteLine($"Registered {person.Name} with ID {person.ID}, Birthdate {BirthDate.ToShortDateString()}, and Phone Number {PhoneNumber}.");
        }

        public void Use()
        {
            if (RegisteredPerson != null)
            {
                Console.WriteLine($"{RegisteredPerson.Name} with ID {RegisteredPerson.ID} has used the phone.");
                if (!string.IsNullOrEmpty(AdditionalInfo))
                {
                    Console.WriteLine($"Additional Info: {AdditionalInfo}");
                }
            }
            else
            {
                Console.WriteLine("No person is registered to this phone.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create card and phone instances with required information
            IAuthenticationMethod teacherCard = new Card(new DateTime(1980, 5, 23));
            IAuthenticationMethod studentCard = new Card(new DateTime(2005, 8, 17));
            IAuthenticationMethod staffCard = new Card(new DateTime(1975, 3, 9));
            IAuthenticationMethod teacherPhone = new Phone(new DateTime(1982, 11, 30), "555-1234");

            // Create instances of each person type with card or phone injected
            var teacher = new Teacher("Alice", 1, "Math", teacherCard);
            var student = new Student("Bob", 2, "10th Grade", studentCard);
            var staff = new Staff("Charlie", 3, "Administration", staffCard);
            var teacherWithPhone = new Teacher("David", 4, "Science", teacherPhone);

            // Use the authentication methods
            teacher.UseAuthenticationMethod();
            student.UseAuthenticationMethod();
            staff.UseAuthenticationMethod();
            teacherWithPhone.UseAuthenticationMethod();
        }
    }
}
