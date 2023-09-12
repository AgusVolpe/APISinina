﻿using TPFinalBitwise.Models;

namespace TPFinalBitwise.DAL.Interfaces
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        public Task<IEnumerable<Item>> ObtenerTodosConData();
        public Task<Item> ObtenerPorIdConData(int id);
    }
}
