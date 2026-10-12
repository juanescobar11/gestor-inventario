namespace GestorInventario.Domain.Exceptions;

public class DomainException(string mensaje) : Exception(mensaje);