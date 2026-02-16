using Core;
using Core.Models.Business;
using DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Университетская система ===");
        var connectionString = "Host=localhost;Database=UniversityDb;Username=postgres;Password=336396";
        var optionsBuilder = new DbContextOptionsBuilder<UniversityDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        
        using var context = new UniversityDbContext(optionsBuilder.Options);

        await context.Database.EnsureCreatedAsync();
        Console.WriteLine("База данных подключена успешно!");
        
        var facade = new Facade(context);

        await RunMainMenu(facade);
    }
    
    static async Task RunMainMenu(Facade facade)
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("\n=== Главное меню ===");
            Console.WriteLine("1. Управление группами");
            Console.WriteLine("2. Управление студентами");
            Console.WriteLine("3. Управление кураторами");
            Console.WriteLine("4. Показать все");
            Console.WriteLine("5. Аналитические запросы");
            Console.WriteLine("6. Выход");
            Console.Write("Выбор: ");
            
            var choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    await ManageGroups(facade);
                    break;
                case "2":
                    await ManageStudents(facade);
                    break;
                case "3":
                    await ManageCurators(facade);
                    break;
                case "4":
                    await ShowAll(facade);
                    break;
                case "5":
                    await AnalyticalQueries(facade);
                    break;
                case "6":
                    running = false;
                    Console.WriteLine("Выход из программы...");
                    break;
            }
        }
    }
    
    static async Task ManageGroups(Facade facade)
    {
        while (true)
        {
            Console.WriteLine("\n=== Управление группами ===");
            Console.WriteLine("1. Создать группу");
            Console.WriteLine("2. Показать все группы");
            Console.WriteLine("3. Обновить группу");
            Console.WriteLine("4. Удалить группу");
            Console.WriteLine("5. Назад");
            Console.Write("Выбор: ");
            
            var choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    await CreateGroup(facade);
                    break;
                case "2":
                    await ShowGroups(facade);
                    break;
                case "3":
                    await UpdateGroup(facade);
                    break;
                case "4":
                    await DeleteGroup(facade);
                    break;
                case "5":
                    return;
            }
        }
    }
    
    static async Task CreateGroup(Facade facade)
    {
        Console.Write("Название группы: ");
        var name = Console.ReadLine();
        if (!string.IsNullOrEmpty(name))
        {
            try
            {
                var groupId = await facade.CreateGroupAsync(name);
                Console.WriteLine($"Создана группа ID={groupId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
    
    static async Task ShowGroups(Facade facade)
    {
        var groups = await facade.GetAllGroupsAsync();
        Console.WriteLine("\nСписок групп:");
        foreach (var g in groups)
            Console.WriteLine($"ID: {g.Id}, Name: {g.Name}, Created: {g.CreationDate:yyyy-MM-dd}");
    }
    
    static async Task UpdateGroup(Facade facade)
    {
        await ShowGroups(facade);
        Console.Write("\nID группы для обновления: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Console.Write("Новое название: ");
            var newName = Console.ReadLine();
            if (!string.IsNullOrEmpty(newName))
            {
                var success = await facade.UpdateGroupAsync(id, newName);
                Console.WriteLine(success ? "Группа обновлена" : "Группа не найдена");
            }
        }
    }
    
    static async Task DeleteGroup(Facade facade)
    {
        await ShowGroups(facade);
        Console.Write("\nID группы для удаления: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Console.Write("Вы уверены? (y/n): ");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                var success = await facade.DeleteGroupAsync(id);
                Console.WriteLine(success ? "Группа удалена" : "Группа не найдена");
            }
        }
    }
    
    static async Task ManageStudents(Facade facade)
    {
        while (true)
        {
            Console.WriteLine("\n=== Управление студентами ===");
            Console.WriteLine("1. Создать студента");
            Console.WriteLine("2. Показать всех студентов");
            Console.WriteLine("3. Обновить студента");
            Console.WriteLine("4. Удалить студента");
            Console.WriteLine("5. Назад");
            Console.Write("Выбор: ");
            
            var choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    await CreateStudent(facade);
                    break;
                case "2":
                    await ShowStudents(facade);
                    break;
                case "3":
                    await UpdateStudent(facade);
                    break;
                case "4":
                    await DeleteStudent(facade);
                    break;
                case "5":
                    return;
            }
        }
    }
    
    static async Task CreateStudent(Facade facade)
    {
        Console.Write("ID группы: ");
        if (!int.TryParse(Console.ReadLine(), out int groupId))
        {
            Console.WriteLine("Неверный ID");
            return;
        }
        
        Console.Write("Имя студента: ");
        var name = Console.ReadLine();
        
        Console.Write("Возраст: ");
        if (!int.TryParse(Console.ReadLine(), out int age))
        {
            Console.WriteLine("Неверный возраст");
            return;
        }
        
        if (!string.IsNullOrEmpty(name))
        {
            try
            {
                var studentId = await facade.CreateStudentAsync(groupId, name, age);
                Console.WriteLine($"Создан студент ID={studentId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
    
    static async Task ShowStudents(Facade facade)
    {
        var students = await facade.GetAllStudentsAsync();
        Console.WriteLine("\nСписок студентов:");
        foreach (var s in students)
            Console.WriteLine($"ID: {s.Id}, Name: {s.Name}, Age: {s.Age}, GroupID: {s.GroupId}");
    }
    
    static async Task UpdateStudent(Facade facade)
    {
        await ShowStudents(facade);
        Console.Write("\nID студента для обновления: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Неверный ID");
            return;
        }
        
        Console.Write("Новое имя: ");
        var newName = Console.ReadLine();
        
        Console.Write("Новый возраст: ");
        if (!int.TryParse(Console.ReadLine(), out int newAge))
        {
            Console.WriteLine("Неверный возраст");
            return;
        }
        
        if (!string.IsNullOrEmpty(newName))
        {
            var success = await facade.UpdateStudentAsync(id, newName, newAge);
            Console.WriteLine(success ? "Студент обновлен" : "Студент не найден");
        }
    }
    
    static async Task DeleteStudent(Facade facade)
    {
        await ShowStudents(facade);
        Console.Write("\nID студента для удаления: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Console.Write("Вы уверены? (y/n): ");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                var success = await facade.DeleteStudentAsync(id);
                Console.WriteLine(success ? "Студент удален" : "Студент не найден");
            }
        }
    }
    
    static async Task ManageCurators(Facade facade)
    {
        while (true)
        {
            Console.WriteLine("\n=== Управление кураторами ===");
            Console.WriteLine("1. Создать куратора");
            Console.WriteLine("2. Показать всех кураторов");
            Console.WriteLine("3. Обновить куратора");
            Console.WriteLine("4. Удалить куратора");
            Console.WriteLine("5. Назад");
            Console.Write("Выбор: ");
            
            var choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    await CreateCurator(facade);
                    break;
                case "2":
                    await ShowCurators(facade);
                    break;
                case "3":
                    await UpdateCurator(facade);
                    break;
                case "4":
                    await DeleteCurator(facade);
                    break;
                case "5":
                    return;
            }
        }
    }
    
    static async Task CreateCurator(Facade facade)
    {
        Console.Write("ID группы: ");
        if (!int.TryParse(Console.ReadLine(), out int groupId))
        {
            Console.WriteLine("Неверный ID");
            return;
        }
        
        Console.Write("Имя куратора: ");
        var name = Console.ReadLine();
        
        Console.Write("Email: ");
        var email = Console.ReadLine();
        
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(email))
        {
            try
            {
                var curatorId = await facade.CreateCuratorAsync(groupId, name, email);
                Console.WriteLine($"Создан куратор ID={curatorId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
    
    static async Task ShowCurators(Facade facade)
    {
        var curators = await facade.GetAllCuratorsAsync();
        Console.WriteLine("\nСписок кураторов:");
        foreach (var c in curators)
            Console.WriteLine($"ID: {c.Id}, Name: {c.Name}, Email: {c.Email}, GroupID: {c.GroupId}");
    }
    
    static async Task UpdateCurator(Facade facade)
    {
        await ShowCurators(facade);
        Console.Write("\nID куратора для обновления: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Неверный ID");
            return;
        }
        
        Console.Write("Новое имя: ");
        var newName = Console.ReadLine();
        
        Console.Write("Новый email: ");
        var newEmail = Console.ReadLine();
        
        if (!string.IsNullOrEmpty(newName) && !string.IsNullOrEmpty(newEmail))
        {
            var success = await facade.UpdateCuratorAsync(id, newName, newEmail);
            Console.WriteLine(success ? "Куратор обновлен" : "Куратор не найден");
        }
    }
    
    static async Task DeleteCurator(Facade facade)
    {
        await ShowCurators(facade);
        Console.Write("\nID куратора для удаления: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Console.Write("Вы уверены? (y/n): ");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                var success = await facade.DeleteCuratorAsync(id);
                Console.WriteLine(success ? "Куратор удален" : "Куратор не найден");
            }
        }
    }
    
    static async Task ShowAll(Facade facade)
    {
        Console.WriteLine("\n=== ВСЕ ДАННЫЕ ===");
        await ShowGroups(facade);
        await ShowStudents(facade);
        await ShowCurators(facade);
    }
    
    static async Task AnalyticalQueries(Facade facade)
    {
        Console.WriteLine("\n=== Аналитические запросы ===");
        Console.WriteLine("1. Количество студентов в группе");
        Console.WriteLine("2. Имя куратора по ID студента");
        Console.WriteLine("3. Средний возраст студентов по куратору");
        Console.Write("Выбор: ");
        
        var queryChoice = Console.ReadLine();
        
        switch (queryChoice)
        {
            case "1":
                Console.Write("ID группы: ");
                if (int.TryParse(Console.ReadLine(), out int groupId))
                {
                    var count = await facade.GetStudentCountInGroupAsync(groupId);
                    Console.WriteLine($"Студентов в группе: {count}");
                }
                break;
                
            case "2":
                Console.Write("ID студента: ");
                if (int.TryParse(Console.ReadLine(), out int studentId))
                {
                    var name = await facade.GetCuratorNameForStudentAsync(studentId);
                    Console.WriteLine($"Куратор: {name ?? "не найден"}");
                }
                break;
                
            case "3":
                Console.Write("ID куратора: ");
                if (int.TryParse(Console.ReadLine(), out int curatorId))
                {
                    var avgAge = await facade.GetAverageAgeByCuratorAsync(curatorId);
                    Console.WriteLine($"Средний возраст: {avgAge?.ToString("F2") ?? "нет данных"}");
                }
                break;
        }
    }
}