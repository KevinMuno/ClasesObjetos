// Este programa muestra el uso de metodos de extension
using Modelo;

int numero = 55;
bool par = numero.EsPar(); //Aqui encontramos la extension
Console.WriteLine("{0} es {1}", numero, par);

// Extension del double
double valor = 55.18;

Console.WriteLine(valor.Duplica());

