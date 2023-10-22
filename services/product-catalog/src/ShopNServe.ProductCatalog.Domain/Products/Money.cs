using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Values;

namespace ShopNServe.ProductCatalog.Products
{
    public class Money : ValueObject
    {
        private Money() 
            : this(Products.Currency.USD, 0)
        { /* for ORM use */ }

        public Money(Currency currency, decimal amount)
        {
            Currency = currency.ToString();
            Amount = amount;
        }

        public string Currency { get; private set; }
        public decimal Amount { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Currency; 
            yield return Amount;
        }
    }

    public class NullMoney : Money
    {
        public NullMoney() 
            : base(Products.Currency.USD, 0)
        {
        }
    }
}
