using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechXpress.Domain.Models;
using TechXpress.Infrastructure;

namespace TechXpress.Core.Services
{
    public class PaymentService
    {
        //private readonly IRepository<Payment> _paymentRepo;
        //private readonly Action<string> _logAction;

        //public PaymentService(IRepository<Payment> paymentRepo)
        //{
        //    _paymentRepo = paymentRepo;
        //    _logAction = message => Console.WriteLine($"Log: {message}");
        //}

        //public async Task<IEnumerable<Payment>> GetAllPaymentAsync()
        //{
        //    return await _paymentRepo.GetAllAsync();
        //}

        //public async Task<Payment> GetPaymentByIdAsync(int id)
        //{
        //    return await _paymentRepo.GetByIdAsync(id);
        //}

        //public async Task AddPaymentAsync(Payment payment)
        //{
        //    await _paymentRepo.AddAsync(payment, _logAction);
        //}

        //public async Task UpdatePaymentAsync(Payment payment)
        //{
        //    await _paymentRepo.UpdateAsync(payment, _logAction);
        //}

        //public async Task DeletePaymentAsync(int id)
        //{
        //    await _paymentRepo.DeleteAsync(id, _logAction);
        //}
    }
}