using System;
using Library;

namespace Playground
{
    class Program
    {
        static readonly string jsonPath = @"D:\Documents\Projetos\C#\C# - Playground\src\json.json";
        static void Main(string[] args)
        {
            // Using class on a Library project
            MyMath calculo = new MyMath();
            double value = calculo.Soma(20, 50);

            // Using struct
            MyStruct newStruct = new MyStruct("Teste");

            // Passagem de Parâmetro - Valor
            double number = 20;
            Console.WriteLine("Valor Inicial: " + number);
            AlterarValor(number);
            Console.WriteLine("Valor 'Alterado': " + number);

            // Passagem de Parâmetro - Referência
            Console.WriteLine("\nValor Inicial: " + number);
            RefAlterarValor(ref number);
            Console.WriteLine("Valor Alterado: " + number);

            // Passagem de Parâmetro - Out
            Console.WriteLine("\nValor Inicial: " + number);
            OutAlterarValor(out number);
            Console.WriteLine("Valor Alterado: " + number);

            // Passagem de Parâmetro - Params
            ManyEntries01("Mateus", "Marcos", "Lucas");
            string[] names = { "Mateus", "Marcos", "Lucas", "João" };
            ManyEntries02(names);
            ManyEntries03("Mateus", "Marcos", "Lucas", "João", "Evangelhos");

            // Testes JSON
            HandlerJSON jsonTest = new HandlerJSON();
            jsonTest.ExtractJsonData(jsonPath);

            // Throwing Personalized Exception
            try {
                throw new MyException("10");
            } catch(MyException ex) {
                Console.WriteLine(ex.Message);
            }

            // Showing project results
            Console.WriteLine("\nReference different project. Sum of values = " + value);
            Console.WriteLine("\nFirst use of structs. Struct name: " + newStruct.Name);

            // Desired output
            // "{ \"service_id\" : \"5c48a0ae585a6bf74393b81f\", \"month\" : 5 , \"count\" : 7083, \"month\" : 5 }"
            Console.WriteLine("");
            String input = "{{ \"id\" : {\"service_id\": ObjectId(\"5c48a0ae585a6bf74393b81f\"), \"month\": 5}, \"count\":7083, \"month\": 5}}";
            Console.WriteLine("Input String: " + input);
            String output = input.Replace("{ \"id\" : {", " ").Replace("ObjectId(", "").Replace(")", "").Replace("}}", "}").Replace("},", ",");
            Console.WriteLine("Output String: " + output);
            
            Console.ReadKey();
        }

        // Passagem de Parâmetro - Valor
        static void AlterarValor(double number)
        {
            Console.WriteLine("Alterar Valor: " + number);
            number += 10;
            Console.WriteLine("Valor Alterado: " + number);
        }

        // Passagem de Parâmetro - Referência
        static void RefAlterarValor(ref double number)
        {
            Console.WriteLine("Alterar Valor: " + number);
            number += 10;
            Console.WriteLine("Valor Alterado: " + number);
        }

        // Passagem de Parâmetro - Out -> Impossible to Read Value
        static void OutAlterarValor(out double number)
        {
            number = 10;
        }

        // Passagem de Parâmatro - Params
        // Using variables
        static void ManyEntries01(string name1, string name2, string name3) { }
        // Using Array
        static void ManyEntries02(string[] names) { }
        // Using Params
        static void ManyEntries03(params string[] names) { }
    }
}
