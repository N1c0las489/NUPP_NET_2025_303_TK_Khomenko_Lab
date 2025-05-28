using System;
using System.Collections.Generic;
using System.Linq;

namespace Student.Common
{
    // Дженерік CRUD сервіс
    public class CrudService<T> : ICrudService<T> where T : class
    {
        private readonly List<T> _items;
        private readonly Func<T, Guid> _getIdFunc;

        // Конструктор
        public CrudService(Func<T, Guid> getIdFunc)
        {
            _items = new List<T>();
            _getIdFunc = getIdFunc ?? throw new ArgumentNullException(nameof(getIdFunc));
        }

        // Метод для створення елементу
        public void Create(T element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            _items.Add(element);
        }

        // Метод для читання елементу по ID
        public T Read(Guid id)
        {
            return _items.FirstOrDefault(item => _getIdFunc(item) == id);
        }

        // Метод для читання всіх елементів
        public IEnumerable<T> ReadAll()
        {
            return _items.ToList();
        }

        // Метод для оновлення елементу
        public void Update(T element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            var id = _getIdFunc(element);
            var existingIndex = _items.FindIndex(item => _getIdFunc(item) == id);

            if (existingIndex >= 0)
            {
                _items[existingIndex] = element;
            }
            else
            {
                throw new InvalidOperationException($"Element with ID {id} not found");
            }
        }

        // Метод для видалення елементу
        public void Remove(T element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            var id = _getIdFunc(element);
            var itemToRemove = _items.FirstOrDefault(item => _getIdFunc(item) == id);

            if (itemToRemove != null)
            {
                _items.Remove(itemToRemove);
            }
        }

        // Додатковий метод для отримання кількості елементів
        public int Count => _items.Count;
    }
}