using DiscountManagement.Application.Contract.CustomerDiscount;
using System;
using System.Collections.Generic;
using _0_Framework.Application;
using DiscountManagement.Domain.CustomerDiscountAgg;

namespace DiscountManagement.Application
{
    public class CustomerDiscountApplication : ICustomerDiscountApplication
    {
        private readonly ICustomerDiscountRepository _customerDiscountRepository;

        public CustomerDiscountApplication(ICustomerDiscountRepository customerDiscountRepository)
        {
            _customerDiscountRepository = customerDiscountRepository;
        }

        public OperationResult Define(DefineCustomerDiscount command)
        {
            var operation = new OperationResult();
            if (_customerDiscountRepository.Exist(c =>
                c.ProductId == command.ProductId && c.DiscountRate == command.DiscountRate))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            var startDate = command.StartDate.ToGeorgianDateTime();
            var endDate = command.EndDate.ToGeorgianDateTime();
            var customerDiscount = new CustomerDiscount(command.ProductId, command.DiscountRate, startDate, endDate,
                command.Reason); 
            _customerDiscountRepository.Create(customerDiscount);
            _customerDiscountRepository.Save();
            return operation.Succeeded();
        }

        public OperationResult Edit(EditCustomerDiscount command)
        {
            var operation = new OperationResult();
            var customerDiscount = _customerDiscountRepository.Get(command.Id);

            if (customerDiscount == null)
                operation.Failed(ApplicationMessages.RecordNotFound);
            if (_customerDiscountRepository.Exist(c =>
                c.ProductId == command.ProductId && c.DiscountRate == command.DiscountRate && c.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            var startDate = command.StartDate.ToGeorgianDateTime();
            var endDate = command.EndDate.ToGeorgianDateTime();
            customerDiscount.Edit(command.Id,command.DiscountRate,startDate,endDate,command.Reason);
            _customerDiscountRepository.Save();
            return operation.Succeeded();

        }

        public List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel)
        {
            return _customerDiscountRepository.Search(searchModel);
        }

        public EditCustomerDiscount GetDetails(long id)
        {
           return _customerDiscountRepository.GetDetails(id);
        }
    }
}
