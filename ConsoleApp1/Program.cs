using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Birthday
{
    public string? Name { get; set; }

    public DateTime Date { get; set; }
}

class Program
{
    private static List<Birthday> birthdays = new List<Birthday>();
    private const string dataFilePath = "birthday_data.txt";

    static void Main()
    {
        LoadData();

        while (true)
        {
            // Console.Clear();
            Console.WriteLine("Поздравлятор - Главное меню");
            Console.WriteLine("1. Все ДР");
            Console.WriteLine("2. Сегодня и ближайшие ДР");
            Console.WriteLine("3. Добавить ДР");
            Console.WriteLine("4. Удалить ДР");
            Console.WriteLine("5. Редактировать ДР");
            Console.WriteLine("6. Выйти");

            Console.Write("Выберите действие (1-6): ");
            string choice = Console.ReadLine()!; 

            switch (choice)
            {
                case "1":
                    ShowAllBirthdays();
                    break;
                case "2":
                    ShowUpcomingBirthdays();
                    break;
                case "3":
                    AddBirthday();
                    break;
                case "4":
                    DeleteBirthday();
                    break;
                case "5":
                    EditBirthday();
                    break;
                case "6":
                    SaveData();
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Некорректный ввод. Пожалуйста, повторите.");
                    break;
            }

            Console.WriteLine("Нажмите Enter, чтобы продолжить...");
            Console.ReadLine();
        }
    }

    static void ShowAllBirthdays()
    {
        Console.WriteLine("Список всех ДР:");
        foreach (var birthday in birthdays)
        {
            Console.WriteLine($"{birthday.Name} - {birthday.Date.ToShortDateString()}");
        }
    }

    static void ShowUpcomingBirthdays()
    {
        var upcomingBirthdays = birthdays
            .Where(b => b.Date >= DateTime.Today)
            .OrderBy(b => b.Date);

        Console.WriteLine("Список сегодняшних и ближайших ДР:");
        foreach (var birthday in upcomingBirthdays)
        {
            Console.WriteLine($"{birthday.Name} - {birthday.Date.ToShortDateString()}");
        }
    }

    static void AddBirthday()
    {
        Console.Write("Введите имя: ");
        string name = Console.ReadLine()!;
        DateTime date = AddBirthdayDateTime();

        if (date != DateTime.MinValue) // Проверка, что дата была успешно введена
        {
            birthdays.Add(new Birthday { Name = name, Date = date });
            Console.WriteLine("ДР успешно добавлен.");
        }
    }

    static void DeleteBirthday()
    {
        Console.Write("Введите имя для удаления: ");
        string name = Console.ReadLine()!;

        var birthdayToDelete = birthdays.Find(b => b.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (birthdayToDelete != null)
        {
            birthdays.Remove(birthdayToDelete);
            Console.WriteLine("ДР успешно удален.");
        }
        else
        {
            Console.WriteLine("ДР с таким именем не найден.");
        }
    }

    static void EditBirthday()
    {
        Console.Write("Введите имя для редактирования: ");
        string name = Console.ReadLine()!;

        var birthdayToEdit = birthdays.Find(b => b.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (birthdayToEdit != null)
        {
            Console.Write("Введите новую дату ДР (в формате ДД.ММ.ГГГГ): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime newDate))
            {
                birthdayToEdit.Date = newDate;
                Console.WriteLine("ДР успешно отредактирован.");
            }
            else
            {
                Console.WriteLine("Ошибка в формате даты. Введите дату в формате ДД.ММ.ГГГГ.");
            }
        }
        else
        {
            Console.WriteLine("ДР с таким именем не найден.");
        }
    }

    static void LoadData()
    {
        if (File.Exists(dataFilePath))
        {
            try
            {
                string[] lines = File.ReadAllLines(dataFilePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');
                    if (parts.Length == 2 && DateTime.TryParse(parts[1], out DateTime date))
                    {
                        birthdays.Add(new Birthday { Name = parts[0], Date = date });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
            }
        }
    }

    static void SaveData()
    {
        try
        {
            List<string> lines = birthdays.Select(b => $"{b.Name};{b.Date.ToShortDateString()}").ToList();
            File.WriteAllLines(dataFilePath, lines);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении данных: {ex.Message}");
        }
    }

    static DateTime AddBirthdayDateTime()
    {
        DateTime date;
        bool isValidDate = false;
        int attempts = 0;
        const int maxAttempts = 2;

        do
        {
            Console.Write("Введите дату ДР (в формате ДД.ММ.ГГГГ): ");
            if (DateTime.TryParse(Console.ReadLine(), out date))
            {
                isValidDate = true;
            }
            else
            {
                attempts++;
                Console.WriteLine($"attempts: {attempts}");
                if (attempts > maxAttempts)
                {
                    Console.WriteLine($"Превышено максимальное количество попыток.");
                    return DateTime.MinValue; // или любое другое значение по умолчанию
                }
                Console.WriteLine($"Ошибка в формате даты. Введите дату в формате ДД.ММ.ГГГГ. Осталось {maxAttempts - attempts} попыток");
            }
        } while (!isValidDate);

        return date;
    }

}
