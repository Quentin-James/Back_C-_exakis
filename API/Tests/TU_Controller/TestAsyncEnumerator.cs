// Classe interne pour simuler un énumérateur asynchrone
// Déclaration d'une classe interne qui représente un énumérateur asynchrone.
internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    // Un champ privé pour stocker un énumérateur synchronique de type T.
    private readonly IEnumerator<T> _inner;

    // Constructeur de la classe qui prend un énumérateur synchronique comme paramètre.
    public TestAsyncEnumerator(IEnumerator<T> inner)
    {
        _inner = inner; // Initialisation du champ _inner avec l'énumérateur passé en paramètre.
    }

    // Méthode qui libère les ressources utilisées par l'énumérateur.
    public ValueTask DisposeAsync()
    {
        _inner.Dispose(); // Appelle la méthode Dispose de l'énumérateur interne pour libérer les ressources.
        return ValueTask.CompletedTask; // Retourne une tâche terminée (aucune opération asynchrone ici).
    }

    // Méthode asynchrone pour avancer à l'élément suivant dans l'énumérateur.
    public ValueTask<bool> MoveNextAsync()
    {
        // Retourne un ValueTask avec le résultat de l'appel à MoveNext sur l'énumérateur interne.
        return new ValueTask<bool>(_inner.MoveNext());
    }

    // Propriété qui retourne l'élément actuel de l'énumérateur.
    public T Current => _inner.Current; // Renvoit la valeur actuelle de l'énumérateur interne.
}