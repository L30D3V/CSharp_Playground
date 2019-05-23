using System;

namespace Library {
    public class UsefulCommands {
        
        // Comando utilizado para realizar um loop de forma paralela
        // Cada iteração funciona em um Thread diferente
        // Útil para download de múltiplos arquivos
        public void ParallelForCommand() {
            int[] numeros = new int[] { 1, 2, 3, 4 };
            System.Threading.Tasks.Parallel.For(0, numeros.Length, index => {
                Console.WriteLine(numeros[index]);
            });
        }
    }
}