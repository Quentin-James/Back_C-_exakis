using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXERCICE_INTEGRATION.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using EXERCICE_INTEGRATION.Controllers;

namespace MoqTestTu_Controller
{
    public class MockDbContextTest
    {
        // Test de théorie pour vérifier que la méthode Delete retourne le résultat attendu
        [Theory]
        [InlineData(1, true)]  // Cas où l'étudiant existe
        [InlineData(2, false)] // Cas où l'étudiant n'existe pas
        public async Task Delete_ReturnsExpectedResult(int studentId, bool studentExists)
        {
            // Arrange
            var mockSet = new Mock<DbSet<Student>>(); // Création d'un mock de DbSet<Student>
            var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>()); // Création d'un mock de AppDbContext

            if (studentExists)
            {
                // Configuration du mock pour retourner un étudiant lorsque FindAsync est appelé avec l'ID de l'étudiant
                mockSet.Setup(m => m.FindAsync(studentId)).ReturnsAsync(new Student { Id = studentId, FirstName = "John", LastName = "Doe" });
            }
            else
            {
                // Configuration du mock pour retourner null lorsque FindAsync est appelé avec l'ID de l'étudiant
                mockSet.Setup(m => m.FindAsync(studentId)).ReturnsAsync((Student?)null);
            }

            // Configuration du mock pour retourner le mock de DbSet<Student> pour la propriété Students
            mockContext.Setup(c => c.Students).Returns(mockSet.Object);

            // Création d'une instance du contrôleur StudentController en utilisant le contexte mocké
            var controller = new StudentController(mockContext.Object);

            // Act
            // Appel de la méthode Delete du contrôleur avec l'ID de l'étudiant
            var result = await controller.Delete(studentId);

            // Assert
            if (studentExists)
            {
                // Vérification que le résultat est de type NoContentResult
                Assert.IsType<NoContentResult>(result);
                // Vérification que la méthode Remove du mock de DbSet<Student> a été appelée une fois avec l'étudiant
                mockSet.Verify(m => m.Remove(It.Is<Student>(s => s.Id == studentId)), Times.Once);
                // Vérification que la méthode SaveChangesAsync du mock de AppDbContext a été appelée une fois
                mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
            }
            else
            {
                // Vérification que le résultat est de type NotFoundResult
                Assert.IsType<NotFoundResult>(result);
                // Vérification que la méthode Remove du mock de DbSet<Student> n'a jamais été appelée
                mockSet.Verify(m => m.Remove(It.IsAny<Student>()), Times.Never);
                // Vérification que la méthode SaveChangesAsync du mock de AppDbContext n'a jamais été appelée
                mockContext.Verify(m => m.SaveChangesAsync(default), Times.Never);
            }
        }

        // Test pour vérifier que la méthode Get retourne une liste d'étudiants
        [Fact]
        public async Task Get_returnsListOfStudents()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Id = 1, FirstName = "John", LastName = "Doe" },
                new Student { Id = 2, FirstName = "Jane", LastName = "Smith" }
            }.AsQueryable(); // Création d'une liste d'étudiants et conversion en IQueryable

            var mockSet = new Mock<DbSet<Student>>(); // Création d'un mock de DbSet<Student>
            var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>()); // Création d'un mock de AppDbContext

            // Configuration du mock pour la liste d'étudiants
            mockSet.As<IQueryable<Student>>().Setup(m => m.Provider).Returns(students.Provider);
            mockSet.As<IQueryable<Student>>().Setup(m => m.Expression).Returns(students.Expression);
            mockSet.As<IQueryable<Student>>().Setup(m => m.ElementType).Returns(students.ElementType);
            mockSet.As<IQueryable<Student>>().Setup(m => m.GetEnumerator()).Returns(students.GetEnumerator());
            mockSet.As<IAsyncEnumerable<Student>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new TestAsyncEnumerator<Student>(students.GetEnumerator()));

            // Configuration du mock pour retourner le mock de DbSet<Student> pour la propriété Students
            mockContext.Setup(c => c.Students).Returns(mockSet.Object);

            // Création d'une instance du contrôleur StudentController en utilisant le contexte mocké
            var controller = new StudentController(mockContext.Object);

            // Act
            // Appel de la méthode Get du contrôleur
            var result = await controller.Get();

            // Assert
            // Vérification que le résultat est de type ActionResult<IEnumerable<Student>>
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Student>>>(result);
            // Vérification que la valeur retournée est de type IEnumerable<Student>
            var returnedStudents = Assert.IsAssignableFrom<IEnumerable<Student>>(actionResult.Value);
            // Vérification que la liste retournée contient le nombre attendu d'étudiants
            Assert.Equal(2, returnedStudents.Count());
            // Vérification que les étudiants retournés ont les noms attendus
            Assert.Collection(returnedStudents,
                student => Assert.Equal("John", student.FirstName),
                student => Assert.Equal("Jane", student.FirstName)
            );
        }

        // Classe interne pour simuler un énumérateur asynchrone
        internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _inner; // Énumérateur interne

            // Constructeur qui prend un énumérateur en paramètre
            public TestAsyncEnumerator(IEnumerator<T> inner)
            {
                _inner = inner;
            }

            // Méthode pour disposer de l'énumérateur
            public ValueTask DisposeAsync()
            {
                _inner.Dispose();
                return ValueTask.CompletedTask;
            }

            // Méthode pour avancer l'énumérateur de manière asynchrone
            public ValueTask<bool> MoveNextAsync()
            {
                return new ValueTask<bool>(_inner.MoveNext());
            }

            // Propriété pour obtenir l'élément courant
            public T Current => _inner.Current;
        }
    }
}