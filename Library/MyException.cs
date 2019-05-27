using System;

namespace Library {
    public class MyException : Exception {
        string LinhaErro;
        public MyException(string Linha) : base("My Exception") {
            LinhaErro = Linha;
        }
    }
}