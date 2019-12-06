using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegate
{
    class Program
    {

        delegate void Feedback(int value);


        Feedback feedback;

        static void Main(string[] args)
        {
            Program program = new Program();
            program.feedback += OnFeedbackStatic;

            program.feedback += program.OnFeedback;

            program.feedback(1);
            System.Console.ReadKey();
        }

        private static void OnFeedbackStatic(int value)
        {
            Console.WriteLine("OnFeedbackStatic");
        }

        private void OnFeedback(int value)
        {
            Console.WriteLine("OnFeedback");
        }
    }
}
