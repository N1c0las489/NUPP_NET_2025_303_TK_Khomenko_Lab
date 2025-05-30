using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University2.REST.Models;
using University2.REST.Interfaces;

namespace University2.REST.Services
{
    public class FakeStudentAddressService : ICrudServiceAsync<StudentAddressModel>
    {
        private readonly ConcurrentDictionary<int, StudentAddressModel> _addresses = new();

        public Task<bool> CreateAsync(StudentAddressModel address)
        {
            if (address.Id == 0)
                address.Id = _addresses.Count + 1;

            return Task.FromResult(_addresses.TryAdd(address.Id, address));
        }

        public Task<StudentAddressModel> ReadAsync(int id)
        {
            _addresses.TryGetValue(id, out var address);
            return Task.FromResult(address);
        }

        public Task<IEnumerable<StudentAddressModel>> ReadAllAsync()
        {
            return Task.FromResult(_addresses.Values.AsEnumerable());
        }

        public Task<IEnumerable<StudentAddressModel>> ReadAllAsync(int page, int amount)
        {
            return Task.FromResult(_addresses.Values
                .Skip((page - 1) * amount)
                .Take(amount));
        }

        public Task<bool> UpdateAsync(StudentAddressModel address)
        {
            if (_addresses.ContainsKey(address.Id))
            {
                _addresses[address.Id] = address;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> RemoveAsync(StudentAddressModel address)
        {
            return Task.FromResult(_addresses.TryRemove(address.Id, out _));
        }

        public Task<bool> SaveAsync() => Task.FromResult(true);

        // Не використовується для адрес (ID - int)
        public Task<StudentAddressModel> ReadAsync(Guid id) =>
            throw new NotSupportedException("Use ReadAsync(int id) for addresses");

        
    }
}