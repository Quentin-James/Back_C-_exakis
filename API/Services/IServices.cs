using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;

namespace Services
{
    public interface IServices
    {
        Task<IEnumerable<Student>> Get();
        Task<Student> Get(int id); // Ajout de cette méthode
        Task<Student> Post(Student student);
        Task<Student> Put(int id, Student student);
        Task<Student> Delete(int id);
    }
}