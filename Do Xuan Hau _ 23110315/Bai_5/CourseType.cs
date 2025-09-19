using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class Student
{
    public string Name { get; set; }
    public List<CourseRegistration> RegisteredCourses { get; set; }

    public Student(string name)
    {
        Name = name;
        RegisteredCourses = new List<CourseRegistration>();
    }

    public bool AddCourse(string courseName, int semester)
    {
        if (!RegisteredCourses.Any(c => c.CourseName.Equals(courseName, StringComparison.OrdinalIgnoreCase) && c.Semester == semester))
        {
            RegisteredCourses.Add(new CourseRegistration(courseName, semester));
            return true;
        }
        return false;
    }
}

public class CourseRegistration
{
    public string CourseName { get; set; }
    public int Semester { get; set; }

    public CourseRegistration(string courseName, int semester)
    {
        CourseName = courseName;
        Semester = semester;
    }

    public override string ToString()
    {
        return $"Môn: {CourseName,-7} | Học kỳ: {Semester}";
    }
}

public class StudentManager
{
    private List<Student> studentList;
    private string filePath;

    public StudentManager(string path)
    {
        filePath = path;
        studentList = new List<Student>();
        LoadStudentsFromFile();
    }

    public void LoadStudentsFromFile()
    {
        if (!File.Exists(filePath)) return;
        studentList.Clear();
        var lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var parts = line.Split(',');
            if (parts.Length == 3 && int.TryParse(parts[1].Trim(), out int semester))
            {
                string name = parts[0].Trim();
                string course = parts[2].Trim();
                Student student = studentList.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (student == null)
                {
                    student = new Student(name);
                    studentList.Add(student);
                }
                student.AddCourse(course, semester);
            }
        }
        Console.WriteLine($"Đã tải thông tin của {studentList.Count} sinh viên từ file.");
    }

    private void SaveChangesToFile()
    {
        var lines = new List<string>();
        foreach (var student in studentList.OrderBy(s => s.Name))
        {
            foreach (var course in student.RegisteredCourses)
            {
                lines.Add($"{student.Name},{course.Semester},{course.CourseName}");
            }
        }
        File.WriteAllLines(filePath, lines);
    }

    public Student FindStudentByName(string name)
    {
        return studentList.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public bool AddRegistration(string name, int semester, string courseName)
    {
        Student student = FindStudentByName(name);
        if (student == null)
        {
            student = new Student(name);
            studentList.Add(student);
        }
        bool result = student.AddCourse(courseName, semester);
        if (result) SaveChangesToFile();
        return result;
    }

    public bool DeleteStudent(string name)
    {
        var student = FindStudentByName(name);
        if (student != null)
        {
            studentList.Remove(student);
            SaveChangesToFile();
            return true;
        }
        return false;
    }

    public bool UpdateRegistration(string studentName, int courseIndex, string newCourseName, int newSemester)
    {
        Student student = FindStudentByName(studentName);
        if (student == null || courseIndex < 0 || courseIndex >= student.RegisteredCourses.Count)
        {
            return false;
        }

        for (int i = 0; i < student.RegisteredCourses.Count; i++)
        {
            if (i == courseIndex) continue;
            var reg = student.RegisteredCourses[i];
            if (reg.CourseName.Equals(newCourseName, StringComparison.OrdinalIgnoreCase) && reg.Semester == newSemester)
            {
                Console.WriteLine("Lỗi: Thông tin mới bị trùng với một đăng ký đã có.");
                return false;
            }
        }

        student.RegisteredCourses[courseIndex].CourseName = newCourseName;
        student.RegisteredCourses[courseIndex].Semester = newSemester;
        SaveChangesToFile();
        return true;
    }

    public void PrintRegistrationReport()
    {
        Console.WriteLine("\n--- THỐNG KÊ SỐ LƯỢT ĐĂNG KÝ THEO MÔN HỌC ---");
        if (!studentList.Any())
        {
            Console.WriteLine("Không có dữ liệu để thống kê.");
            return;
        }
        var allRegistrations = studentList.SelectMany(student =>
            student.RegisteredCourses.Select(course => new { student.Name, course.CourseName })
        );
        var reportData = allRegistrations
            .GroupBy(reg => reg.CourseName, StringComparer.OrdinalIgnoreCase)
            .OrderBy(courseGroup => courseGroup.Key);
        Console.WriteLine("Student Name      | Course     | Total of Course");
        Console.WriteLine("------------------|------------|-----------------");
        foreach (var courseGroup in reportData)
        {
            var studentsInCourse = courseGroup
                .GroupBy(reg => reg.Name)
                .Select(studentGroup => new { StudentName = studentGroup.Key, Count = studentGroup.Count() })
                .OrderBy(item => item.StudentName);
            foreach (var item in studentsInCourse)
            {
                Console.WriteLine($"{item.StudentName,-17} | {courseGroup.Key,-10} | {item.Count}");
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        StudentManager manager = new StudentManager("students.txt");

        bool isRunning = true;
        while (isRunning)
        {
            Console.WriteLine("\n--- CHƯƠNG TRÌNH QUẢN LÝ SINH VIÊN (TÊN DUY NHẤT) ---");
            Console.WriteLine("1. Thêm sinh viên / lượt đăng ký");
            Console.WriteLine("2. Sửa thông tin đăng ký của sinh viên");
            Console.WriteLine("3. Xóa sinh viên");
            Console.WriteLine("4. Tìm kiếm sinh viên");
            Console.WriteLine("5. Xem thống kê");
            Console.WriteLine("6. Thoát");
            Console.Write("Vui lòng chọn chức năng: ");

            switch (Console.ReadLine())
            {
                case "1": AddStudents(manager); break;
                case "2": UpdateRegistration(manager); break;
                case "3": RemoveStudents(manager); break;
                case "4": SearchStudent(manager); break;
                case "5": manager.PrintRegistrationReport(); break;
                case "6": isRunning = false; break;
                default: Console.WriteLine("Lựa chọn không hợp lệ."); break;
            }
        }
        Console.WriteLine("Cảm ơn đã sử dụng chương trình!");
    }

    static void AddStudents(StudentManager manager)
    {
        int count;
        while (true)
        {
            Console.Write("\nNhập số lượng lượt đăng ký bạn muốn thêm: ");
            if (int.TryParse(Console.ReadLine(), out count) && count > 0)
            {
                break;
            }
            Console.WriteLine("Số lượng không hợp lệ. Vui lòng nhập một số lớn hơn 0.");
        }

        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"\n--- Thêm lượt đăng ký thứ {i + 1}/{count} ---");

            string name;
            while (true)
            {
                Console.Write("Nhập tên sinh viên: ");
                name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    break;
                }
                Console.WriteLine("Tên sinh viên không được để trống. Vui lòng nhập lại.");
            }

            int semester;
            while (true)
            {
                Console.Write("Nhập học kỳ: ");
                if (int.TryParse(Console.ReadLine(), out semester) && semester > 0)
                {
                    break;
                }
                Console.WriteLine("Học kỳ không hợp lệ. Vui lòng nhập số nguyên dương.");
            }

            string courseName;
            var allowedCourses = new List<string> { "Java", ".Net", "C/C++" };
            while (true)
            {
                Console.Write("Nhập tên môn học (Java, .Net, C/C++): ");
                courseName = Console.ReadLine();
                if (allowedCourses.Any(c => c.Equals(courseName, StringComparison.OrdinalIgnoreCase)))
                {
                    courseName = allowedCourses.First(c => c.Equals(courseName, StringComparison.OrdinalIgnoreCase));
                    break;
                }
                Console.WriteLine("Tên môn học không hợp lệ. Vui lòng nhập lại.");
            }

            if (manager.AddRegistration(name, semester, courseName))
            {
                Console.WriteLine("=> Thêm thành công!");
            }
            else
            {
                Console.WriteLine("=> Lượt đăng ký này đã tồn tại, không thêm.");
            }
        }
    }

    static void RemoveStudents(StudentManager manager)
    {
        int count;
        while (true)
        {
            Console.Write("\nNhập số lượng sinh viên bạn muốn xóa: ");
            if (int.TryParse(Console.ReadLine(), out count) && count > 0)
            {
                break;
            }
            Console.WriteLine("Số lượng không hợp lệ. Vui lòng nhập một số lớn hơn 0.");
        }

        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"\n--- Xóa sinh viên thứ {i + 1}/{count} ---");

            string name;
            while (true)
            {
                Console.Write("Nhập tên sinh viên cần xóa: ");
                name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    break;
                }
                Console.WriteLine("Tên sinh viên không được để trống. Vui lòng nhập lại.");
            }

            if (manager.DeleteStudent(name))
            {
                Console.WriteLine($"=> Đã xóa thành công sinh viên '{name}'.");
            }
            else
            {
                Console.WriteLine($"=> Không tìm thấy sinh viên '{name}'.");
            }
        }
    }

    static void SearchStudent(StudentManager manager)
    {
        string name;
        while (true)
        {
            Console.Write("\nNhập tên sinh viên cần tìm: ");
            name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
            {
                break;
            }
            Console.WriteLine("Tên sinh viên không được để trống. Vui lòng nhập lại.");
        }

        var student = manager.FindStudentByName(name);
        if (student != null)
        {
            Console.WriteLine($"\nThông tin sinh viên: {student.Name}");
            Console.WriteLine("Các môn đã đăng ký (sắp xếp theo học kỳ):");
            if (student.RegisteredCourses.Any())
            {
                foreach (var course in student.RegisteredCourses.OrderBy(c => c.Semester))
                {
                    Console.WriteLine($"- {course}");
                }
            }
            else
            {
                Console.WriteLine("Chưa đăng ký môn nào.");
            }
        }
        else
        {
            Console.WriteLine($"Không tìm thấy sinh viên nào có tên '{name}'.");
        }
    }

    static void UpdateRegistration(StudentManager manager)
    {
        Console.WriteLine("\n--- SỬA THÔNG TIN ĐĂNG KÝ ---");

        string name;
        while (true)
        {
            Console.Write("Nhập tên sinh viên cần sửa: ");
            name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name)) break;
            Console.WriteLine("Tên không được để trống.");
        }

        Student student = manager.FindStudentByName(name);
        if (student == null)
        {
            Console.WriteLine($"Không tìm thấy sinh viên '{name}'.");
            return;
        }

        if (!student.RegisteredCourses.Any())
        {
            Console.WriteLine($"Sinh viên '{name}' chưa đăng ký môn nào.");
            return;
        }

        Console.WriteLine("Danh sách các lượt đăng ký của sinh viên:");
        for (int i = 0; i < student.RegisteredCourses.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {student.RegisteredCourses[i]}");
        }

        int choice;
        while (true)
        {
            Console.Write("Chọn lượt đăng ký bạn muốn sửa (nhập số): ");
            if (int.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= student.RegisteredCourses.Count)
            {
                break;
            }
            Console.WriteLine("Lựa chọn không hợp lệ.");
        }
        int courseIndex = choice - 1;

        Console.WriteLine("Nhập thông tin mới cho lượt đăng ký này:");
        int newSemester;
        while (true)
        {
            Console.Write("Học kỳ mới: ");
            if (int.TryParse(Console.ReadLine(), out newSemester) && newSemester > 0) break;
            Console.WriteLine("Học kỳ không hợp lệ.");
        }

        string newCourseName;
        var allowedCourses = new List<string> { "Java", ".Net", "C/C++" };
        while (true)
        {
            Console.Write("Tên môn học mới (Java, .Net, C/C++): ");
            newCourseName = Console.ReadLine();
            if (allowedCourses.Any(c => c.Equals(newCourseName, StringComparison.OrdinalIgnoreCase)))
            {
                newCourseName = allowedCourses.First(c => c.Equals(newCourseName, StringComparison.OrdinalIgnoreCase));
                break;
            }
            Console.WriteLine("Tên môn học không hợp lệ.");
        }

        if (manager.UpdateRegistration(name, courseIndex, newCourseName, newSemester))
        {
            Console.WriteLine("=> Cập nhật thành công!");
        }
        else
        {
            Console.WriteLine("=> Cập nhật thất bại. Vui lòng thử lại.");
        }
    }
}
