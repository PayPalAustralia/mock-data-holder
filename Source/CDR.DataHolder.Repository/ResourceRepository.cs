﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CDR.DataHolder.Domain.Entities;
using CDR.DataHolder.Domain.Repositories;
using CDR.DataHolder.Domain.ValueObjects;
using CDR.DataHolder.Repository.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CDR.DataHolder.Repository
{
    public class ResourceRepository : IResourceRepository
	{
		private readonly DataHolderDatabaseContext _dataHolderDatabaseContext;
		private readonly IMapper _mapper;

		public ResourceRepository(DataHolderDatabaseContext dataHolderDatabaseContext, IMapper mapper)
		{
			this._dataHolderDatabaseContext = dataHolderDatabaseContext;
			this._mapper = mapper;
		}

		public async Task<Customer> GetCustomer(Guid customerId)
		{
			var customer = await _dataHolderDatabaseContext.Customers.AsNoTracking()
				.Include(p => p.Person)
				.Include(o => o.Organisation)
				.FirstOrDefaultAsync(customer => customer.CustomerId == customerId);
			if (customer == null)
			{
				return null;
			}

			switch (customer.CustomerUType.ToLower())
			{
				case "organisation":
					return _mapper.Map<Organisation>(customer);

				case "person":
					return _mapper.Map<Person>(customer);

				default:
					return null;
			}
		}

		public async Task<Customer> GetCustomerByLoginId(string loginId)
		{
			var customer = await _dataHolderDatabaseContext.Customers.AsNoTracking()
				.FirstOrDefaultAsync(customer => customer.LoginId == loginId);
			return _mapper.Map<Customer>(customer);
		}

		public async Task<Page<Account[]>> GetAllAccounts(AccountFilter accountFilter, int page, int pageSize)
		{
			var result = new Page<Account[]>()
			{
				Data = new Account[] { },
				CurrentPage = page,
				PageSize = pageSize,
			};

			// We always return accounts for the individual. We don't have a concept of joint or shared accounts at the moment
			// So, if asked from accounts which rent owned, just return empty result.
			if (accountFilter.IsOwned.HasValue && !accountFilter.IsOwned.Value)
			{
				return result;
			}

			// If none of the account ids are allowed, return empty list
			if (accountFilter.AllowedAccountIds == null || !accountFilter.AllowedAccountIds.Any())
			{
				return result;
			}

			IQueryable<Entities.Account> accountsQuery = _dataHolderDatabaseContext.Accounts.AsNoTracking()
				.Include(account => account.Customer)
				.Where(account => 
					accountFilter.AllowedAccountIds.Contains(account.AccountId)	
					&& account.Customer.CustomerId == accountFilter.CustomerId);

			// Apply filters.
			if (!string.IsNullOrEmpty(accountFilter.OpenStatus))
			{
				accountsQuery = accountsQuery.Where(account => account.OpenStatus == accountFilter.OpenStatus);
			}
			if (!string.IsNullOrEmpty(accountFilter.ProductCategory))
			{
				accountsQuery = accountsQuery.Where(account => account.ProductCategory == accountFilter.ProductCategory);
			}

			var totalRecords = await accountsQuery.CountAsync();

			// Apply ordering and pagination
			accountsQuery = accountsQuery
				.OrderBy(account => account.DisplayName).ThenBy(account => account.AccountId)
				.Skip((page - 1) * pageSize)
				.Take(pageSize);

			var accounts = await accountsQuery.ToListAsync();
			result.Data = _mapper.Map<Account[]>(accounts);
			result.TotalRecords = totalRecords;

			return result;
		}

		/// <summary>
		/// Check that the customer can access the given accounts.
		/// </summary>
		/// <param name="accountId">Account ID</param>
		/// <param name="customerId">Customer ID</param>
		/// <returns>True if the customer can access the account, otherwise false.</returns>
		public async Task<bool> CanAccessAccount(string accountId, Guid customerId)
		{
			return await _dataHolderDatabaseContext.Accounts.AnyAsync(a => a.AccountId == accountId && a.CustomerId == customerId);
		}

		/// <summary>
		/// Get a list of all transactions for a given account.
		/// </summary>
		/// <param name="transactionsFilter">Query filter</param>
		/// <param name="page">Page number</param>
		/// <param name="pageSize">Page size</param>
		/// <returns></returns>
		public async Task<Page<AccountTransaction[]>> GetAccountTransactions(AccountTransactionsFilter transactionsFilter, int page, int pageSize)
		{
			if (!transactionsFilter.NewestTime.HasValue)
			{
				transactionsFilter.NewestTime = DateTime.UtcNow;
			}

			if (!transactionsFilter.OldestTime.HasValue)
			{
				transactionsFilter.OldestTime = transactionsFilter.NewestTime.Value.AddDays(-90);
			}

			var result = new Page<AccountTransaction[]>()
			{
				Data = new AccountTransaction[] { },
				CurrentPage = page,
				PageSize = pageSize,
			};

            IQueryable<Entities.Transaction> accountTransactionsQuery = _dataHolderDatabaseContext
                            .Transactions.Include(x => x.Account).ThenInclude(x => x.Customer).AsNoTracking()
                    .Where(t => t.AccountId == transactionsFilter.AccountId && t.Account.CustomerId == transactionsFilter.CustomerId)
					// Oldest/Newest Time
					//Newest
					.WhereIf(transactionsFilter.NewestTime.HasValue,
							 t => (t.PostingDateTime ?? t.ExecutionDateTime) <= transactionsFilter.NewestTime)
					//Oldest
					.WhereIf(transactionsFilter.OldestTime.HasValue,
							 t => (t.PostingDateTime ?? t.ExecutionDateTime) >= transactionsFilter.OldestTime)

                    // Min/Max Amount
                    //Min
                    .WhereIf(transactionsFilter.MinAmount.HasValue,
							 t => t.Amount >= transactionsFilter.MinAmount.Value)
					//Max
					.WhereIf(transactionsFilter.MaxAmount.HasValue,
							 t => t.Amount <= transactionsFilter.MaxAmount.Value)					

					//Text
                    .WhereIf(!string.IsNullOrEmpty(transactionsFilter.Text), 
							 t => EF.Functions.Like(t.Description, $"%{transactionsFilter.Text}%") || EF.Functions.Like(t.Reference, $"%{transactionsFilter.Text}%"));

            var totalRecords = await accountTransactionsQuery.CountAsync();

            // Apply ordering and pagination
            accountTransactionsQuery = accountTransactionsQuery
                .OrderByDescending(t => t.PostingDateTime).ThenByDescending(t => t.ExecutionDateTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var transactions = await accountTransactionsQuery.ToListAsync();
            result.Data = _mapper.Map<AccountTransaction[]>(transactions);
            result.TotalRecords = totalRecords;

            return result;
        }

        public async Task<Account[]> GetAllAccountsByCustomerIdForConsent(Guid customerId)
        {
            var allAccounts = await _dataHolderDatabaseContext.Accounts.AsNoTracking()
                .Include(account => account.Customer)
                .Where(account => account.Customer.CustomerId == customerId)
                .OrderBy(account => account.DisplayName).ThenBy(account => account.AccountId)
                .ToListAsync();

            return _mapper.Map<Account[]>(allAccounts);
        }

		public async Task<Balance> GetAccountBalanceByAccountId(string accountId, Guid customerId)
		{
			var balance = await _dataHolderDatabaseContext.Balances.Include(x => x.Purses).Include(x => x.Account)
				.ThenInclude(x => x.Customer).AsNoTracking()
				.Where(t => t.AccountId == accountId && t.Account.CustomerId == customerId)
				.FirstOrDefaultAsync();

			return _mapper.Map<Balance>(balance);
		}

    }
}
