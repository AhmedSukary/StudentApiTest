using StudentApi.Model;
namespace StudentApi.DataSimulation
{
    public class StudentDataSimulation
    {
        // Static list of students, acting as an in-memory data store, you can change it later on to retrieve students from Database.
        public static readonly List<Student> StudentsList = new List<Student>
        {
            // Initialize the list with some student objects.
            new Student{ Id = 1, Name = "Ahmed Sukary", Age =22, Grade =89.5},
            new Student{ Id = 2, Name = "Maram Halaaq", Age =17, Grade =98.7},
            new Student{ Id = 3, Name = "Ola Jaber", Age =21, Grade =57.9},
            new Student{ Id = 4, Name = "Alia Maher", Age =19, Grade =44.4}
        };
    }
}