﻿using POS.DAL.Query;

namespace POS.WPF.ViewModels
{
    public class AccountsVM : BaseViewModel
    {
        public AccountsVM(AccountQuery accountQuery, OptionQuery optionQuery)
        {
            this.accountQuery = accountQuery;
            this.optionQuery = optionQuery;
        }

        private readonly AccountQuery accountQuery;
        private readonly OptionQuery optionQuery;
    }
}
