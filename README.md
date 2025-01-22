 // Test pour vérifier que la méthode Delete retourne NotFound si l'étudiant n'existe pas
 [Fact]
 public async Task Delete_method_should_return_not_found()
 {
     // Arrange
     var studentId = 1; // ID de l'étudiant à supprimer
     var mockSet = new Mock<DbSet<Student>>(); // Création d'un mock de DbSet<Student>
     var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>()); // Création d'un mock de AppDbContext

     // Configuration du mock pour retourner null lorsque FindAsync est appelé avec l'ID de l'étudiant
     mockSet.Setup(m => m.FindAsync(studentId)).ReturnsAsync((Student?)null);
     // Configuration du mock pour retourner le mock de DbSet<Student> pour la propriété Students
     mockContext.Setup(m => m.Students).Returns(mockSet.Object);

     // Création d'une instance du contrôleur StudentController en utilisant le contexte mocké
     var controller = new StudentController(mockContext.Object);

     // Act
     // Appel de la méthode Delete du contrôleur avec l'ID de l'étudiant
     var result = await controller.Delete(studentId);

     // Assert
     // Vérification que le résultat est de type NotFoundResult
     Assert.IsType<NotFoundResult>(result);
     // Vérification que la méthode Remove du mock de DbSet<Student> n'a jamais été appelée
     mockSet.Verify(m => m.Remove(It.IsAny<Student>()), Times.Never);
     // Vérification que la méthode SaveChangesAsync du mock de AppDbContext n'a jamais été appelée
     mockContext.Verify(m => m.SaveChangesAsync(default), Times.Never);
 }

 // Test pour vérifier que la méthode Delete retourne NoContent si l'étudiant est supprimé avec succès
 [Fact]
 public async Task Delete_method_should_return_no_content()
 {
     // Arrange
     var studentId = 1; // ID de l'étudiant à supprimer
     var student = new Student { Id = studentId, FirstName = "John", LastName = "Kennedy" }; // Création d'un étudiant
     var mockSet = new Mock<DbSet<Student>>(); // Création d'un mock de DbSet<Student>
     var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>()); // Création d'un mock de AppDbContext

     // Configuration du mock pour retourner l'étudiant lorsque FindAsync est appelé avec l'ID de l'étudiant
     mockSet.Setup(m => m.FindAsync(studentId)).ReturnsAsync(student);
     // Configuration du mock pour retourner le mock de DbSet<Student> pour la propriété Students
     mockContext.Setup(m => m.Students).Returns(mockSet.Object);

     // Création d'une instance du contrôleur StudentController en utilisant le contexte mocké
     var controller = new StudentController(mockContext.Object);

     // Act
     // Appel de la méthode Delete du contrôleur avec l'ID de l'étudiant
     var result = await controller.Delete(studentId);

     // Assert
     // Vérification que le résultat est de type NoContentResult
     Assert.IsType<NoContentResult>(result);
     // Vérification que la méthode Remove du mock de DbSet<Student> a été appelée une fois avec l'étudiant
     mockSet.Verify(m => m.Remove(student), Times.Once);
     // Vérification que la méthode SaveChangesAsync du mock de AppDbContext a été appelée une fois
     mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
 }
