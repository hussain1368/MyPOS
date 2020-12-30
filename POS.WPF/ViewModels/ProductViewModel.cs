﻿using POS.DAL.Query;
using POS.DAL.Models;
using POS.WPF.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace POS.WPF.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        private readonly ProductQuery productQuery;
        private readonly OptionQuery optionQuery;

        public RelayCommandAsync LoadListCmd { get; set; }
        public RelayCommandAsync LoadOptionsCmd { get; set; }
        public RelayCommandAsync SaveCmd { get; set; }

        public ProductViewModel(ProductQuery productQuery, OptionQuery optionQuery)
        {
            this.productQuery = productQuery;
            this.optionQuery = optionQuery;
            AssignCommands();
        }

        private ProductModel product = new ProductModel();
        public ProductModel Product
        {
            get { return product; }
            set { product = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ProductDT> productsList;
        public ObservableCollection<ProductDT> ProductsList
        {
            get { return productsList; }
            set { productsList = value; OnPropertyChanged(); }
        }

        private IList<OptionValueDT> categoryList;
        public IList<OptionValueDT> CategoryList
        {
            get { return categoryList; }
            set { categoryList = value; OnPropertyChanged(); }
        }

        private void AssignCommands()
        {
            LoadListCmd = new RelayCommandAsync(async () =>
            {
                var data = await productQuery.GetList();
                ProductsList = new ObservableCollection<ProductDT>(data);
            });

            LoadOptionsCmd = new RelayCommandAsync(async () =>
            {
                CategoryList = await optionQuery.OptionsByTypeId(1);
            });

            SaveCmd = new RelayCommandAsync(async () =>
            {
                var data = new ProductDT
                {
                    Code = Product.Code,
                    CodeStatus = Product.CodeStatus,
                    Name = Product.Name,
                    Cost = Product.Cost.Value,
                    Profit = Product.Profit.Value,
                    Price = Product.Price.Value,
                    Discount = 0,
                    AlertQuantity = 0,
                    UnitId = null,
                    BrandId = null,
                    CategoryId = Product.CategoryId,
                    CurrencyId = 2,
                    ExpiryDate = null,
                    Note = null,
                    InsertedBy = 1,
                    InsertedDate = DateTime.Now,
                    UpdatedBy = null ,
                    UpdatedDate = null,
                    IsDeleted = false,
                };

                await productQuery.Create(data);

                LoadListCmd.Execute(null);
            });
        }
    }
}